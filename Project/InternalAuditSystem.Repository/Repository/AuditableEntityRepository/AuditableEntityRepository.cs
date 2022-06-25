using AutoMapper;
using InternalAuditSystem.DomailModel.DatabaseContext;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.EntityDocumentRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.EntityRelationMappingRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.PrimaryGeographicalAreaRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.RiskAssessmentRepository;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Repository.Repository.EntityCategoryRepository;
using InternalAuditSystem.Repository.Repository.EntityTypeReporsitory;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Repository.Repository.UserRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditableEntityRepository
{
    public class AuditableEntityRepository : IAuditableEntityRepository
    {
        #region Private variable(s)
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IGlobalRepository _globalRepository;
        public IEntityCategoryRepository _entityCategoryRepository;
        public IEntityTypeRepository _entityTypeRepository;
        public IPrimaryGeographicalAreaRepository _primaryGeographicalAreaRepository;
        public IRiskAssessmentRepository _riskAssessmentRepository;
        public IEntityRelationMappingRepository _entityRelationMappingRepository;
        public IEntityDocumentRepository _entityDocumentRepository;
        public IUserRepository _userRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        public IExportToExcelRepository _exportToExcelRepository;
        #endregion

        #region Public method(s)
        public AuditableEntityRepository(
            IDataRepository dataRepository,
            IMapper mapper, IGlobalRepository globalRepository,
            IEntityCategoryRepository entityCategoryRepository,
            IEntityTypeRepository entityTypeRepository,
            IPrimaryGeographicalAreaRepository primaryGeographicalAreaRepository,
            IRiskAssessmentRepository riskAssessmentRepository,
            IEntityRelationMappingRepository entityRelationMappingRepository,
            IEntityDocumentRepository entityDocumentRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
         IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _globalRepository = globalRepository;
            _entityCategoryRepository = entityCategoryRepository;
            _entityTypeRepository = entityTypeRepository;
            _primaryGeographicalAreaRepository = primaryGeographicalAreaRepository;
            _riskAssessmentRepository = riskAssessmentRepository;
            _entityRelationMappingRepository = entityRelationMappingRepository;
            _entityDocumentRepository = entityDocumentRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _exportToExcelRepository = exportToExcelRepository;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
        }


        /// <summary>
        /// Get Auditable Entity List
        /// </summary>
        /// <param name="pagination">Pagination object</param>
        /// <returns>Pagination object</returns>
        public async Task<Pagination<AuditableEntityAC>> GetAuditableEntityListAsync(Pagination<AuditableEntityAC> pagination)
        {
            // Apply pagination
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);

            // get all the entity id where current user is added as a member or created by own
            var userEntityIdList = await _dataRepository.Where<EntityUserMapping>(x => x.UserId == currentUserId && !x.IsDeleted).AsNoTracking().Select(x => x.EntityId).ToListAsync();

            IQueryable<AuditableEntity> auditableEntityList = _dataRepository.Where<AuditableEntity>(x => x.IsStrategyAnalysisDone && (x.CreatedBy == currentUserId || userEntityIdList.Contains(x.Id))
                                                                 && (!String.IsNullOrEmpty(pagination.searchText) ? x.Name.ToLower().Contains(pagination.searchText.ToLower()) : true)
                                                                 && !x.IsDeleted).Include(x => x.EntityType);

            //Get total count
            pagination.TotalRecords = auditableEntityList.Count();

            if (pagination.PageSize == 0)//Get all records
            {
                List<AuditableEntity> auditableEntityFinalList = await auditableEntityList.OrderByDescending(x => x.CreatedDateTime).Skip(0).Take(pagination.TotalRecords).ToListAsync();
                pagination.Items = _mapper.Map<List<AuditableEntityAC>>(auditableEntityFinalList);
            }
            else
            {
                List<AuditableEntity> auditableEntityFinalList = await auditableEntityList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToListAsync();
                pagination.Items = _mapper.Map<List<AuditableEntityAC>>(auditableEntityFinalList);

            }
            // engagement partner and manager bind 
            // Note TODO :Change designation to user role 
            List<Guid> entityIdList = pagination.Items.Select(x => (Guid)x.Id).ToList();
            List<EntityUserMapping> entityUserList = await _dataRepository.Where<EntityUserMapping>(x => entityIdList.Contains(x.EntityId)
                                          && (x.User.Designation.ToLower() == StringConstant.AsstMangagerDesignation.ToLower()
                                         || x.User.Designation.ToLower() == StringConstant.AsscDirectorDesignation.ToLower()
                                         || x.User.Designation.ToLower() == StringConstant.ManagerDesignation.ToLower())
                                         && !x.IsDeleted && !x.User.IsDeleted).Include(x => x.User)
                                        .ToListAsync();

            for (var i = 0; i < pagination.Items.Count(); i++)
            {
                pagination.Items[i].EngagementManagerStringList = string.Empty;
                entityUserList.Where(x => x.EntityId == pagination.Items[i].Id).ToList().ForEach(a =>
                {
                    pagination.Items[i].EngagementManagerStringList += a.User.Name + StringConstant.AddComma;
                });
                if (!string.IsNullOrEmpty(pagination.Items[i].EngagementManagerStringList))
                {
                    pagination.Items[i].EngagementManagerStringList = pagination.Items[i].EngagementManagerStringList.Remove(pagination.Items[i].EngagementManagerStringList.Length - 2);
                }
            }

            pagination.CurrentUserDetails = await GetPermittedEntitiesOfLoggedInUserAsync(currentUserId, userEntityIdList);


            return pagination;
        }

        /// <summary>
        /// Get auditable entity details
        /// </summary>
        /// <param name="id">Auditable entity id</param>
        /// <param name="stepNo">Step number</param>
        /// <returns>AuditableEntityAC object</returns>
        public async Task<AuditableEntityAC> GetAuditableEntityDetailsAsync(Guid id, int stepNo)
        {
            AuditableEntity auditableEntity = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => x.Id == id && !x.IsDeleted);
            AuditableEntityAC auditableEntityAC = _mapper.Map<AuditableEntityAC>(auditableEntity);
            if (stepNo == 1)
            {
                AuditableEntity childAuditableEntity = _dataRepository.Where<AuditableEntity>(x => !x.IsDeleted &&
                                                      auditableEntity.ParentEntityId != null ? x.ParentEntityId == auditableEntity.ParentEntityId :
                                                      x.ParentEntityId == id).OrderByDescending(x => x.Version).FirstOrDefault();

                if (childAuditableEntity != null)
                {
                    auditableEntityAC.NewVersion = childAuditableEntity.Version + 1;
                }
                else
                {
                    auditableEntityAC.NewVersion = auditableEntity.Version + 1;
                }

            }
            else if (stepNo == 2)
            {
                auditableEntityAC.EntityCategoryACList = await _entityCategoryRepository.GetAllEntityCategoryByEntityIdAsync(id);
                auditableEntityAC.EntityTypeACList = await _entityTypeRepository.GetAllEntityTypeByEntityIdAsync(id);
            }
            return auditableEntityAC;
        }


        /// <summary>
        /// Update Auditable Entity
        /// </summary>
        /// <param name="auditableEntityAC">Auditable entity AC object</param>
        public async Task UpdateAuditableEntityAsync(AuditableEntityAC auditableEntityAC)
        {
            AuditableEntity auditableEntity = new AuditableEntity();
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    if (!auditableEntityAC.IsNewVersion)
                    {
                        if (CheckIfAuditableEntityNameExistAsync(auditableEntityAC.Name, auditableEntityAC.Id, auditableEntityAC.ParentEntityId))
                        {
                            throw new DuplicateDataException(StringConstant.AuditableEntityModuleName, auditableEntityAC.Name);
                        }
                        auditableEntity = _mapper.Map<AuditableEntity>(auditableEntityAC);
                    }
                    else
                    {
                        auditableEntity = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => x.Id == auditableEntityAC.Id && !x.IsDeleted);
                        auditableEntity.SelectedCategoryId = auditableEntityAC.SelectedCategoryId;
                        auditableEntity.SelectedTypeId = auditableEntityAC.SelectedTypeId;
                    }

                    auditableEntity.UpdatedDateTime = DateTime.UtcNow;
                    auditableEntity.UpdatedBy = currentUserId;

                    _dataRepository.Update(auditableEntity);
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
        /// Add Auditable Entity to database
        /// </summary>
        /// <param name="auditableEntityAC">Auditable Entity model to be added</param>
        /// <returns>Added Auditable Entity</returns>
        public async Task<AuditableEntityAC> AddAuditableEntityAsync(AuditableEntityAC auditableEntityAC, bool isFromStrategicAnalysis = false)
        {
            AuditableEntity auditableEntity = new AuditableEntity();

            if(!isFromStrategicAnalysis)
            {
                //Check if AuditableEntity exists
                if (CheckIfAuditableEntityNameExistAsync(auditableEntityAC.Name, auditableEntityAC.Id) && !auditableEntityAC.IsNewVersion)
                {
                    throw new DuplicateDataException(StringConstant.AuditableEntityModuleName, auditableEntityAC.Name);
                }
            }
            

            if (auditableEntityAC.Id != null)
            {
                if (auditableEntityAC.ParentEntityId == null)
                {
                    auditableEntityAC.ParentEntityId = auditableEntityAC.Id;
                }
                auditableEntityAC.Version = auditableEntityAC.NewVersion;
                auditableEntityAC.SelectedCategoryId = null;
                auditableEntityAC.SelectedTypeId = null;
            }
            auditableEntityAC.Id = new Guid();
            auditableEntity = _mapper.Map<AuditableEntity>(auditableEntityAC);

            auditableEntity.CreatedDateTime = DateTime.UtcNow;
            auditableEntity.CreatedBy = currentUserId;

            await _dataRepository.AddAsync(auditableEntity);
            await _dataRepository.SaveChangesAsync();
            return _mapper.Map<AuditableEntityAC>(auditableEntity);



        }
        /// <summary>
        /// Delete AuditableEntity & its all linked data (masters)
        /// </summary>
        /// <param name="entityId">AuditableEntity id</param>
        /// <returns>Void</returns>
        public async Task DeleteAuditableEntityAync(Guid entityId)
        {
            
            //Check whether it has Audit plan reference
            if (await _dataRepository.AnyAsync<AuditPlan>(x => x.EntityId == entityId && !x.IsDeleted))
            {
                throw new DeleteValidationException(StringConstant.AuditableEntityModuleName, StringConstant.AuditPlanModuleName);
            }
            //Check whether it has Report reference
            if (await _dataRepository.AnyAsync<Report>(x => x.EntityId == entityId && !x.IsDeleted))
            {
                throw new DeleteValidationException(StringConstant.AuditableEntityModuleName, StringConstant.ReportModule);
            }
            //Check whether it has ACM reference
            if (await _dataRepository.AnyAsync<ACMPresentation>(x => x.EntityId == entityId && !x.IsDeleted))
            {
                throw new DeleteValidationException(StringConstant.AuditableEntityModuleName, StringConstant.ACMPresentationString);
            }
            //Check if it has any strategic analysis linked to it
            if (await _dataRepository.AnyAsync<StrategicAnalysis>(x => x.AuditableEntityId == entityId && !x.IsDeleted))
            {
                throw new DeleteLinkedDataException(StringConstant.DeleteAllLinkedStartegicAnalyses, StringConstant.StrategicAnalysisString);
            }
            //Check if it has any rcm linked to it
            if (await _dataRepository.AnyAsync<RiskControlMatrix>(x => x.EntityId == entityId && !x.IsDeleted))
            {
                throw new DeleteLinkedDataException(StringConstant.DeleteAllLinkedStartegicAnalyses, StringConstant.RcmModuleName);
            }
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    AuditableEntity auditableEntity = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => x.Id == entityId && !x.IsDeleted);
                    auditableEntity.IsDeleted = true;
                    auditableEntity.UpdatedBy = currentUserId;
                    auditableEntity.UpdatedDateTime = DateTime.UtcNow;
                    auditableEntity.EntityType = null;
                    auditableEntity.EntityCategory = null;
                    _dataRepository.Update<AuditableEntity>(auditableEntity);
                    await _dataRepository.SaveChangesAsync();

                    
                    #region Risk Assessment data
                    var riskAssessmentData = await _dataRepository.Where<RiskAssessment>(x => x.EntityId == entityId && !x.IsDeleted).AsNoTracking().ToListAsync();
                    if (riskAssessmentData.Count > 0)
                    {
                        for (int i = 0; i < riskAssessmentData.Count; i++)
                        {
                            riskAssessmentData[i].IsDeleted = true;
                            riskAssessmentData[i].UpdatedBy = currentUserId;
                            riskAssessmentData[i].UpdatedDateTime = DateTime.UtcNow;
                            riskAssessmentData[i].AuditableEntity = null;
                        }
                        _dataRepository.UpdateRange(riskAssessmentData);
                    }
                    #endregion

                    #region Primary Geographical Area
                    var geographcialAreaData = await _dataRepository.Where<PrimaryGeographicalArea>(x => x.EntityId == entityId && !x.IsDeleted).AsNoTracking().ToListAsync();
                    if (geographcialAreaData.Count > 0)
                    {
                        for (int i = 0; i < geographcialAreaData.Count; i++)
                        {
                            geographcialAreaData[i].IsDeleted = true;
                            geographcialAreaData[i].UpdatedBy = currentUserId;
                            geographcialAreaData[i].UpdatedDateTime = DateTime.UtcNow;
                            geographcialAreaData[i].AuditableEntity = null;
                            geographcialAreaData[i].Region = null;
                            geographcialAreaData[i].EntityCountry = null;
                            geographcialAreaData[i].EntityState = null;
                            geographcialAreaData[i].Location = null;
                        }
                        _dataRepository.UpdateRange(riskAssessmentData);
                    }
                    #endregion

                    #region Relation
                    var relationData = await _dataRepository.Where<EntityRelationMapping>(x => x.EntityId == entityId && !x.IsDeleted).AsNoTracking().ToListAsync();
                    if (relationData.Count > 0)
                    {
                        for (int i = 0; i < relationData.Count; i++)
                        {
                            relationData[i].IsDeleted = true;
                            relationData[i].UpdatedBy = currentUserId;
                            relationData[i].UpdatedDateTime = DateTime.UtcNow;
                            relationData[i].AuditableEntity = null;
                            relationData[i].RelationshipType = null;
                        }
                        _dataRepository.UpdateRange(riskAssessmentData);
                    }
                    #endregion

                    //soft delete all masters(entity, plan, observation, rating ) data for that auditable entity
                    await DeleteMastersData(entityId);

                    #region remove current selected entity of any user
                    // delete all users current selected entity
                    var userDetails = await _dataRepository.Where<User>(x => x.CurrentSelectedEntityId == entityId).AsNoTracking().ToListAsync();
                    for (int i = 0; i < userDetails.Count; i++)
                    {
                        userDetails[i].CurrentSelectedEntityId = null;
                    }
                    _dataRepository.UpdateRange(userDetails);
                    #endregion

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
        /// Create new version from auditable entity
        /// </summary>
        /// <param name="auditableEntityId">Parent auditable entity</param>
        /// <returns></returns>
        public async Task CreateNewVersionAsync(Guid auditableEntityId)
        {
            //Copy auditable entity data and create new version
            AuditableEntityAC auditableEntityAC = new AuditableEntityAC();
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    auditableEntityAC = await GetAuditableEntityDetailsAsync(auditableEntityId, 1);
                    auditableEntityAC.IsNewVersion = true;
                    auditableEntityAC = await AddAuditableEntityAsync(auditableEntityAC);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            if (auditableEntityAC != null)
            {
                //Copy risk assessment data and create new version
                Pagination<RiskAssessmentAC> paginationRiskAssessment = new Pagination<RiskAssessmentAC>()
                {
                    EntityId = auditableEntityId,
                    PageSize = 0
                };
                Pagination<RiskAssessmentAC> riskAssessmentAC = await _riskAssessmentRepository.GetRiskAssessmentListAsync(paginationRiskAssessment);
                await _riskAssessmentRepository.AddRiskAssessmentInNewVersionAsync(riskAssessmentAC.Items, auditableEntityAC.Id ?? new Guid());

                //Copy entity documents
                Pagination<EntityDocumentAC> paginationEntityDocument = new Pagination<EntityDocumentAC>()
                {
                    EntityId = auditableEntityId,
                    PageSize = 0
                };
                Pagination<EntityDocumentAC> entityDocumentAC = await _entityDocumentRepository.GetEntityDocumentListAsync(paginationEntityDocument);
                for (var i = 0; i < entityDocumentAC.Items.Count(); i++)
                {
                    entityDocumentAC.Items[i].Path = entityDocumentAC.Items[i].FileName;
                }
                await _entityDocumentRepository.AddEntityDocumentInNewVersionAsync(entityDocumentAC.Items, auditableEntityAC.Id ?? new Guid());

            }
        }

        /// <summary>
        /// Export auditable entity to Excel
        /// </summary>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> ExportToExcelAsync(int timeOffset)
        {
            List<AuditableEntityAC> auditableEntityACList = await _dataRepository.Where<AuditableEntity>(x => !x.IsDeleted && x.IsStrategyAnalysisDone)
                                                    .Include(x => x.EntityType)
                                                    .Include(x => x.EntityCategory)
                                                    .Select(x =>
                                                    new AuditableEntityAC
                                                    {
                                                        Id = x.Id,
                                                        Name = x.Name,
                                                        Version = x.Version,
                                                        StatusString = x.Status.ToString(),
                                                        Description = x.Description,
                                                        SelectedTypeString = x.EntityType.TypeName,
                                                        SelectedCategoryString = x.EntityCategory.CategoryName,
                                                        CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                                        UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                                        CreatedDateTime = x.CreatedDateTime,
                                                    }).OrderByDescending(x => x.CreatedDateTime).ToListAsync();


            List<Guid> auditableEntityIdList = auditableEntityACList.Select(x => x.Id ?? new Guid()).ToList();
            List<EntityUserMapping> entityUserList = await _dataRepository.Where<EntityUserMapping>(x => auditableEntityIdList.Contains(x.EntityId)
                                         && (x.User.Designation.ToLower() == StringConstant.AsstMangagerDesignation.ToLower()
                                         || x.User.Designation.ToLower() == StringConstant.AsscDirectorDesignation.ToLower()
                                         || x.User.Designation.ToLower() == StringConstant.ManagerDesignation.ToLower())
                                         && !x.IsDeleted && !x.User.IsDeleted).Include(x => x.User)
                                        .ToListAsync();

            for (var i = 0; i < auditableEntityACList.Count(); i++)
            {
                auditableEntityACList[i].EngagementManagerStringList = string.Empty;
                entityUserList.Where(x => x.EntityId == auditableEntityACList[i].Id).ToList().ForEach(a =>
                {
                    auditableEntityACList[i].EngagementManagerStringList += a.User.Name + StringConstant.AddComma;
                });
                if (!string.IsNullOrEmpty(auditableEntityACList[i].EngagementManagerStringList))
                {
                    auditableEntityACList[i].EngagementManagerStringList = auditableEntityACList[i].EngagementManagerStringList.Remove(auditableEntityACList[i].EngagementManagerStringList.Length - 2);
                }
            }

            //Get PrimaryGeographicalAreaAC list
            List<PrimaryGeographicalAreaAC> primaryGeographicalAreaACList = await _dataRepository.Where<PrimaryGeographicalArea>(x =>
                                          auditableEntityIdList.Contains(x.EntityId) && !x.IsDeleted && !x.AuditableEntity.IsDeleted)
                                          .Include(x => x.AuditableEntity)
                                          .Include(x => x.Region)
                                          .Include(x => x.EntityCountry)
                                          .Include(x => x.EntityCountry.Country)
                                          .Include(x => x.EntityState)
                                          .Include(x => x.EntityState.ProvinceState)
                                          .Include(x => x.Location)
                                          .Select(x => new PrimaryGeographicalAreaAC
                                          {
                                              Id = x.Id,
                                              AuditableEntity = x.AuditableEntity.Name,
                                              RegionString = x.Region.Name,
                                              CountryString = x.EntityCountry.Country.Name,
                                              StateString = x.EntityState.ProvinceState.Name,
                                              LocationString = x.Location.Name,
                                              CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                              UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                              CreatedDateTime = x.CreatedDateTime
                                          }).OrderByDescending(x => x.CreatedDateTime).ToListAsync();


            //Get Risk Assessment list
            List<RiskAssessmentAC> riskAssessmentACList = await _dataRepository.Where<RiskAssessment>(x =>
                                          auditableEntityIdList.Contains(x.EntityId) && !x.IsDeleted)
                                          .Include(x => x.AuditableEntity)
                                          .Select(x => new RiskAssessmentAC
                                          {
                                              Id = x.Id,
                                              AuditableEntity = x.AuditableEntity.Name,
                                              Name = x.Name,
                                              StatusString = x.Status.ToString(),
                                              Year = x.Year,
                                              Summary = x.Summary,
                                              CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                              UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                              CreatedDateTime = x.CreatedDateTime
                                          }).OrderByDescending(x => x.CreatedDateTime).ToListAsync();

            //Get Relationship list
            List<EntityRelationMappingAC> entityRelationMappingACList = await _dataRepository.Where<EntityRelationMapping>(x =>
                                          auditableEntityIdList.Contains(x.EntityId) && !x.IsDeleted)
                                          .Include(x => x.AuditableEntity)
                                          .Include(x => x.RelatedAuditableEntity)
                                          .Include(x => x.RelationshipType)
                                          .Select(x => new EntityRelationMappingAC
                                          {
                                              Id = x.Id,
                                              AuditableEntity = x.AuditableEntity.Name,
                                              RelatedEntityName = x.RelatedAuditableEntity.Name,
                                              RelationName = x.RelationshipType.Name,
                                              CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                              UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                              CreatedDateTime = x.CreatedDateTime
                                          }).OrderByDescending(x => x.CreatedDateTime).ToListAsync();

            if (primaryGeographicalAreaACList == null || primaryGeographicalAreaACList.Count == 0)
            {
                PrimaryGeographicalAreaAC primaryGeographicalAreaAC = new PrimaryGeographicalAreaAC();
                primaryGeographicalAreaACList.Add(primaryGeographicalAreaAC);
            }
            if (riskAssessmentACList == null || riskAssessmentACList.Count == 0)
            {
                RiskAssessmentAC riskAssessmentAC = new RiskAssessmentAC();
                riskAssessmentACList.Add(riskAssessmentAC);
            }
            if (entityRelationMappingACList == null || entityRelationMappingACList.Count == 0)
            {
                EntityRelationMappingAC entityRelationMappingAC = new EntityRelationMappingAC();
                entityRelationMappingACList.Add(entityRelationMappingAC);
            }

            //create dynamic directory
            dynamic dynamicDictionary = new DynamicDictionary<string, dynamic>();
            dynamicDictionary.Add(StringConstant.AuditableEntityModuleName, auditableEntityACList);
            dynamicDictionary.Add(StringConstant.PrimaryGeopgraphicalAreaModuleName, primaryGeographicalAreaACList);
            dynamicDictionary.Add(StringConstant.RiskAssessmentString, riskAssessmentACList);
            dynamicDictionary.Add(StringConstant.EntityRelationModuleName, entityRelationMappingACList);

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFileWithMultipleTable(dynamicDictionary, StringConstant.AuditableEntityModuleName);
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Common Method
        /// <summary>
        /// Get all the auditable entity whose strategy analysis is done and user has access along with user details
        /// </summary>
        /// <param name="currentLoggedInUserId">Logged User Id</param>
        /// <param name="permittedEntityIdList">Permitted entity id list of a current user</param>
        /// <returns>Logged in user details</returns>
        public async Task<LoggedInUserDetails> GetPermittedEntitiesOfLoggedInUserAsync(Guid currentLoggedInUserId,List<Guid> permittedEntityIdList = null)
        {
            // 
            if(permittedEntityIdList == null)
            {
                permittedEntityIdList = await _dataRepository.Where<EntityUserMapping>(x => x.UserId == currentLoggedInUserId).AsNoTracking().Select(x => x.EntityId).ToListAsync();
            }
             
            // fetch data of entityes where user is added as a member or user create
            var permittedEntityList = await _dataRepository.Where<AuditableEntity>(x => !x.IsDeleted && x.IsStrategyAnalysisDone && (x.CreatedBy == currentLoggedInUserId || permittedEntityIdList.Contains(x.Id)))
                                                  .AsNoTracking()
                                                  .Select(entity => new AuditableEntityAC { Id = entity.Id, Name = entity.Name, Version = entity.Version })
                                                  .OrderBy(x => x.Name).ToListAsync();

            // basic details of current user 
            var userDetails = await _userRepository.GetCurrentLoggedInUserDetailsById(currentLoggedInUserId);


            LoggedInUserDetails loggedInUserDetails = new LoggedInUserDetails()
            {
                PermittedEntityList = permittedEntityList,
                UserDetails = userDetails
            };

            return loggedInUserDetails;

        }

        /// <summary>
        /// Get name of selected entity
        /// </summary>
        /// <param name="entityId">Selected entity id<param>
        /// <returns>Name of auditable Entity</returns>
        public async Task<string> GetEntityNameById(string entityId)
        {
            var entity = await _dataRepository.FirstAsync<AuditableEntity>(x => !x.IsDeleted && x.Id.ToString() == entityId);
            return entity.Name;
        }
        #endregion

        #endregion

        #region Private method(s)
        /// <summary>
        /// Check if work program name exist
        /// </summary>
        /// <param name="auditableEntityName">Name of workprogram entered by user</param>
        /// <param name="auditableEntityId">If it is in edit page then this id of workprogram will be there else in add form it will be null</param>
        /// <returns>Returns if name is duplicate or not</returns>
        private bool CheckIfAuditableEntityNameExistAsync(string auditableEntityName, Guid? auditableEntityId = null, Guid? parentEntityId = null)
        {
            bool isNameExists;
            if (auditableEntityId != null)
            {
                if (parentEntityId != null)
                {
                    isNameExists = _dataRepository.Any<AuditableEntity>(x => x.Id != auditableEntityId && x.ParentEntityId != parentEntityId && x.Id != parentEntityId && !x.IsDeleted && x.Name.ToLower() == auditableEntityName.ToLower());
                }
                else
                {
                    isNameExists = _dataRepository.Any<AuditableEntity>(x => x.Id != auditableEntityId && x.ParentEntityId != auditableEntityId && !x.IsDeleted && x.Name.ToLower() == auditableEntityName.ToLower());

                }
            }
            else
            {
                isNameExists = _dataRepository.Any<AuditableEntity>(x => x.Name.ToLower() == auditableEntityName.ToLower() && !x.IsDeleted);
            }
            return isNameExists;
        }

        /// <summary>
        /// Delete all masters data added for the entity
        /// </summary>
        /// <param name="id">Id of the enitty</param>
        /// <returns>Task</returns>
        private async Task DeleteMastersData(Guid id)
        {
            #region Entity type soft delete
            var entityTypeMasterData = await _dataRepository.Where<EntityType>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (entityTypeMasterData.Count > 0)
            {
                for (int i = 0; i < entityTypeMasterData.Count; i++)
                {
                    entityTypeMasterData[i].IsDeleted = true;
                    entityTypeMasterData[i].UpdatedBy = currentUserId;
                    entityTypeMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    entityTypeMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(entityTypeMasterData);
            }
            #endregion

            #region Entity category soft delete
            var entityCategoryMasterData = await _dataRepository.Where<EntityCategory>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (entityCategoryMasterData.Count > 0)
            {
                for (int i = 0; i < entityCategoryMasterData.Count; i++)
                {
                    entityCategoryMasterData[i].IsDeleted = true;
                    entityCategoryMasterData[i].UpdatedBy = currentUserId;
                    entityCategoryMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    entityCategoryMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(entityCategoryMasterData);
            }
            #endregion

            #region Division soft delete
            var divisionMasterData = await _dataRepository.Where<Division>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (divisionMasterData.Count > 0)
            {
                for (int i = 0; i < divisionMasterData.Count; i++)
                {
                    divisionMasterData[i].IsDeleted = true;
                    divisionMasterData[i].UpdatedBy = currentUserId;
                    divisionMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    divisionMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(divisionMasterData);
            }
            #endregion

            #region relationship type soft delete
            var relationshipTypeMasterData = await _dataRepository.Where<RelationshipType>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (relationshipTypeMasterData.Count > 0)
            {
                for (int i = 0; i < relationshipTypeMasterData.Count; i++)
                {
                    relationshipTypeMasterData[i].IsDeleted = true;
                    relationshipTypeMasterData[i].UpdatedBy = currentUserId;
                    relationshipTypeMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    relationshipTypeMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(relationshipTypeMasterData);
            }
            #endregion

            #region region soft delete
            var regionMasterData = await _dataRepository.Where<Region>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (regionMasterData.Count > 0)
            {
                for (int i = 0; i < regionMasterData.Count; i++)
                {
                    regionMasterData[i].IsDeleted = true;
                    regionMasterData[i].UpdatedBy = currentUserId;
                    regionMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    regionMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(regionMasterData);
            }
            #endregion

            #region country soft delete
            var countryMasterData = await _dataRepository.Where<EntityCountry>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (countryMasterData.Count > 0)
            {
                for (int i = 0; i < countryMasterData.Count; i++)
                {
                    countryMasterData[i].IsDeleted = true;
                    countryMasterData[i].UpdatedBy = currentUserId;
                    countryMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    countryMasterData[i].Region = null;
                    countryMasterData[i].Country = null;
                    countryMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(countryMasterData);
            }
            #endregion

            #region state soft delete
            var stateMasterData = await _dataRepository.Where<DomailModel.Models.AuditableEntityModels.EntityState>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (stateMasterData.Count > 0)
            {
                for (int i = 0; i < stateMasterData.Count; i++)
                {
                    stateMasterData[i].IsDeleted = true;
                    stateMasterData[i].UpdatedBy = currentUserId;
                    stateMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    stateMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(stateMasterData);
            }
            #endregion

            #region audit team & clientParticipant soft delete
            var auditTeamData = await _dataRepository.Where<EntityUserMapping>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (auditTeamData.Count > 0)
            {
                for (int i = 0; i < auditTeamData.Count; i++)
                {
                    auditTeamData[i].IsDeleted = true;
                    auditTeamData[i].UpdatedBy = currentUserId;
                    auditTeamData[i].UpdatedDateTime = DateTime.UtcNow;
                    auditTeamData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(auditTeamData);
            }
            #endregion

            #region location soft delete
            var locationMasterData = await _dataRepository.Where<Location>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (locationMasterData.Count > 0)
            {
                for (int i = 0; i < locationMasterData.Count; i++)
                {
                    locationMasterData[i].IsDeleted = true;
                    locationMasterData[i].UpdatedBy = currentUserId;
                    locationMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    locationMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(locationMasterData);
            }
            #endregion

            #region Plan audit type  soft delete
            var planTypeMasterData = await _dataRepository.Where<AuditType>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (planTypeMasterData.Count > 0)
            {
                for (int i = 0; i < planTypeMasterData.Count; i++)
                {
                    planTypeMasterData[i].IsDeleted = true;
                    planTypeMasterData[i].UpdatedBy = currentUserId;
                    planTypeMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    planTypeMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(planTypeMasterData);
            }
            #endregion

            #region Plan audit category soft delete
            var planCategoryMasterData = await _dataRepository.Where<AuditCategory>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (planCategoryMasterData.Count > 0)
            {
                for (int i = 0; i < planCategoryMasterData.Count; i++)
                {
                    planCategoryMasterData[i].IsDeleted = true;
                    planCategoryMasterData[i].UpdatedBy = currentUserId;
                    planCategoryMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    planCategoryMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(planCategoryMasterData);
            }
            #endregion

            #region plan process & subprocess soft delete
            var planProcessMasterData = await _dataRepository.Where<Process>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (planCategoryMasterData.Count > 0)
            {
                for (int i = 0; i < planCategoryMasterData.Count; i++)
                {
                    planCategoryMasterData[i].IsDeleted = true;
                    planCategoryMasterData[i].UpdatedBy = currentUserId;
                    planCategoryMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    planCategoryMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(planCategoryMasterData);
            }
            #endregion

            #region rating soft delete
            var ratingMasterData = await _dataRepository.Where<Rating>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (ratingMasterData.Count > 0)
            {
                for (int i = 0; i < ratingMasterData.Count; i++)
                {
                    ratingMasterData[i].IsDeleted = true;
                    ratingMasterData[i].UpdatedBy = currentUserId;
                    ratingMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    ratingMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(regionMasterData);
            }
            #endregion

            #region observation category soft delete
            var observationCategoryMasterData = await _dataRepository.Where<ObservationCategory>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (observationCategoryMasterData.Count > 0)
            {
                for (int i = 0; i < observationCategoryMasterData.Count; i++)
                {
                    observationCategoryMasterData[i].IsDeleted = true;
                    observationCategoryMasterData[i].UpdatedBy = currentUserId;
                    observationCategoryMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    observationCategoryMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(observationCategoryMasterData);
            }
            #endregion

            #region distribution list soft delete
            var distributorMasterData = await _dataRepository.Where<Distributors>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (distributorMasterData.Count > 0)
            {
                for (int i = 0; i < distributorMasterData.Count; i++)
                {
                    distributorMasterData[i].IsDeleted = true;
                    distributorMasterData[i].UpdatedBy = currentUserId;
                    distributorMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    distributorMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(distributorMasterData);
            }
            #endregion

            #region rcm process list soft delete
            var rcmProcessMasterData = await _dataRepository.Where<RiskControlMatrixProcess>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (rcmProcessMasterData.Count > 0)
            {
                for (int i = 0; i < rcmProcessMasterData.Count; i++)
                {
                    rcmProcessMasterData[i].IsDeleted = true;
                    rcmProcessMasterData[i].UpdatedBy = currentUserId;
                    rcmProcessMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    rcmProcessMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(distributorMasterData);
            }
            #endregion

            #region rcm subprocess list soft delete
            var rcmSubprocessMaster = await _dataRepository.Where<RiskControlMatrixSubProcess>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (rcmSubprocessMaster.Count > 0)
            {
                for (int i = 0; i < rcmSubprocessMaster.Count; i++)
                {
                    rcmSubprocessMaster[i].IsDeleted = true;
                    rcmSubprocessMaster[i].UpdatedBy = currentUserId;
                    rcmSubprocessMaster[i].UpdatedDateTime = DateTime.UtcNow;
                    rcmSubprocessMaster[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(rcmSubprocessMaster);
            }
            #endregion

            #region rcm sector list soft delete
            var rcmSectorMasterData = await _dataRepository.Where<RiskControlMatrixSector>(x => x.EntityId == id && !x.IsDeleted).AsNoTracking().ToListAsync();
            if (rcmSectorMasterData.Count > 0)
            {
                for (int i = 0; i < rcmSectorMasterData.Count; i++)
                {
                    rcmSectorMasterData[i].IsDeleted = true;
                    rcmSectorMasterData[i].UpdatedBy = currentUserId;
                    rcmSectorMasterData[i].UpdatedDateTime = DateTime.UtcNow;
                    rcmSectorMasterData[i].AuditableEntity = null;
                }
                _dataRepository.UpdateRange(rcmSectorMasterData);
            }
            #endregion
        }
        #endregion
    }

}
