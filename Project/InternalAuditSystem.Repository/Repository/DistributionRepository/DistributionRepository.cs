using AutoMapper;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using System.IO;
using InternalAuditSystem.DomailModel.Enums;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Repository.Repository.DistributionRepository
{
    public class DistributionRepository : IDistributionRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private readonly IAuditableEntityRepository _auditableEntityRepository;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        public DistributionRepository(IDataRepository dataRepository, IMapper mapper,
            IExportToExcelRepository exportToExcelRepository, IAuditableEntityRepository auditableEntityRepository,
            IHttpContextAccessor httpContextAccessor)
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
        /// Get distributor data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">search string </param>
        /// <returns>List of distributor</returns>
        public async Task<List<DistributorsAC>> GetDistributorsAsync(int? page, int pageSize, string searchString, string selectedEntityId)
        {
            List<DistributorsAC> distributorsACList = await GetAllDistributorsAsync(selectedEntityId);

            if (!string.IsNullOrEmpty(searchString))
            {
                distributorsACList = distributorsACList.Where(a => !a.IsDeleted && a.Name.ToLower().Contains(searchString.ToLower()))
                    .Skip((page - 1 ?? 0) * pageSize).Take(pageSize).OrderByDescending(a => a.CreatedDateTime).ToList();
            }
            else
            {
                distributorsACList = distributorsACList.Where(a => !a.IsDeleted).OrderByDescending(a => a.CreatedDateTime).ToList();
            }
            return distributorsACList;
        }

        /// <summary>
        /// Get count of distributor
        /// </summary>
        /// <param name="searchValue">Search value</param
        /// <param name="selectedEntityId">search string </param>
        /// <returns>count of distributors</returns>
        public async Task<int> GetDistributorsCountAsync(string searchValue, string selectedEntityId)
        {
            List<DistributorsAC> distributorsACList = await GetAllDistributorsAsync(selectedEntityId);
            int totalRecords;
            if (!string.IsNullOrEmpty(searchValue))
            {
                totalRecords = distributorsACList.Where<DistributorsAC>(a => !a.IsDeleted && a.Name.ToLower().Contains(searchValue.ToLower())).Count();
            }
            else
            {
                totalRecords = distributorsACList.Where<DistributorsAC>(a => !a.IsDeleted).Count();
            }
            return totalRecords;

        }

        /// <summary>
        /// Get users data
        /// </summary>
        /// <param name="selectedEntityId">search string </param>
        /// <returns>List of Users</returns>
        public async Task<List<EntityUserMappingAC>> GetUsersAsync(string selectedEntityId)
        {
            List<EntityUserMapping> entityUserMappingList = await _dataRepository.Where<EntityUserMapping>(a => !a.IsDeleted &&
            a.EntityId.ToString() == selectedEntityId).Include(a => a.User).AsNoTracking().ToListAsync();

            List<Distributors> distributorsList = await _dataRepository.Where<Distributors>(a => !a.IsDeleted && a.EntityId.ToString() == selectedEntityId).AsNoTracking().ToListAsync();

            HashSet<Guid> diffids = new HashSet<Guid>(distributorsList.Select(s => s.UserId));
            //You will have the difference here
            List<EntityUserMapping> userList = entityUserMappingList.Where(m => !diffids.Contains(m.UserId)).ToList();

            List<EntityUserMappingAC> entityUserMappingACList = _mapper.Map<List<EntityUserMappingAC>>(userList);

            return entityUserMappingACList;
        }

        /// <summary>
        /// Add distributor detail
        /// </summary>
        /// <param name="distributor">detail of distributor</param>
        /// <returns>Task</returns>
        public async Task AddDistributorsAsync(List<EntityUserMappingAC> distributors)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    List<Distributors> addDistributorsList = new List<Distributors>();
                    for (int i = 0; i < distributors.Count; i++)
                    {
                        Distributors distributor = new Distributors();
                        distributor.UserId = distributors[i].UserId;
                        distributor.EntityId = distributors[i].EntityId;
                        distributor.CreatedDateTime = DateTime.UtcNow;
                        distributor.CreatedBy = currentUserId;
                        addDistributorsList.Add(distributor);
                    }

                    await _dataRepository.AddRangeAsync<Distributors>(addDistributorsList);
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
        /// Delete distributor detail
        /// <param name="id">id of delete distributor</param>
        /// </summary>
        /// <returns>Task</returns>
        public async Task DeleteDistributorAsync(string id)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    Distributors distributorsToDelete = await _dataRepository.FirstOrDefaultAsync<Distributors>(x => x.Id.ToString().Equals(id));
                    distributorsToDelete.IsDeleted = true;
                    distributorsToDelete.UpdatedBy = currentUserId;
                    distributorsToDelete.UpdatedDateTime = DateTime.UtcNow;

                    //Check user is used or not
                    if (_dataRepository.Any<ReportUserMapping>(a => !a.IsDeleted
                    && a.UserId == distributorsToDelete.UserId))
                    {
                        throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.ReportText);
                    }

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
        /// Get distributors list 
        /// </summary>
        /// <returns>list of distributors</returns>
        public async Task<List<DistributorsAC>> GetAllDistributorsAsync(string entityId)
        {
            List<Distributors> distributorsList = await _dataRepository.Where<Distributors>(a => !a.IsDeleted && a.EntityId.ToString() == entityId)
                     .Include(a => a.User).AsNoTracking().ToListAsync();
            List<DistributorsAC> distributorsACList = _mapper.Map<List<DistributorsAC>>(distributorsList);
            return distributorsACList;
        }

        /// <summary>
        /// Export Distributors to Excel
        /// </summary>
        /// <param name="entityId">Id of entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> ExportDistributorsAsync(string entityId, int timeOffset)
        {
            List<Distributors> distributorsList = await _dataRepository.Where<Distributors>(a => !a.IsDeleted && a.EntityId.ToString() == entityId)
            .OrderByDescending(a => a.CreatedDateTime).Include(a => a.User).AsNoTracking().ToListAsync();
            List<DistributorsAC> exportDistributors = _mapper.Map<List<DistributorsAC>>(distributorsList);

            //convert UTC date time to system date time formate
            exportDistributors.ForEach(a =>
            {
                a.CreatedDate = a.CreatedDateTime != null ? a.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                a.UpdatedDate = a.UpdatedDateTime != null ? a.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            });
            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(exportDistributors, StringConstant.DistributorModule + "(" + entityName + ")");
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
