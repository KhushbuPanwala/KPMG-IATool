using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.Common;
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

namespace InternalAuditSystem.Repository.Repository.ObservationCategoryRepository
{
  public  class ObservationCategoryRepository:IObservationCategoryRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IGlobalRepository _globalRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public ObservationCategoryRepository(IDataRepository dataRepository, IMapper mapper, IGlobalRepository globalRepository, IHttpContextAccessor httpContextAccessor,
            IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _globalRepository = globalRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
        }

        #region Observation Category CRUD

        /// <summary>
        /// Get all the observation category  with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <returns>List of observation categories</returns>
        public async Task<Pagination<ObservationCategoryAC>> GetAllObservationCategoriesPageWiseAndSearchWiseAsync(Pagination<ObservationCategoryAC> pagination)
        {
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);
            //return list as per search string 

            IQueryable<ObservationCategoryAC> observationCategoryACList = _dataRepository.Where<ObservationCategory>(x => !x.IsDeleted
           && x.EntityId == pagination.EntityId &&
           (!String.IsNullOrEmpty(pagination.searchText) ? x.CategoryName.ToLower().Contains(pagination.searchText.ToLower()) : true))
           .Select(x =>
                                                new ObservationCategoryAC
                                                {
                                                    Id = x.Id,
                                                    CategoryName = x.CategoryName,
                                                    EntityId = x.EntityId,
                                                    CreatedDateTime = x.CreatedDateTime
                                                });

            pagination.TotalRecords = observationCategoryACList.Count();

            pagination.Items = await observationCategoryACList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToListAsync();
            return pagination;

        }


        /// <summary>
        /// Get all the ObservationCategory under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all ObservationCategory </returns>
        public async Task<List<ObservationCategoryAC>> GetAllObservationCategoryByEntityIdAsync(Guid selectedEntityId)
        {
            var regionList = await _dataRepository.Where<ObservationCategory>(x => !x.IsDeleted && x.EntityId == selectedEntityId).Distinct().OrderByDescending(x => x.CreatedDateTime)
                                                          .AsNoTracking().ToListAsync();
            return _mapper.Map<List<ObservationCategoryAC>>(regionList);
        }


        /// <summary>
        /// Get data of a particular ObservationCategory 
        /// </summary>
        /// <param name="observationCategoryId">Id of the ObservationCategory</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular ObservationCategory data</returns>
        public async Task<ObservationCategoryAC> GetObservationCategoryDetailsByIdAsync(string observationCategoryId, string selectedEntityId)
        {
            var result = await _dataRepository.FirstAsync<ObservationCategory>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == observationCategoryId);

            return _mapper.Map<ObservationCategoryAC>(result);
        }

        /// <summary>
        /// Add new ObservationCategory under an auditable entity
        /// </summary>
        /// <param name="categoryDetails">Details of ObservationCategory</param>
        /// <returns>Added ObservationCategory details</returns>
        public async Task<ObservationCategoryAC> AddObservationCategoryAsync(ObservationCategoryAC categoryDetails)
        {

            //check user already exist in db 
            if (!await _dataRepository.AnyAsync<ObservationCategory>(x => !x.IsDeleted
            && x.EntityId == categoryDetails.EntityId
            && x.CategoryName.ToLower() == categoryDetails.CategoryName.ToLower()))
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        var observationCategoryData = new ObservationCategory()
                        {
                            CategoryName = categoryDetails.CategoryName,
                            EntityId = categoryDetails.EntityId,
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy =currentUserId
                        };

                        var addedCategory = await _dataRepository.AddAsync<ObservationCategory>(observationCategoryData);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();

                        return _mapper.Map<ObservationCategoryAC>(addedCategory);
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
                throw new DuplicateDataException(StringConstant.ObservationCategoryModuleName, categoryDetails.CategoryName);
            }

        }

        /// <summary>
        /// Update ObservationCategory under an auditable entity
        /// </summary>
        /// <param name="updatedObservationCategoryDetails">Updated details of ObservationCategory</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateObservationCategoryAsync(ObservationCategoryAC updatedObservationCategoryDetails)
        {

            var dbObservationCategoryData = await _dataRepository.Where<ObservationCategory>(x => !x.IsDeleted && x.Id == updatedObservationCategoryDetails.Id).AsNoTracking().FirstAsync();

            var isToUpdateRegion = dbObservationCategoryData.CategoryName.ToLower() == updatedObservationCategoryDetails.CategoryName.ToLower() ? true :
                !(await _dataRepository.AnyAsync<ObservationCategory>(x => !x.IsDeleted && x.CategoryName.ToLower() == updatedObservationCategoryDetails.CategoryName.ToLower()));

            if (isToUpdateRegion)
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        dbObservationCategoryData = _mapper.Map<ObservationCategory>(updatedObservationCategoryDetails);
                        dbObservationCategoryData.UpdatedDateTime = DateTime.UtcNow;
                        dbObservationCategoryData.UpdatedBy = currentUserId;

                        _dataRepository.Update(dbObservationCategoryData);
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
                throw new DuplicateDataException(StringConstant.ObservationCategoryModuleName, updatedObservationCategoryDetails.CategoryName);
            }
        }

        /// <summary>
        /// Delete ObservationCategory from an auditable entity
        /// </summary>
        /// <param name="observationCategoryId">Id of the ObservationCategory that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteObservationCategoryAsync(string observationCategoryId, string selectedEntityId)
        {

            var categoryUsedInObservation = await _dataRepository.CountAsync<Observation>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.ObservationCategoryId == Guid.Parse(observationCategoryId));
            var categoryUsedInReportObservation = await _dataRepository.CountAsync<ReportObservation>(x => !x.IsDeleted  && x.ObservationCategoryId == Guid.Parse(observationCategoryId));
            // if no enitty has this ObservationCategory as selected then do soft delete
            if (categoryUsedInObservation == 0 && categoryUsedInReportObservation==0)
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        var dbObservationCategoryData = await _dataRepository.FirstAsync<ObservationCategory>(x => x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == observationCategoryId);

                        dbObservationCategoryData.IsDeleted = true;
                        dbObservationCategoryData.UpdatedDateTime = DateTime.UtcNow;
                        dbObservationCategoryData.UpdatedBy = currentUserId;

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
            
            else
            {
                //throw exception if this ObservationCategory is currently selected in any observation under the selected entity
                if (categoryUsedInObservation > 0)
                    throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.ObservationModuleName);
                else
                    //throw exception if this ObservationCategory is currently selected in any report observation under the selected entity
                    throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.ReportObservationModule);
            }


        }

        #region export to excel observation category
        /// <summary>
        /// Method for exporting  auditable observation category
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportObservationCategoriesAsync(string entityId, int timeOffset)
        {
            var observationCategoryList = await _dataRepository.Where<ObservationCategory>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                          .Include(x => x.AuditableEntity).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var observationCategoryACList = _mapper.Map<List<ObservationCategoryAC>>(observationCategoryList);
            if (observationCategoryACList.Count == 0)
            {
                ObservationCategoryAC observationCategoryAC = new ObservationCategoryAC();
                observationCategoryACList.Add(observationCategoryAC);
            }

            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            observationCategoryACList.ForEach(x =>
            {
                x.EntityName = x.Id != null ? entityName : string.Empty;
                x.CategoryName = x.Id != null ? x.CategoryName : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });


            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(observationCategoryACList, StringConstant.ObservationCategoryModuleName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #endregion
    }
}
