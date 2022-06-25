using AutoMapper;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Repository.Repository.RatingRepository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        private readonly IAuditableEntityRepository _auditableEntityRepository;
        private readonly IMapper _mapper;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;

        public RatingRepository(IDataRepository dataRepository, IMapper mapper,
            IExportToExcelRepository exportToExcelRepository, IAuditableEntityRepository auditableEntityRepository, IHttpContextAccessor httpContextAccessor)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _auditableEntityRepository = auditableEntityRepository;
            _exportToExcelRepository = exportToExcelRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
        }

        #region Public Methods

        /// <summary>
        /// Get Ratings data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">search string </param>
        /// <returns>List of ratings</returns>
        public async Task<List<RatingAC>> GetRatingsAsync(int? page, int pageSize, string searchString, string selectedEntityId)
        {
            List<Rating> ratingsList;
            if (!string.IsNullOrEmpty(searchString))
            {
                ratingsList = await _dataRepository.Where<Rating>(a => !a.IsDeleted && a.Ratings.ToLower().Contains(searchString.ToLower())
                    && a.EntityId.ToString() == selectedEntityId).Skip((page - 1 ?? 0) * pageSize)
                  .Take(pageSize).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            }
            else
            {
                ratingsList = await _dataRepository.Where<Rating>(a => !a.IsDeleted && a.EntityId.ToString() == selectedEntityId)
                  .OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            }
            return _mapper.Map<List<RatingAC>>(ratingsList);
        }

        /// <summary>
        /// Get count of ratings
        /// </summary>
        /// <param name="searchValue">Search value</param>
        /// <param name="selectedEntityId">search string </param>
        /// <returns>count of ratings</returns>
        public async Task<int> GetRatingCountAsync(string searchString, string selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.Where<Rating>(a => !a.IsDeleted && a.Ratings.ToLower().Contains(searchString.ToLower())
                && a.EntityId.ToString() == selectedEntityId).AsNoTracking().CountAsync();
            }
            else
            {
                totalRecords = await _dataRepository.Where<Rating>(a => !a.IsDeleted && a.EntityId.ToString() == selectedEntityId).AsNoTracking().CountAsync();
            }
            return totalRecords;

        }


        /// <summary>
        /// Get detail of ratings
        /// </summary>
        /// <param name="ratingId">Rating id</param>
        /// <returns>RatingAC</returns>
        public async Task<RatingAC> GetRatingsByIdAsync(string ratingId)
        {
            Rating ratingDetail = await _dataRepository.FirstOrDefaultAsync<Rating>(a => a.Id.ToString() == ratingId && !a.IsDeleted);
            return _mapper.Map<RatingAC>(ratingDetail);
        }


        /// <summary>
        /// Delete rating detail
        /// <param name="id">id of delete rating</param>
        /// </summary>
        /// <returns>Task</returns>
        public async Task DeleteRatingAsync(string id)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    Rating ratingToDelete = await _dataRepository.FirstOrDefaultAsync<Rating>(x => x.Id.ToString().Equals(id));
                    ratingToDelete.IsDeleted = true;
                    ratingToDelete.UpdatedBy = currentUserId;
                    ratingToDelete.UpdatedDateTime = DateTime.UtcNow;

                    //Check rating is used or not
                    CheckRatingReferenceExist(ratingToDelete);

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

        /// <summary>
        /// Add Rating detail
        /// </summary>
        /// <param name="rating">Details of added rating</param>
        /// <returns>Return Rating AC detail</returns>
        public async Task<RatingAC> AddRatingAsync(RatingAC ratingAC)
        {
            if (CheckRatingExist(ratingAC.Ratings, ratingAC.EntityId.ToString(), null))
            {
                throw new DuplicateDataException(StringConstant.RatingText, ratingAC.Ratings);
            }
            else
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        Rating rating = _mapper.Map<Rating>(ratingAC);

                        rating.CreatedBy = currentUserId;
                        rating.CreatedDateTime = DateTime.UtcNow;
                        await _dataRepository.AddAsync<Rating>(rating);
                        await _dataRepository.SaveChangesAsync();

                        transaction.Commit();
                        return _mapper.Map<RatingAC>(rating);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// Update Rating detail
        /// </summary>
        /// <param name="rating">Updated rating Detail</param>
        /// <returns>Return Rating AC Detail</returns>
        public async Task<RatingAC> UpdateRatingAsync(RatingAC ratingAC)
        {
            if (CheckRatingExist(ratingAC.Ratings, ratingAC.EntityId.ToString(), ratingAC.Id.ToString()))
            {
                throw new DuplicateDataException(StringConstant.RatingText, ratingAC.Ratings);
            }
            else
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        Rating rating = _mapper.Map<Rating>(ratingAC);

                        rating.UpdatedBy = currentUserId;
                        rating.UpdatedDateTime = DateTime.UtcNow;

                        _dataRepository.Update<Rating>(rating);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();
                        return _mapper.Map<RatingAC>(rating);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Get Ratings data
        /// </summary>
        /// <param name="entityId">Id of entity</param>
        /// <returns>List of ratings</returns>
        public async Task<List<RatingAC>> GetRatingsByEntityIdAsync(string entityId)
        {
            List<Rating> ratingsList = await _dataRepository.Where<Rating>(a => !a.IsDeleted && a.EntityId.ToString() == entityId).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            return _mapper.Map<List<RatingAC>>(ratingsList);
        }


        /// <summary>
        /// Export Ratings to Excel
        /// </summary>
        /// <param name="entityId">Id of entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> ExportRatingsAsync(string entityId, int timeOffset)
        {
            List<Rating> ratingsList = await _dataRepository.Where<Rating>(a => !a.IsDeleted && a.EntityId.ToString() == entityId).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            List<RatingAC> exportRatings = _mapper.Map<List<RatingAC>>(ratingsList);

            //convert UTC date time to system date time formate
            exportRatings.ForEach(a =>
            {
                a.CreatedDate = a.CreatedDateTime != null ? a.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                a.UpdatedDate = a.UpdatedDateTime != null ? a.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            });

            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(exportRatings, StringConstant.RatingModule + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Check rating already exist or notea
        /// </summary>
        /// <param name="name">rating name</param>
        /// <param name="entityId">entity id</param>
        /// <returns>Return boolean value for rating exist or not </returns>
        private bool CheckRatingExist(string name, string entityId, string ratingId)
        {
            bool exist;
            if (ratingId != null)
            {
                exist = _dataRepository.Any<Rating>(a => a.Ratings.ToLower().Equals(name.ToLower())
                  && a.EntityId.ToString() == entityId && !a.IsDeleted
                && a.Id.ToString() != ratingId);
            }
            else
            {
                exist = _dataRepository.Any<Rating>(a => a.Ratings.ToLower().Equals(name.ToLower())
                && a.EntityId.ToString() == entityId && !a.IsDeleted);
            }
            return exist;
        }
        /// <summary>
        /// Check rating reference exist or not
        /// </summary>
        /// <param name="rating">rating detail</param>
        /// <returns></returns>
        private void CheckRatingReferenceExist(Rating rating)
        {
            if (_dataRepository.Any<Report>(a => !a.IsDeleted && a.RatingId == rating.Id))
            {
                throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.ReportText);
            }
            else if (_dataRepository.Any<ReportObservation>(a => !a.IsDeleted && a.RatingId == rating.Id))
            {
                throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.ReportObservationText);
            }
        }

        #endregion

    }
}
