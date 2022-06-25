using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
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

namespace InternalAuditSystem.Repository.Repository.EntityCategoryRepository
{
    public class EntityCategoryRepository : IEntityCategoryRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public EntityCategoryRepository(IDataRepository dataRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
        }

        /// <summary>
        /// Get all the entity categories under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of entity categories under an auditable entity</returns>
        public async Task<List<EntityCategoryAC>> GetAllEntityCategoryPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId)
        {
            List<EntityCategory> entityCategoryList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                entityCategoryList = await _dataRepository.Where<EntityCategory>(x => !x.IsDeleted && x.EntityId == selectedEntityId
                                                               && x.CategoryName.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                entityCategoryList.OrderBy(a => a.CategoryName);
            }
            else
            {
                //when only to get audit types data without searchstring - to show initial data on list page 

                entityCategoryList = await _dataRepository.Where<EntityCategory>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();
            }

            return _mapper.Map<List<EntityCategoryAC>>(entityCategoryList);

        }


        /// <summary>
        /// Get all the entity categories under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all entity categories </returns>
        public async Task<List<EntityCategoryAC>> GetAllEntityCategoryByEntityIdAsync(Guid selectedEntityId)
        {
            var entityCategoryList = await _dataRepository.Where<EntityCategory>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                                          .AsNoTracking().ToListAsync();
            return _mapper.Map<List<EntityCategoryAC>>(entityCategoryList);
        }


        /// <summary>
        /// Get data of a particular entity catgory under an auditable entity
        /// </summary>
        /// <param name="entityCategoryId">Id of the entity catgory</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular entity catgory data</returns>
        public async Task<EntityCategoryAC> GetEntityCategoryByIdAsync(Guid entityCategoryId, Guid selectedEntityId)
        {
            var result = await _dataRepository.FirstAsync<EntityCategory>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.Id == entityCategoryId);

            return _mapper.Map<EntityCategoryAC>(result);
        }

        /// <summary>
        /// Add new entity catgory under an auditable entity
        /// </summary>
        /// <param name="entityCategoryDetails">Details of entity catgory</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added entity catgory details</returns>
        public async Task<EntityCategoryAC> AddEntityCategoryAsync(EntityCategoryAC entityCategoryDetails, Guid selectedEntityId)
        {

            //check user already exist in db 
            if (!await _dataRepository.AnyAsync<EntityCategory>(x => !x.IsDeleted 
            && x.EntityId == selectedEntityId 
            && x.CategoryName.ToLower() == entityCategoryDetails.CategoryName.ToLower()))
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        var entityCategoryData = new EntityCategory()
                        {
                            CategoryName = entityCategoryDetails.CategoryName,
                            EntityId = selectedEntityId,
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy = currentUserId,
                            UserCreatedBy = null,
                            AuditableEntity = null                         
                        };

                        var addedEntityCategory = await _dataRepository.AddAsync<EntityCategory>(entityCategoryData);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();

                        return _mapper.Map<EntityCategoryAC>(addedEntityCategory);
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
                throw new DuplicateDataException(StringConstant.EntityCategoryFieldName, entityCategoryDetails.CategoryName);
            }

        }

        /// <summary>
        /// Update entity category under an auditable entity
        /// </summary>
        /// <param name="updatedEntityCategoryDetails">Updated details of entity type</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateEntityCategoryAsync(EntityCategoryAC updatedEntityCategoryDetails, Guid selectedEntityId)
        {

            var dbEntityCategory = await _dataRepository.Where<EntityCategory>(x => !x.IsDeleted && x.Id == updatedEntityCategoryDetails.Id).AsNoTracking().FirstAsync();

            var isToUpdateAuditType = dbEntityCategory.CategoryName.ToLower() == updatedEntityCategoryDetails.CategoryName.ToLower() ? true :
                !(await _dataRepository.AnyAsync<EntityCategory>(x => !x.IsDeleted && x.CategoryName.ToLower() == updatedEntityCategoryDetails.CategoryName.ToLower()));

            if (isToUpdateAuditType)
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        dbEntityCategory = _mapper.Map<EntityCategory>(updatedEntityCategoryDetails);
                        dbEntityCategory.UpdatedDateTime = DateTime.UtcNow;
                        dbEntityCategory.UpdatedBy = currentUserId;
                        dbEntityCategory.UserUpdatedBy = null;
                        dbEntityCategory.AuditableEntity = null;

                        _dataRepository.Update(dbEntityCategory);
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
                throw new DuplicateDataException(StringConstant.EntityTypeFieldName, updatedEntityCategoryDetails.CategoryName);
            }
        }

        /// <summary>
        /// Delete entity catgory from an auditable entity
        /// </summary>
        /// <param name="entityCategoryId">Id of the entity catgory that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteEntityCategoryAsync(Guid entityCategoryId, Guid selectedEntityId)
        {

            //check is this category is used in any active auditable entity or not 
            var totalEntityIncludeCurrententityCategory = await _dataRepository.CountAsync<AuditableEntity>(x => !x.IsDeleted && x.SelectedCategoryId == entityCategoryId);

            // if no enitty has this type as selected then do soft delete
            if (totalEntityIncludeCurrententityCategory == 0)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        var dbEntityCategoryData = await _dataRepository.FirstAsync<EntityCategory>(x => x.Id == entityCategoryId);

                        dbEntityCategoryData.IsDeleted = true;
                        dbEntityCategoryData.UpdatedDateTime = DateTime.UtcNow;
                        dbEntityCategoryData.UpdatedBy = currentUserId;

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
            //throw exception if this entity category is currently selected in any plan under the selected entity
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
        public async Task<int> GetTotalCountOfEntityCategorySearchStringWiseAsync(string searchString, Guid selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<EntityCategory>(x => !x.IsDeleted &&
                                                                                x.EntityId == selectedEntityId &&
                                                                                x.CategoryName.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<EntityCategory>(x => !x.IsDeleted && x.EntityId == selectedEntityId);
            }

            return totalRecords;
        }

        #region export to excel  auditable entity category
        /// <summary>
        /// Method for exporting  auditable entity category
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportEntityCategoriesAsync(string entityId, int timeOffset)
        {
            var entityCategoryList = await _dataRepository.Where<EntityCategory>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                          .Include(x => x.AuditableEntity).OrderByDescending(x=>x.CreatedDateTime).AsNoTracking().ToListAsync();
            var entityCategoryACList = _mapper.Map<List<EntityCategoryAC>>(entityCategoryList);
            if (entityCategoryACList.Count == 0)
            {
                EntityCategoryAC entityCategoryAC = new EntityCategoryAC();
                entityCategoryACList.Add(entityCategoryAC);
            }


            entityCategoryACList.ForEach(x =>
            {
                x.EntityName = x.Id != null ? entityCategoryList.FirstOrDefault(z => z.EntityId == x.EntityId)?.AuditableEntity.Name : string.Empty;
                x.CategoryName = x.Id != null ? x.CategoryName : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(entityCategoryACList, StringConstant.EntityCategoryFieldName + "(" + entityName + ")");
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

