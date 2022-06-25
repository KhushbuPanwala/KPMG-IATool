using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.RelationshipTypeRepository
{
    public class RelationshipTypeRepository : IRelationshipTypeRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public RelationshipTypeRepository(IDataRepository dataRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;

        }

        /// <summary>
        /// Get all the relationship types under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of relationship types under an auditable entity</returns>
        public async Task<List<RelationshipTypeAC>> GetAllRelationshipTypesPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId)
        {
            List<RelationshipType> relationshipTypeList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                relationshipTypeList = await _dataRepository.Where<RelationshipType>(x => !x.IsDeleted && x.EntityId == selectedEntityId
                                                               && x.Name.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                relationshipTypeList.OrderBy(a => a.Name);
            }
            else
            {
                //when only to get audit types data without searchstring - to show initial data on list page 

                relationshipTypeList = await _dataRepository.Where<RelationshipType>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();
            }

            return _mapper.Map<List<RelationshipTypeAC>>(relationshipTypeList);

        }


        // <summary>
        /// Get all the relationship types under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all relationship types </returns>
        public async Task<List<RelationshipTypeAC>> GetAllRelationshipTypeByEntityIdAsync(Guid selectedEntityId)
        {
            var relationshipTypeList = await _dataRepository.Where<RelationshipType>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                                          .AsNoTracking().ToListAsync();
            return _mapper.Map<List<RelationshipTypeAC>>(relationshipTypeList);
        }


        /// <summary>
        /// Get data of a particular relationship type under an auditable entity
        /// </summary>
        /// <param name="relationshipTypeId">Id of the relationship type</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular relationship type data</returns>
        public async Task<RelationshipTypeAC> GetRelationshipTypeByIdAsync(Guid relationshipTypeId, Guid selectedEntityId)
        {
            var result = await _dataRepository.FirstAsync<RelationshipType>(x => !x.IsDeleted && x.Id == relationshipTypeId);

            return _mapper.Map<RelationshipTypeAC>(result);
        }


        /// <summary>
        /// Add new relationship type under an auditable entity
        /// </summary>
        /// <param name="relationshipTypeDetails">Details of relationship type</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added relationship type details</returns>
        public async Task<RelationshipTypeAC> AddRelationshipTypeAsync(RelationshipTypeAC relationshipTypeDetails, Guid selectedEntityId)
        {

            //check user already exist in db 
            if (!await _dataRepository.AnyAsync<RelationshipType>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.Name.ToLower() == relationshipTypeDetails.Name.ToLower()))
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        var relationshipTypeData = new RelationshipType()
                        {
                            Name = relationshipTypeDetails.Name,
                            EntityId = selectedEntityId,
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy = currentUserId
                        };

                        var addedEntityType = await _dataRepository.AddAsync<RelationshipType>(relationshipTypeData);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();

                        return _mapper.Map<RelationshipTypeAC>(addedEntityType);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                throw new DuplicateDataException(StringConstant.RelationshipTypeFieldName, relationshipTypeDetails.Name);
            }

        }

        /// <summary>
        /// Update relationship type under an auditable entity
        /// </summary>
        /// <param name="updatedRelationshipTypeDetails">Updated details of relationship type</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateRelationshipTypeAsync(RelationshipTypeAC updatedRelationshipTypeDetails, Guid selectedEntityId)
        {

            var dbRelationshipType = await _dataRepository.Where<RelationshipType>(x => !x.IsDeleted && x.Id == updatedRelationshipTypeDetails.Id).AsNoTracking().FirstAsync();

            var isToUpdateRelationshipType = dbRelationshipType.Name.ToLower() == updatedRelationshipTypeDetails.Name.ToLower() ? true :
                !(await _dataRepository.AnyAsync<RelationshipType>(x => !x.IsDeleted && x.Name.ToLower() == updatedRelationshipTypeDetails.Name.ToLower()));

            if (isToUpdateRelationshipType)
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        dbRelationshipType = _mapper.Map<RelationshipType>(updatedRelationshipTypeDetails);
                        dbRelationshipType.UpdatedDateTime = DateTime.UtcNow;
                        dbRelationshipType.UpdatedBy = currentUserId;
                        dbRelationshipType.UserUpdatedBy = null;
                        dbRelationshipType.AuditableEntity = null;

                        _dataRepository.Update(dbRelationshipType);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
            else
            {
                throw new DuplicateDataException(StringConstant.RelationshipTypeFieldName, updatedRelationshipTypeDetails.Name);
            }
        }

        /// <summary>
        /// Delete relationship type from an auditable entity
        /// </summary>
        /// <param name="relationshipTypeId">Id of the relationship type that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteRelationshipTypeAsync(Guid relationshipTypeId, Guid selectedEntityId)
        {

            //check is this type is used in any active auditable entity or not 
            var totalUsedCurrentRelationshipType = await _dataRepository.CountAsync<EntityRelationMapping>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.RelationTypeId == relationshipTypeId);

            // if no enitty has this type as selected then do soft delete
            if (totalUsedCurrentRelationshipType == 0)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        var dbRelationshipType = await _dataRepository.FirstAsync<RelationshipType>(x => x.EntityId == selectedEntityId && x.Id == relationshipTypeId);

                        dbRelationshipType.IsDeleted = true;
                        dbRelationshipType.UpdatedDateTime = DateTime.UtcNow;
                        dbRelationshipType.UpdatedBy = currentUserId;
                        dbRelationshipType.UserUpdatedBy = null;
                        dbRelationshipType.AuditableEntity = null;

                        await _dataRepository.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            //throw exception if this relation type is currently linked to other entity with this seletced entity
            else
            {
                throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.EntityRelationModuleName);
            }
        }

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalCountOfRelationshipTypeSearchStringWiseAsync(string searchString, Guid selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<RelationshipType>(x => !x.IsDeleted &&
                                                                                x.EntityId == selectedEntityId &&
                                                                                x.Name.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<RelationshipType>(x => !x.IsDeleted && x.EntityId == selectedEntityId);
            }

            return totalRecords;
        }

        #region export to excel  auditable entity relationship type
        /// <summary>
        /// Method for exporting  auditable entity relationship type
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportEntityRelationShipTypeAsync(string entityId, int timeOffset)
        {
            var relationTypeList = await _dataRepository.Where<RelationshipType>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                          .Include(x => x.AuditableEntity).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var relationTypeACList = _mapper.Map<List<RelationshipTypeAC>>(relationTypeList);
            if (relationTypeACList.Count == 0)
            {
                RelationshipTypeAC entityRelationTypeAC = new RelationshipTypeAC();
                relationTypeACList.Add(entityRelationTypeAC);
            }

            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            relationTypeACList.ForEach(x =>
            {
                x.EntityName = x.Id != null ? entityName : string.Empty;
                x.Name = x.Id != null ? x.Name : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

          
            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(relationTypeACList, StringConstant.RelationshipTypeFieldName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
