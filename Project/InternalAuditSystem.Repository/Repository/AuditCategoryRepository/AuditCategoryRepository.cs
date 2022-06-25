using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
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

namespace InternalAuditSystem.Repository.Repository.AuditCategoryRepository
{
    public class AuditCategoryRepository : IAuditCategoryRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        private readonly IAuditableEntityRepository _auditableEntityRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        public AuditCategoryRepository(IDataRepository dataRepository, IMapper mapper, IExportToExcelRepository exportToExcelRepository, IAuditableEntityRepository auditableEntityRepository,
             IHttpContextAccessor httpContextAccessor)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _exportToExcelRepository = exportToExcelRepository;
            _auditableEntityRepository = auditableEntityRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
        }

        /// <summary>
        /// Get all the audit categories under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit types under an auditable entity</returns>
        public async Task<List<AuditCategoryAC>> GetAllAuditCategoriesPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, string selectedEntityId)
        {
            List<AuditCategory> auditCategoryList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                auditCategoryList = await _dataRepository.Where<AuditCategory>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId
                                                               && x.Name.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                auditCategoryList.OrderBy(a => a.Name);
            }
            else
            {
                //when only to get audit types data without searchstring - to show initial data on list page 

                auditCategoryList = await _dataRepository.Where<AuditCategory>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId)
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();
            }

            return _mapper.Map<List<AuditCategoryAC>>(auditCategoryList);

        }

        /// <summary>
        /// Get alll the audit categories under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all audit categories </returns>
        public async Task<List<AuditCategoryAC>> GetAllAuditCategoriesByEntityIdAsync(Guid selectedEntityId)
        {
            var auditCategoryList = await _dataRepository.Where<AuditCategory>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                                          .Select(x => new AuditCategoryAC { Id = x.Id, Name = x.Name })
                                                          .AsNoTracking().ToListAsync();
            return _mapper.Map<List<AuditCategoryAC>>(auditCategoryList);
        }


        /// <summary>
        /// Get data of a particular audit category under an auditable entity
        /// </summary>
        /// <param name="auditCategoryId">Id of the audit category</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular aduit category data</returns>
        public async Task<AuditCategoryAC> GetAuditCategoryByIdAsync(string auditCategoryId, string selectedEntityId)
        {
            var result = await _dataRepository.FirstAsync<AuditCategory>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == auditCategoryId);

            return _mapper.Map<AuditCategoryAC>(result);
        }

        /// <summary>
        /// Add new audit category under an auditable entity
        /// </summary>
        /// <param name="auditCategoryDetails">Details of audit category/param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit category details</returns>
        public async Task<AuditCategoryAC> AddAuditCategoryAsync(AuditCategoryAC auditCategoryDetails, string selectedEntityId)
        {

            //check user already exist in db 
            if (!await _dataRepository.AnyAsync<AuditCategory>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.Name.ToLower() == auditCategoryDetails.Name.ToLower()))
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        var auditCategoryData = new AuditCategory()
                        {
                            Name = auditCategoryDetails.Name,
                            EntityId = new Guid(selectedEntityId),
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy = currentUserId,
                            AuditableEntity = null
                        };

                        var addedAuditType = await _dataRepository.AddAsync<AuditCategory>(auditCategoryData);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();

                        return _mapper.Map<AuditCategoryAC>(addedAuditType);
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
                throw new DuplicateDataException(StringConstant.AuditCategoryFieldName, auditCategoryDetails.Name);
            }

        }

        /// <summary>
        /// Update audit category under an auditable entity
        /// </summary>
        /// <param name="updatedAuditCategoryDetails">Updated details of audit category</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateAuditCategoryAsync(AuditCategoryAC updatedAuditCategoryDetails, string selectedEntityId)
        {
            var dbAuditCategory = await _dataRepository.Where<AuditCategory>(x => !x.IsDeleted && x.Id == updatedAuditCategoryDetails.Id).AsNoTracking().FirstAsync();

            var isToUpdateAuditCategory = dbAuditCategory.Name.ToLower() == updatedAuditCategoryDetails.Name.ToLower() ? true :
                !(await _dataRepository.AnyAsync<AuditCategory>(x => !x.IsDeleted && x.Name.ToLower() == updatedAuditCategoryDetails.Name.ToLower()));

            if (isToUpdateAuditCategory)
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        dbAuditCategory = _mapper.Map<AuditCategory>(updatedAuditCategoryDetails);
                        dbAuditCategory.UpdatedDateTime = DateTime.UtcNow;
                        dbAuditCategory.UpdatedBy = currentUserId;
                        dbAuditCategory.AuditableEntity = null;
                        _dataRepository.Update(dbAuditCategory);
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
                throw new DuplicateDataException(StringConstant.AuditCategoryFieldName, updatedAuditCategoryDetails.Name);
            }
        }

        /// <summary>
        /// Delete audit category from an auditable entity
        /// </summary>
        /// <param name="auditCategoryId">Id of the audit category that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteAuditCategoryAsync(string auditCategoryId, string selectedEntityId)
        {

            //check is this category is used in any active audit plan or not 
            var totalAuditPlansIncludeCurrentAuditCategory = await _dataRepository.CountAsync<AuditPlan>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.SelectCategoryId.ToString() == auditCategoryId);

            // if no plan has this category as selected then do soft delete
            if (totalAuditPlansIncludeCurrentAuditCategory == 0)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        var dbAuditTypeData = await _dataRepository.FirstAsync<AuditCategory>(x => x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == auditCategoryId);

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
            //throw exception if this audit category is currently selected in any plan under the selected entity
            else
            {
                throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.AuditPlanModuleName);
            }


        }

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalCountOfAuditCategorySearchStringWiseAsync(string searchString, string selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<AuditCategory>(x => !x.IsDeleted &&
                                                                                x.EntityId.ToString() == selectedEntityId &&
                                                                                x.Name.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<AuditCategory>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId);
            }

            return totalRecords;
        }

        /// <summary>
        /// Method for exporting audit categories
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportAuditCategoriesAsync(string entityId, int timeOffset)
        {
            var auditCategoriesList = await _dataRepository.Where<AuditCategory>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                         .AsNoTracking().ToListAsync();
            var auditCategoriesACList = _mapper.Map<List<AuditCategoryAC>>(auditCategoriesList);
            if (auditCategoriesACList.Count == 0)
            {
                AuditCategoryAC auditCategoryAC = new AuditCategoryAC();
                auditCategoriesACList.Add(auditCategoryAC);
            }
            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            auditCategoriesACList.ForEach(x =>
            {
                x.Name = x.Id != null ? x.Name : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            });

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(auditCategoriesACList, StringConstant.AuditCategoryFieldName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
