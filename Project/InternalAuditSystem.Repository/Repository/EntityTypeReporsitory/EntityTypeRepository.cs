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
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.EntityTypeReporsitory
{
    public class EntityTypeRepository : IEntityTypeRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public EntityTypeRepository(IDataRepository dataRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository= exportToExcelRepository;
        }

        /// <summary>
        /// Get all the entity types under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of entity types under an auditable entity</returns>
        public async Task<List<EntityTypeAC>> GetAllEntityTypesPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId)
        {
            List<EntityType> entityTypeList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                entityTypeList = await _dataRepository.Where<EntityType>(x => !x.IsDeleted && x.EntityId == selectedEntityId
                                                               && x.TypeName.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                entityTypeList.OrderBy(a => a.TypeName);
            }
            else
            {
                //when only to get audit types data without searchstring - to show initial data on list page 

                entityTypeList = await _dataRepository.Where<EntityType>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();
            }

            return _mapper.Map<List<EntityTypeAC>>(entityTypeList);

        }


        /// <summary>
        /// Get all the entity types under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all entity types </returns>
        public async Task<List<EntityTypeAC>> GetAllEntityTypeByEntityIdAsync(Guid selectedEntityId)
        {
            var entityTypeList = await _dataRepository.Where<EntityType>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                                          .AsNoTracking().ToListAsync();
            return _mapper.Map<List<EntityTypeAC>>(entityTypeList);
        }


        /// <summary>
        /// Get data of a particular entity type under an auditable entity
        /// </summary>
        /// <param name="entityTypeId">Id of the entity type</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular entity type data</returns>
        public async Task<EntityTypeAC> GetEntityTypeByIdAsync(Guid entityTypeId, Guid selectedEntityId)
        {
            var result = await _dataRepository.FirstAsync<EntityType>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.Id == entityTypeId);

            return _mapper.Map<EntityTypeAC>(result);
        }

        /// <summary>
        /// Add new entity type under an auditable entity
        /// </summary>
        /// <param name="entityTypeDetails">Details of entity type</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added entity type details</returns>
        public async Task<EntityTypeAC> AddEntityTypeAsync(EntityTypeAC entityTypeDetails, Guid selectedEntityId)
        {

            //check user already exist in db 
            if (!await _dataRepository.AnyAsync<EntityType>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.TypeName.ToLower() == entityTypeDetails.TypeName.ToLower()))
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        var entityTypeData = new EntityType()
                        {
                            TypeName = entityTypeDetails.TypeName,
                            EntityId = selectedEntityId,
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy = currentUserId,
                            UserCreatedBy = null,
                            AuditableEntity=null
                        };

                        var addedEntityType = await _dataRepository.AddAsync<EntityType>(entityTypeData);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();

                        return _mapper.Map<EntityTypeAC>(addedEntityType);
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
                throw new DuplicateDataException(StringConstant.EntityTypeFieldName, entityTypeDetails.TypeName);
            }

        }

        /// <summary>
        /// Update entity type under an auditable entity
        /// </summary>
        /// <param name="updatedEntityTypeDetails">Updated details of entity type</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateEntityTypeAsync(EntityTypeAC updatedEntityTypeDetails, Guid selectedEntityId)
        {

            var dbEntityType = await _dataRepository.Where<EntityType>(x => !x.IsDeleted && x.Id == updatedEntityTypeDetails.Id).AsNoTracking().FirstAsync();

            var isToUpdateAuditType = dbEntityType.TypeName.ToLower() == updatedEntityTypeDetails.TypeName.ToLower() ? true :
                !(await _dataRepository.AnyAsync<EntityType>(x => !x.IsDeleted && x.TypeName.ToLower() == updatedEntityTypeDetails.TypeName.ToLower()));

            if (isToUpdateAuditType)
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        dbEntityType = _mapper.Map<EntityType>(updatedEntityTypeDetails);
                        dbEntityType.UpdatedDateTime = DateTime.UtcNow;
                        dbEntityType.UpdatedBy = currentUserId;
                        dbEntityType.UserUpdatedBy = null;
                        dbEntityType.AuditableEntity = null;

                        _dataRepository.Update(dbEntityType);
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
                throw new DuplicateDataException(StringConstant.EntityTypeFieldName, updatedEntityTypeDetails.TypeName);
            }
        }

        /// <summary>
        /// Delete entity type from an auditable entity
        /// </summary>
        /// <param name="entityTypeId">Id of the entity type that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteEntityTypeAsync(Guid entityTypeId, Guid selectedEntityId)
        {

            //check is this type is used in any active auditable entity or not 
            var totalEntityIncludeCurrentEntityType = await _dataRepository.CountAsync<AuditableEntity>(x => !x.IsDeleted && x.SelectedTypeId == entityTypeId);

            // if no enitty has this type as selected then do soft delete
            if (totalEntityIncludeCurrentEntityType == 0)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        var dbAuditTypeData = await _dataRepository.FirstAsync<EntityType>(x => x.EntityId == selectedEntityId && x.Id == entityTypeId);

                        dbAuditTypeData.IsDeleted = true;
                        dbAuditTypeData.UpdatedDateTime = DateTime.UtcNow;
                        dbAuditTypeData.UpdatedBy = currentUserId;

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
            //throw exception if this audit type is currently selected in any plan under the selected entity
            else
            {
                throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.AuditableEntityModuleName);
            }


        }

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalCountOfEntityTypeSearchStringWiseAsync(string searchString, Guid selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<EntityType>(x => !x.IsDeleted &&
                                                                                x.EntityId == selectedEntityId &&
                                                                                x.TypeName.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<EntityType>(x => !x.IsDeleted && x.EntityId == selectedEntityId);
            }

            return totalRecords;
        }

        #region export to excel  auditable entity type
        /// <summary>
        /// Method for exporting  auditable entity type
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportEntityTypesAsync(string entityId, int timeOffset)
        {
            var entityTypeList = await _dataRepository.Where<EntityType>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                          .Include(x => x.AuditableEntity).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var entityTypeACList = _mapper.Map<List<EntityTypeAC>>(entityTypeList);
            if (entityTypeACList.Count == 0)
            {
                EntityTypeAC entityTypeAC = new EntityTypeAC();
                entityTypeACList.Add(entityTypeAC);
            }


            entityTypeACList.ForEach(x =>
            {
                x.EntityName = x.Id != null ? entityTypeList.FirstOrDefault(z => z.EntityId == x.EntityId)?.AuditableEntity.Name : string.Empty;
                x.TypeName = x.Id != null ? x.TypeName : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(entityTypeACList, StringConstant.EntityTypeFieldName + "(" + entityName + ")");
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
