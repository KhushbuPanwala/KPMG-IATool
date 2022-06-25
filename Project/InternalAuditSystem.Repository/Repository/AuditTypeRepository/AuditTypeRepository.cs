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

namespace InternalAuditSystem.Repository.Repository.AuditTypeRepository
{
    public class AuditTypeRepository : IAuditTypeRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        private readonly IAuditableEntityRepository _auditableEntityRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        public AuditTypeRepository(IDataRepository dataRepository, IMapper mapper, 
            IExportToExcelRepository exportToExcelRepository,
             IHttpContextAccessor httpContextAccessor,
            IAuditableEntityRepository auditableEntityRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _auditableEntityRepository = auditableEntityRepository;
            _exportToExcelRepository = exportToExcelRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
        }

        /// <summary>
        /// Get all the audit types under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit types under an auditable entity</returns>
        public async Task<List<AuditTypeAC>> GetAllAuditTypesPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, string selectedEntityId)
        {
            List<AuditType> auditTypeList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                auditTypeList = await _dataRepository.Where<AuditType>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId
                                                               && x.Name.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                auditTypeList.OrderBy(a => a.Name);
            }
            else
            {
                //when only to get audit types data without searchstring - to show initial data on list page 

                auditTypeList = await _dataRepository.Where<AuditType>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId)
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();
            }

            return _mapper.Map<List<AuditTypeAC>>(auditTypeList);

        }


        /// <summary>
        /// Get alll the audit types under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all audit types </returns>
        public async Task<List<AuditTypeAC>> GetAllAuditTypeByEntityIdAsync(Guid selectedEntityId)
        {
            var auditTypeList = await _dataRepository.Where<AuditType>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                                          .Select(x => new AuditTypeAC { Id = x.Id, Name = x.Name })
                                                          .AsNoTracking().ToListAsync();
            return _mapper.Map<List<AuditTypeAC>>(auditTypeList);
        }


        /// <summary>
        /// Get data of a particular audit type under an auditable entity
        /// </summary>
        /// <param name="auditTypeId">Id of the audit type</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular aduit type data</returns>
        public async Task<AuditTypeAC> GetAuditTypeByIdAsync(string auditTypeId, string selectedEntityId)
        {
            var result = await _dataRepository.FirstAsync<AuditType>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == auditTypeId);

            return _mapper.Map<AuditTypeAC>(result);
        }

        /// <summary>
        /// Add new audit type under an auditable entity
        /// </summary>
        /// <param name="auditTypeDetails">Details of audit type</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit type details</returns>
        public async Task<AuditTypeAC> AddAuditTypeAsync(AuditTypeAC auditTypeDetails, string selectedEntityId)
        {

            //check user already exist in db 
            if (!await _dataRepository.AnyAsync<AuditType>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.Name.ToLower() == auditTypeDetails.Name.ToLower()))
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        var auditTypeData = new AuditType()
                        {
                            Name = auditTypeDetails.Name,
                            EntityId = new Guid(selectedEntityId),
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy = currentUserId,
                            AuditableEntity = null,
                            UserCreatedBy = null
                        };

                        var addedAuditType = await _dataRepository.AddAsync<AuditType>(auditTypeData);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();

                        return _mapper.Map<AuditTypeAC>(addedAuditType);
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
                throw new DuplicateDataException(StringConstant.AuditTypeFieldName, auditTypeDetails.Name);
            }

        }

        /// <summary>
        /// Update audit type under an auditable entity
        /// </summary>
        /// <param name="updatedAuditTypeDetails">Updated details of audit type</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateAuditTypeAsync(AuditTypeAC updatedAuditTypeDetails, string selectedEntityId)
        {

            var dbAuditType = await _dataRepository.Where<AuditType>(x => !x.IsDeleted && x.Id == updatedAuditTypeDetails.Id).AsNoTracking().FirstAsync();

            var isToUpdateAuditType = dbAuditType.Name.ToLower() == updatedAuditTypeDetails.Name.ToLower() ? true :
                !(await _dataRepository.AnyAsync<AuditType>(x => !x.IsDeleted && x.Name.ToLower() == updatedAuditTypeDetails.Name.ToLower()));

            if (isToUpdateAuditType)
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        dbAuditType = _mapper.Map<AuditType>(updatedAuditTypeDetails);
                        dbAuditType.UpdatedDateTime = DateTime.UtcNow;
                        dbAuditType.UpdatedBy = currentUserId;
                        dbAuditType.AuditableEntity = null;
                        dbAuditType.UserUpdatedBy = null;

                        _dataRepository.Update(dbAuditType);
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
                throw new DuplicateDataException(StringConstant.AuditTypeFieldName, updatedAuditTypeDetails.Name);
            }
        }

        /// <summary>
        /// Delete audit type from an auditable entity
        /// </summary>
        /// <param name="auditTypeId">Id of the audit type that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteAuditTypeAsync(string auditTypeId, string selectedEntityId)
        {

            //check is this type is used in any active audit plan or not 
            var totalAuditPlansIncludeCurrentAuditType = await _dataRepository.CountAsync<AuditPlan>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.SelectedTypeId.ToString() == auditTypeId);

            // if no plan has this type as selected then do soft delete
            if (totalAuditPlansIncludeCurrentAuditType == 0)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        var dbAuditTypeData = await _dataRepository.FirstAsync<AuditType>(x => x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == auditTypeId);

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
                throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.AuditPlanModuleName);
            }


        }

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalCountOfAuditTypeSearchStringWiseAsync(string searchString, string selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<AuditType>(x => !x.IsDeleted &&
                                                                                x.EntityId.ToString() == selectedEntityId &&
                                                                                x.Name.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<AuditType>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId);
            }

            return totalRecords;
        }

        /// <summary>
        /// Method for exporting audit types
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportAuditTypesAsync(string entityId, int timeOffset)
        {
            var auditTypeList = await _dataRepository.Where<AuditType>(x => !x.IsDeleted && x.EntityId ==Guid.Parse(entityId))
                                                          .AsNoTracking().ToListAsync();
            var auditTypeACList=_mapper.Map<List<AuditTypeAC>>(auditTypeList);
            if (auditTypeACList.Count == 0)
            {
                AuditTypeAC auditTypeAC = new AuditTypeAC();
                auditTypeACList.Add(auditTypeAC);
            }
            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            auditTypeACList.ForEach(x =>
            {
                x.Name = x.Id != null ? x.Name : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            });
            
            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(auditTypeACList, StringConstant.AuditTypeFieldName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
