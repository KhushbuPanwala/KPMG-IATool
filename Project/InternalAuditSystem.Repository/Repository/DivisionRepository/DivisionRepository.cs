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

namespace InternalAuditSystem.Repository.Repository.DivisionRepository
{
    public class DivisionRepository : IDivisionRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public DivisionRepository(IDataRepository dataRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
        }

        /// <summary>
        /// Get all the divisions under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of divisions under an auditable entity</returns>
        public async Task<List<DivisionAC>> GetAllDivisionPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId)
        {
            List<Division> divisionList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                divisionList = await _dataRepository.Where<Division>(x => !x.IsDeleted && x.EntityId == selectedEntityId
                                                               && x.Name.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                divisionList.OrderBy(a => a.Name);
            }
            else
            {
                //when only to get divisions data without searchstring - to show initial data on list page 

                divisionList = await _dataRepository.Where<Division>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();
            }

            return _mapper.Map<List<DivisionAC>>(divisionList);

        }


        /// <summary>
        /// Get alll the divisions under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all divisions</returns>
        public async Task<List<DivisionAC>> GetAllDivisionByEntityIdAsync(Guid selectedEntityId)
        {
            var divisionList = await _dataRepository.Where<Division>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                                          .AsNoTracking().ToListAsync();
            return _mapper.Map<List<DivisionAC>>(divisionList);
        }


        /// <summary>
        /// Get data of a particular division under an auditable entity
        /// </summary>
        /// <param name="divisionId">Id of the division</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular division data</returns>
        public async Task<DivisionAC> GetDivisionByIdAsync(Guid divisionId, Guid selectedEntityId)
        {
            var result = await _dataRepository.FirstAsync<Division>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.Id == divisionId);

            return _mapper.Map<DivisionAC>(result);
        }

        /// <summary>
        /// Add new division under an auditable entity
        /// </summary>
        /// <param name="divisionDetails">Details of division</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added division details</returns>
        public async Task<DivisionAC> AddDivisionAsync(DivisionAC divisionDetails, Guid selectedEntityId)
        {

            //check user already exist in db 
            if (!await _dataRepository.AnyAsync<Division>(x => !x.IsDeleted
            && x.EntityId == selectedEntityId
            && x.Name.ToLower() == divisionDetails.Name.ToLower()))
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        var divisionData = new Division()
                        {
                            Name = divisionDetails.Name,
                            EntityId = selectedEntityId,
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy = currentUserId,
                            AuditableEntity= null,
                            UserCreatedBy= null
                        };

                        var addedDivisionData = await _dataRepository.AddAsync<Division>(divisionData);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();

                        return _mapper.Map<DivisionAC>(addedDivisionData);
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
                throw new DuplicateDataException(StringConstant.divisionFieldName, divisionDetails.Name);
            }

        }

        /// <summary>
        /// Update division under an auditable entity
        /// </summary>
        /// <param name="updatedDivisionDetails">Updated details of division</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateDivisionAsync(DivisionAC updatedDivisionDetails, Guid selectedEntityId)
        {

            var dbDivisionData = await _dataRepository.Where<Division>(x => !x.IsDeleted && x.Id == updatedDivisionDetails.Id).AsNoTracking().FirstAsync();

            var isToUpdateDivision = dbDivisionData.Name.ToLower() == updatedDivisionDetails.Name.ToLower() ? true :
                !(await _dataRepository.AnyAsync<Division>(x => !x.IsDeleted && x.Name.ToLower() == updatedDivisionDetails.Name.ToLower()));

            if (isToUpdateDivision)
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        dbDivisionData = _mapper.Map<Division>(updatedDivisionDetails);
                        dbDivisionData.UpdatedDateTime = DateTime.UtcNow;
                        dbDivisionData.UpdatedBy = currentUserId;
                        dbDivisionData.AuditableEntity = null;
                        dbDivisionData.UserUpdatedBy = null;

                        _dataRepository.Update(dbDivisionData);
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
                throw new DuplicateDataException(StringConstant.divisionFieldName, updatedDivisionDetails.Name);
            }
        }

        /// <summary>
        /// Delete division from an auditable entity
        /// </summary>
        /// <param name="divisionId">Id of the division that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteDivisionAsync(Guid divisionId, Guid selectedEntityId)
        {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        var dbDivisionData = await _dataRepository.FirstAsync<Division>(x => x.EntityId == selectedEntityId && x.Id == divisionId);

                        dbDivisionData.IsDeleted = true;
                        dbDivisionData.UpdatedDateTime = DateTime.UtcNow;
                        dbDivisionData.UpdatedBy = currentUserId;
                        dbDivisionData.UserUpdatedBy = null;
                        dbDivisionData.AuditableEntity = null;

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

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalCountOfDivisionSearchStringWiseAsync(string searchString, Guid selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<Division>(x => !x.IsDeleted &&
                                                                                x.EntityId == selectedEntityId &&
                                                                                x.Name.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<Division>(x => !x.IsDeleted && x.EntityId == selectedEntityId);
            }

            return totalRecords;
        }

        #region export to excel  auditable entity division
        /// <summary>
        /// Method for exporting  auditable entity division
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportEntityDivisionsAsync(string entityId, int timeOffset)
        {
            var divisionList = await _dataRepository.Where<Division>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                          .Include(x => x.AuditableEntity).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var divisionACList = _mapper.Map<List<DivisionAC>>(divisionList);
            if (divisionACList.Count == 0)
            {
                DivisionAC divisionAC = new DivisionAC();
                divisionACList.Add(divisionAC);
            }


            divisionACList.ForEach(x =>
            {
                x.EntityName = x.Id != null ? divisionList.FirstOrDefault(z => z.EntityId == x.EntityId)?.AuditableEntity.Name : string.Empty;
                x.Name = x.Id != null ? x.Name : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(divisionACList, StringConstant.divisionFieldName + "(" + entityName + ")");
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
