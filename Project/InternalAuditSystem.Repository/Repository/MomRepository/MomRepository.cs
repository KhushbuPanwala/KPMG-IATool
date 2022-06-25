using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.MomModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.MomModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.ApplicationClasses.WorkProgram;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.Repository.Repository.UserRepository;
using InternalAuditSystem.Repository.Repository.WorkProgramRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternalAuditSystem.Utility.PdfGeneration;
using InternalAuditSystem.Utility.Constants;
using System.IO;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Repository.Repository.MomRepository
{
    public class MomRepository : IMomRepository
    {
        #region Private Member
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private readonly IWorkProgramRepository _workProgramRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuditableEntityRepository _auditableEntityRepository;
        private readonly IViewRenderService _viewRenderService;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        private readonly IConfiguration _iConfig;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;

        private List<MomUserMapping> momUserMappingList = new List<MomUserMapping>();
        private List<MomUserMapping> momUserMappingPersonResposibleList = new List<MomUserMapping>();
        private List<MomUserMapping> momUserMappingPersonResposibleListToBeAddedInEdit = new List<MomUserMapping>();
        List<MainDiscussionPoint> tempMainDiscussionPointList = new List<MainDiscussionPoint>();
        List<SubPointOfDiscussion> tempSubPointOfDiscussionListToBeAdded = new List<SubPointOfDiscussion>();
        List<SubPointOfDiscussion> tempSubPointOfDiscussionListToBeUpdated = new List<SubPointOfDiscussion>();
        List<MomUserMapping> tempMomUserMappingListToBeAdded = new List<MomUserMapping>();
        #endregion

        #region Constructor
        public MomRepository(IDataRepository dataRepository, IMapper mapper, IWorkProgramRepository workProgramRepository, IUserRepository userRepository,
            IAuditableEntityRepository auditableEntityRepository, IViewRenderService viewRenderService,
            IExportToExcelRepository exportToExcelRepository, IConfiguration iConfig, IHttpContextAccessor httpContextAccessor)
        {

            _dataRepository = dataRepository;
            _mapper = mapper;
            _workProgramRepository = workProgramRepository;
            _userRepository = userRepository;
            _auditableEntityRepository = auditableEntityRepository;
            _viewRenderService = viewRenderService;
            _exportToExcelRepository = exportToExcelRepository;
            _iConfig = iConfig;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
        }
        #endregion


        #region public methods
        /// <summary>
        /// Get count of Mom
        /// </summary>
        /// <param name="searchString">Search value</param>
        /// <param name="entityId">Id of selected entity</param>
        /// <returns>count of Mom</returns>
        public async Task<int> GetMomCountAsync(string searchString, string entityId)
        {
            int totalProcessRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalProcessRecords = await _dataRepository.Where<Mom>(a => !a.IsDeleted && a.EntityId == Guid.Parse(entityId) && a.Agenda.ToLower().Contains(searchString.ToLower())).AsNoTracking().CountAsync();
            }
            else
            {
                totalProcessRecords = await _dataRepository.Where<Mom>(a => !a.IsDeleted && a.EntityId == Guid.Parse(entityId)).AsNoTracking().CountAsync();
            }
            return totalProcessRecords;

        }

        /// <summary>
        /// Get mom data
        /// </summary>
        /// <param name="pageIndex">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="entityId">Id of selected entity</param>
        /// <returns>List of mom</returns>
        public async Task<List<MomAC>> GetMomAsync(int? pageIndex, int pageSize, string searchString, string entityId)
        {
            List<Mom> momList;

            if (!string.IsNullOrEmpty(searchString))
            {
                momList = await _dataRepository.Where<Mom>(a => !a.IsDeleted && a.EntityId == Guid.Parse(entityId) && a.Agenda.ToLower().Contains(searchString.ToLower())).Skip((pageIndex - 1 ?? 0) * pageSize)
                  .Take(pageSize).OrderByDescending(o => o.CreatedDateTime).Include(z => z.WorkProgram).AsNoTracking().ToListAsync();
            }
            else
            {
                momList = await _dataRepository.Where<Mom>(a => !a.IsDeleted && a.EntityId == Guid.Parse(entityId)).OrderByDescending(o => o.CreatedDateTime).Include(z => z.WorkProgram).AsNoTracking().ToListAsync();
            }
            return _mapper.Map<List<Mom>, List<MomAC>>(momList);
        }

        /// <summary>
        /// Method  for adding mom data
        /// </summary>
        /// <param name="momAC">Application class of mom</param>
        /// <returns>Object of newly added mom</returns>
        public async Task<MomAC> AddMomAsync(MomAC momAC)
        {
            using (_dataRepository.BeginTransaction())
            {
                try
                {
                    List<MainDiscussionPoint> getMainDiscussionPointList = new List<MainDiscussionPoint>();
                    List<SubPointOfDiscussion> subPointOfDiscussionList = new List<SubPointOfDiscussion>();
                    Mom mom = _mapper.Map<MomAC, Mom>(momAC);
                    var momUserDetail = await _dataRepository.FirstAsync<User>(x => !x.IsDeleted);// ToDo:I will change this after login implementation

                    // add mom details
                    momAC.EntityId = Guid.Parse(momAC.EntityId.ToString());
                    mom.EntityId = momAC.EntityId;
                    mom.CreatedBy = momUserDetail.Id;
                    mom.CreatedDateTime = DateTime.UtcNow;
                    var addedMom = await _dataRepository.AddAsync(mom);
                    await _dataRepository.SaveChangesAsync();


                    // add main point details
                    for (int i = 0; i < momAC.MainDiscussionPointACCollection.Count; i++)
                    {
                        MainDiscussionPoint mainDiscussionPoint = new MainDiscussionPoint
                        {
                            Id = Guid.NewGuid(),
                            MomId = addedMom.Id,
                            CreatedBy = momUserDetail.Id,
                            CreatedDateTime = DateTime.UtcNow,
                            MainPoint = momAC.MainDiscussionPointACCollection[i].MainPoint
                        };
                        getMainDiscussionPointList.Add(mainDiscussionPoint);

                        // add sub point details
                        var temsubpointlist = momAC.MainDiscussionPointACCollection[i].SubPointDiscussionACCollection;
                        if (temsubpointlist != null)
                        {
                            for (int j = 0; j < temsubpointlist.Count; j++)
                            {
                                SubPointOfDiscussion subPointOfDiscussion = new SubPointOfDiscussion
                                {
                                    Id = Guid.NewGuid(),
                                    SubPoint = temsubpointlist[j].SubPoint,
                                    TargetDate = temsubpointlist[j].TargetDate,
                                    Status = temsubpointlist[j].Status,
                                    IsDeleted = false,
                                    CreatedBy = momUserDetail.Id,
                                    CreatedDateTime = DateTime.UtcNow,
                                    MainPointId = mainDiscussionPoint.Id
                                };
                                subPointOfDiscussionList.Add(subPointOfDiscussion);
                                // add person resposible of subpoint
                                if (temsubpointlist[j].PersonResponsibleACCollection != null)
                                {
                                    for (int k = 0; k < temsubpointlist[j].PersonResponsibleACCollection.Count; k++)
                                    {
                                        MomUserMapping momUserMapping = new MomUserMapping
                                        {
                                            MomId = addedMom.Id,
                                            UserId = temsubpointlist[j].PersonResponsibleACCollection[k].UserId,
                                            CreatedBy = momUserDetail.Id,
                                            CreatedDateTime = DateTime.UtcNow,
                                            SubPointOfDiscussionId = subPointOfDiscussion.Id//momAC.MainPointDiscussionCollection[i].SubPointDiscussionCollection[j].Id
                                        };
                                        momUserMappingPersonResposibleList.Add(momUserMapping);
                                    }
                                }
                            }
                        }
                    }
                    await _dataRepository.AddRangeAsync(getMainDiscussionPointList);
                    await _dataRepository.SaveChangesAsync();

                    await _dataRepository.AddRangeAsync(subPointOfDiscussionList);
                    await _dataRepository.SaveChangesAsync();

                    await _dataRepository.AddRangeAsync(momUserMappingPersonResposibleList);
                    await _dataRepository.SaveChangesAsync();
                    // add internal(team) user list
                    for (int i = 0; i < momAC.InternalUserList.Count; i++)
                    {

                        MomUserMapping momUserMapping = new MomUserMapping
                        {
                            MomId = addedMom.Id,
                            UserId = momAC.InternalUserList[i].UserId,
                            CreatedBy = momUserDetail.Id,
                            CreatedDateTime = DateTime.UtcNow

                        };
                        momUserMappingList.Add(momUserMapping);
                    }

                    // add external(client participant) user list
                    for (int j = 0; j < momAC.ExternalUserList.Count; j++)
                    {

                        MomUserMapping momUserMapping = new MomUserMapping
                        {
                            MomId = addedMom.Id,
                            UserId = momAC.ExternalUserList[j].UserId,
                            CreatedBy = momUserDetail.Id,
                            CreatedDateTime = DateTime.UtcNow
                        };
                        momUserMappingList.Add(momUserMapping);
                    }

                    await _dataRepository.AddRangeAsync(momUserMappingList);
                    await _dataRepository.SaveChangesAsync();

                    _dataRepository.CommitTransaction();

                    return _mapper.Map<MomAC>(mom);

                }
                catch (Exception)
                {
                    _dataRepository.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// Method for fetching mom detail by Id
        /// </summary>
        /// <param name="Id">Id of mom</param>
        /// <param name="entityId">Id of selected entity</param>
        /// <returns>Application class of mom</returns>
        public async Task<MomAC> GetMomDetailByIdAsync(string Id, string entityId)
        {
            Mom getMomAcById = await _dataRepository.Where<Mom>(x => x.Id == Guid.Parse(Id) && x.EntityId == Guid.Parse(entityId) && !x.IsDeleted)
                .Include(w => w.WorkProgram)
                .Include(m => m.MainDiscussionPointCollection)
                .ThenInclude(s => s.SubPointDiscussionCollection).ThenInclude(f => f.PersonResponsibleCollection)
                .Include(z => z.MomUserMappingCollection).AsNoTracking().FirstAsync();

            MomAC momAC = _mapper.Map<MomAC>(getMomAcById);
            //main point & subpoint 
            momAC.MainDiscussionPointACCollection = _mapper.Map<List<MainDiscussionPointAC>>(getMomAcById.MainDiscussionPointCollection.Where(x => !x.IsDeleted).OrderBy(y => y.CreatedDateTime));

            //all users of mom
            List<MomUserMapping> momUserList = await _dataRepository.Where<MomUserMapping>(x =>
                                          x.MomId == Guid.Parse(Id) && !x.IsDeleted).Include(x => x.User).AsNoTracking().ToListAsync();

            //per subpoint user mapping
            var subpointuserList = momUserList.Where(x => !x.IsDeleted && x.SubPointOfDiscussionId != null).ToList();

            //assign subpoint users to its subpoint and assign status of it
            for (int m = 0; m < momAC.MainDiscussionPointACCollection.Count; m++)
            {
                for (int s = 0; s < momAC.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection.Count; s++)
                {
                    momAC.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection = momAC.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection.Where(x => !x.IsDeleted).OrderBy(y => y.CreatedDateTime).ToList();
                    if (momAC.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection.Count > 0)
                    {
                        var users = subpointuserList.Where(x => x.SubPointOfDiscussionId == momAC.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection[s].Id && !x.IsDeleted).OrderBy(y => y.CreatedDateTime).ToList();
                        momAC.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection[s].PersonResponsibleACCollection = _mapper.Map<List<MomUserMappingAC>>(users);
                        if (momAC.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection[s].Status == SubPointStatus.Completed)
                        {
                            momAC.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection[s].StatusString = SubPointStatus.Completed.ToString();
                        }
                        else if (momAC.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection[s].Status == SubPointStatus.InProgress)
                        {
                            momAC.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection[s].StatusString = SubPointStatus.InProgress.ToString();
                        }
                        else
                        {
                            momAC.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection[s].StatusString = SubPointStatus.Pending.ToString();
                        }

                    }
                }
            }
            // method for showing predefined data of work program and users in edit mode
            MomAC getMomDataAC = await GetPredefinedDataForMomAsync(getMomAcById.EntityId);
            momAC.InternalUserList = _mapper.Map<List<MomUserMappingAC>>(momUserList.Where(x => x.SubPointOfDiscussionId == null && x.User.UserType == DomailModel.Enums.UserType.Internal).ToList());
            momAC.ExternalUserList = _mapper.Map<List<MomUserMappingAC>>(momUserList.Where(x => x.SubPointOfDiscussionId == null && x.User.UserType == DomailModel.Enums.UserType.External).ToList());
            momAC.WorkProgramCollection = getMomDataAC.WorkProgramCollection;
            momAC.TeamCollection = getMomDataAC.TeamCollection;
            momAC.ClientParticipantCollection = getMomDataAC.ClientParticipantCollection;
            momAC.AllPersonResposibleACDataCollection = getMomDataAC.AllPersonResposibleACDataCollection;

            return momAC;

        }

        /// <summary>
        /// Method for update Mom
        /// </summary>
        /// <param name="updatedMomDetails">Application class of mom</param>
        /// <returns>Updated momAc object</returns>
        public async Task<MomAC> UpdateMomDetailAsync(MomAC updatedMomDetails)
        {
            using (_dataRepository.BeginTransaction())
            {
                try
                {
                    Mom updateMom = new Mom();
                    updateMom = _mapper.Map<MomAC, Mom>(updatedMomDetails);
                    updateMom.EntityId = updatedMomDetails.EntityId;

                    // TOdo: Change it after login implementaion
                    var momUserDetail = await _dataRepository.FirstAsync<User>(x => !x.IsDeleted);
                    updateMom.UpdatedBy = momUserDetail.Id;
                    updateMom.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update(updateMom);
                    await _dataRepository.SaveChangesAsync();

                    //get all users from db for mom
                    var allMomUserList = _dataRepository.Where<MomUserMapping>(x => !x.IsDeleted && x.MomId == updateMom.Id).AsNoTracking().ToList();

                    #region TeamData
                    //db team users 
                    var teamUserList = allMomUserList.Where(x => x.SubPointOfDiscussionId == null).ToList();
                    // update team data
                    await UpdateMomTeamData(updatedMomDetails, teamUserList);
                    #endregion


                    await RemoveNonExistingMainPointAndItsRelatedData(updatedMomDetails);

                    #region MainPoint 

                    var allMainPointList = _dataRepository.Where<MainDiscussionPoint>(x => x.MomId == updateMom.Id).Include(y => y.SubPointDiscussionCollection).ThenInclude(z => z.PersonResponsibleCollection).AsNoTracking().ToList();
                    var existingMainPointList = updatedMomDetails.MainDiscussionPointACCollection.Where(x => x.Id != null).ToList();
                    var newMainPointList = updatedMomDetails.MainDiscussionPointACCollection.Where(x => x.Id == null).ToList();

                    // update data of main point ,sub point , person resposible
                    await UpdateDataForMainPointAndItsRelatedData((Guid)updatedMomDetails.Id, existingMainPointList);

                    // add new data of main point ,sub point , person resposible
                    await AddNewDataForMainPointAndItsRelatedData((Guid)updatedMomDetails.Id, newMainPointList);
                    #endregion
                    _dataRepository.CommitTransaction();
                    return _mapper.Map<Mom, MomAC>(updateMom);
                }
                catch (Exception)
                {
                    _dataRepository.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// Method for removing person resposible data that are not exist on client side
        /// </summary>
        /// <param name="updatedMomObject">Application class object of mom</param>
        /// <returns>Task</returns>
        public async Task RemoveNonExistingMainPointAndItsRelatedData(MomAC updatedMomObject)
        {
            Mom updatedMomData = _mapper.Map<Mom>(updatedMomObject);

            // updated list from client side 
            var updatedPersonResponsibleList = new List<MomUserMappingAC>();
            var tempUpdatedPersonResponsibleList = new List<MomUserMappingAC>();

            List<SubPointOfDiscussion> tempSubPointOfDiscussionListToBeDeleted = new List<SubPointOfDiscussion>();
            var dbMainPointList = await _dataRepository.Where<MainDiscussionPoint>(m => !m.IsDeleted && m.MomId == updatedMomObject.Id).Include(s => s.SubPointDiscussionCollection).ThenInclude(p => p.PersonResponsibleCollection).AsNoTracking().ToListAsync();

            #region  remove person responsible 
            var dbPersonResponsibleList = await _dataRepository.Where<MomUserMapping>(v => v.MomId == updatedMomData.Id && !v.IsDeleted && v.SubPointOfDiscussionId != null).AsNoTracking().ToListAsync();
            List<Guid> dbPersonResponsibleIdList = dbPersonResponsibleList.Where(y => y.SubPointOfDiscussionId != null).Select(x => x.UserId).ToList();
            var list = new List<MomUserMapping>();
            //get all the person responsible added in client side 
            for (int m = 0; m < updatedMomObject.MainDiscussionPointACCollection.Count; m++)
            {
                for (int s = 0; s < updatedMomObject.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection.Count; s++)
                {
                    for (int u = 0; u < updatedMomObject.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection[s].PersonResponsibleACCollection.Count; u++)
                    {
                        tempUpdatedPersonResponsibleList = updatedMomObject.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection[s].PersonResponsibleACCollection;
                    }
                    updatedPersonResponsibleList.AddRange(tempUpdatedPersonResponsibleList);
                }
            }

            var removePersonIdList = dbPersonResponsibleList.Where(p => updatedPersonResponsibleList.All(p2 => p2.Id != p.Id)).ToList();

            _dataRepository.RemoveRange(removePersonIdList);
            await _dataRepository.SaveChangesAsync();

            #endregion

            #region  remove sub point
            // remove sub points those are removed from client side in edit mode
            List<SubPointOfDiscussionAC> recordsNotInExistingSubPointList = new List<SubPointOfDiscussionAC>();

            var existingMainPointData = await _dataRepository.Where<MainDiscussionPoint>(m => m.MomId == updatedMomObject.Id && !m.IsDeleted).Include(s => s.SubPointDiscussionCollection).ToListAsync();


            existingMainPointData.ForEach(v =>
            {
                var subPointList = v.SubPointDiscussionCollection;
                tempSubPointOfDiscussionListToBeDeleted.AddRange(subPointList);
            });
            List<SubPointOfDiscussionAC> clientSideSubPointList = new List<SubPointOfDiscussionAC>();

            for (int m = 0; m < updatedMomObject.MainDiscussionPointACCollection.Count; m++)
            {
                clientSideSubPointList = updatedMomObject.MainDiscussionPointACCollection[m].SubPointDiscussionACCollection;
                recordsNotInExistingSubPointList.AddRange(clientSideSubPointList);
            }
            List<Guid> clientSideSubPointids = recordsNotInExistingSubPointList.Where(n => n.Id != null).Select(x => (Guid)x.Id).ToList();
            List<SubPointOfDiscussion> removedSubPointList = tempSubPointOfDiscussionListToBeDeleted.Where(m => !clientSideSubPointids.Contains(m.Id)).ToList();

            _dataRepository.RemoveRange(removedSubPointList);
            await _dataRepository.SaveChangesAsync();

            #endregion

            #region remove main point
            // remove main points those are removed from client side in edit mode
            List<Guid> UpdatedRecordOfMainPoint = updatedMomObject.MainDiscussionPointACCollection.Where(y => y.Id != null).Select(x => (Guid)x.Id).ToList();

            List<MainDiscussionPoint> recordsNotInExistingMainPointList = existingMainPointData.Where(y => !UpdatedRecordOfMainPoint.Contains(y.Id)).ToList();

            for (int i = 0; i < recordsNotInExistingMainPointList.Count; i++)
            {
                recordsNotInExistingMainPointList[i].IsDeleted = true;
                recordsNotInExistingMainPointList[i].UpdatedDateTime = DateTime.UtcNow;
                recordsNotInExistingMainPointList[i].UpdatedBy = currentUserId;
            }
            _dataRepository.UpdateRange(recordsNotInExistingMainPointList);
            await _dataRepository.SaveChangesAsync();

            #endregion
            existingMainPointData.ForEach(x =>
            {
                _dataRepository.DetachEntityEntry(x);
            });

        }



        /// <summary>
        /// Method for adding new data of main point,subpoint and person resposible
        /// </summary>
        /// <param name="momId">Id of mom</param>
        /// <param name="newMainPointList">Owner user detail</param>
        /// <returns>Task</returns>
        public async Task AddNewDataForMainPointAndItsRelatedData(Guid momId, List<MainDiscussionPointAC> newMainPointList)
        {
            // add new main point with new subpoint and person resposible which are added in edit mode
            tempMainDiscussionPointList = new List<MainDiscussionPoint>();
            tempSubPointOfDiscussionListToBeAdded = new List<SubPointOfDiscussion>();
            momUserMappingPersonResposibleList = new List<MomUserMapping>();
            if (newMainPointList.Count > 0)
            {
                for (int i = 0; i < newMainPointList.Count; i++)
                {
                    MainDiscussionPoint mainDiscussionPointToBeAdded = new MainDiscussionPoint()
                    {
                        Id = Guid.NewGuid(),
                        MomId = momId,
                        CreatedBy = currentUserId,
                        CreatedDateTime = DateTime.UtcNow,
                        MainPoint = newMainPointList[i].MainPoint
                    };
                    tempMainDiscussionPointList.Add(mainDiscussionPointToBeAdded);

                    var temsubpointlist = newMainPointList[i].SubPointDiscussionACCollection;
                    if (temsubpointlist.Count > 0)
                    {
                        for (int j = 0; j < temsubpointlist.Count; j++)
                        {
                            SubPointOfDiscussion subPointToBeAdded = new SubPointOfDiscussion()
                            {
                                Id = Guid.NewGuid(),
                                SubPoint = temsubpointlist[j].SubPoint,
                                TargetDate = temsubpointlist[j].TargetDate,
                                Status = temsubpointlist[j].Status,

                                CreatedBy = currentUserId, 
                                CreatedDateTime = DateTime.UtcNow,
                                MainPointId = mainDiscussionPointToBeAdded.Id
                            };
                            tempSubPointOfDiscussionListToBeAdded.Add(subPointToBeAdded);
                            var personListNullIdList = temsubpointlist[j].PersonResponsibleACCollection.Where(y => y.Id == null).ToList();
                            for (int k = 0; k < personListNullIdList.Count; k++)
                            {
                                MomUserMapping momUserMapping = new MomUserMapping()
                                {
                                    MomId = momId,
                                    UserId = temsubpointlist[j].PersonResponsibleACCollection[k].UserId,
                                    CreatedBy=currentUserId,
                                    CreatedDateTime = DateTime.UtcNow,
                                    SubPointOfDiscussionId = subPointToBeAdded.Id
                                };
                                momUserMappingPersonResposibleList.Add(momUserMapping);
                            }

                        }
                    }
                }
            }
            _dataRepository.AddRange(tempMainDiscussionPointList);
            await _dataRepository.SaveChangesAsync();

            _dataRepository.AddRange(tempSubPointOfDiscussionListToBeAdded);
            await _dataRepository.SaveChangesAsync();

            _dataRepository.AddRange(momUserMappingPersonResposibleList);
            await _dataRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Method for updating data of main point,subpoint and person resposible
        /// </summary>
        /// <param name="momId">Application class object of mom</param>
        /// <param name="existingMainPointList">List of existing main point in client side</param>
        /// <returns>Task</returns>
        public async Task UpdateDataForMainPointAndItsRelatedData(Guid momId, List<MainDiscussionPointAC> existingMainPointList)
        {
            ////update main point data 
            if (existingMainPointList.Count > 0)
            {
                for (int i = 0; i < existingMainPointList.Count; i++)
                {
                    MainDiscussionPoint mainDiscussionPoint = new MainDiscussionPoint()
                    {
                        Id = (Guid)existingMainPointList[i].Id,
                        MomId = momId,
                        MainPoint = existingMainPointList[i].MainPoint,
                        CreatedDateTime = (DateTime)existingMainPointList[i].CreatedDateTime,
                        UpdatedBy=currentUserId,
                        UpdatedDateTime = DateTime.UtcNow
                    };

                    tempMainDiscussionPointList.Add(mainDiscussionPoint);

                    // update subpoint of mainpoint
                    var temsubpointlist = existingMainPointList[i].SubPointDiscussionACCollection;
                    var subPointListNotNullIdList = temsubpointlist.Where(y => y.Id != null && !y.IsDeleted).ToList();
                    if (subPointListNotNullIdList.Count > 0)
                    {
                        for (int j = 0; j < subPointListNotNullIdList.Count; j++)
                        {
                            SubPointOfDiscussion subPointOfDiscussion = new SubPointOfDiscussion();

                            subPointOfDiscussion.Id = (Guid)subPointListNotNullIdList[j].Id;
                            subPointOfDiscussion.SubPoint = subPointListNotNullIdList[j].SubPoint;
                            subPointOfDiscussion.TargetDate = subPointListNotNullIdList[j].TargetDate;
                            subPointOfDiscussion.Status = subPointListNotNullIdList[j].Status;
                            subPointOfDiscussion.CreatedDateTime = (DateTime)subPointListNotNullIdList[j].CreatedDateTime;
                            subPointOfDiscussion.UpdatedBy = currentUserId;
                            subPointOfDiscussion.UpdatedDateTime = DateTime.UtcNow;
                            subPointOfDiscussion.MainPointId = mainDiscussionPoint.Id;


                            tempSubPointOfDiscussionListToBeUpdated.Add(subPointOfDiscussion);

                            // update person resposible of subpoint
                            var personListNotNullIdList = subPointListNotNullIdList[j].PersonResponsibleACCollection.Where(p => p.Id != null && !p.IsDeleted).ToList();
                            for (int k = 0; k < personListNotNullIdList.Count; k++)
                            {

                                MomUserMapping updatedMomUserMapping = new MomUserMapping();

                                updatedMomUserMapping.Id = (Guid)personListNotNullIdList[k].Id;
                                updatedMomUserMapping.MomId = momId;
                                updatedMomUserMapping.UserId = personListNotNullIdList[k].UserId;
                                updatedMomUserMapping.CreatedDateTime = (DateTime)personListNotNullIdList[k].CreatedDateTime;
                                updatedMomUserMapping.UpdatedBy = currentUserId;
                                updatedMomUserMapping.UpdatedDateTime = DateTime.UtcNow;
                                updatedMomUserMapping.SubPointOfDiscussionId = subPointOfDiscussion.Id;


                                momUserMappingPersonResposibleList.Add(updatedMomUserMapping);
                            }
                            var personListNullIdListInEdit = subPointListNotNullIdList[j].PersonResponsibleACCollection.Where(p => p.Id == null && !p.IsDeleted).ToList();
                            for (int t = 0; t < personListNullIdListInEdit.Count; t++)
                            {
                                MomUserMapping momUserMappingToBeAdded = new MomUserMapping()
                                {
                                    Id = Guid.NewGuid(),
                                    MomId = momId,
                                    UserId = personListNullIdListInEdit[t].UserId,
                                    CreatedBy=currentUserId,
                                    CreatedDateTime = DateTime.UtcNow,
                                    SubPointOfDiscussionId = subPointOfDiscussion.Id
                                };

                                momUserMappingPersonResposibleListToBeAddedInEdit.Add(momUserMappingToBeAdded);
                            }
                        }
                    }
                    // add  sub point which are added in edit mode
                    var subPointNullIdListToBeAdded = temsubpointlist.Where(y => y.Id == null).ToList();
                    if (subPointNullIdListToBeAdded.Count > 0)
                    {
                        for (int j = 0; j < subPointNullIdListToBeAdded.Count; j++)
                        {
                            SubPointOfDiscussion subPointToBeAdded = new SubPointOfDiscussion()
                            {
                                Id = Guid.NewGuid(),
                                SubPoint = subPointNullIdListToBeAdded[j].SubPoint,
                                TargetDate = subPointNullIdListToBeAdded[j].TargetDate,
                                Status = subPointNullIdListToBeAdded[j].Status,

                                CreatedBy=currentUserId,
                                CreatedDateTime = DateTime.UtcNow,
                                MainPointId = mainDiscussionPoint.Id
                            };
                            tempSubPointOfDiscussionListToBeAdded.Add(subPointToBeAdded);
                            // add person resposible which are added in edit mode
                            var personListNullIdToBeAddedList = subPointNullIdListToBeAdded[j].PersonResponsibleACCollection.Where(y => y.Id == null).ToList();
                            for (int k = 0; k < personListNullIdToBeAddedList.Count; k++)
                            {
                                MomUserMapping momUserMappingToBeAdded = new MomUserMapping()
                                {

                                    MomId = momId,
                                    UserId = personListNullIdToBeAddedList[k].UserId,
                                    CreatedBy=currentUserId,
                                    CreatedDateTime = DateTime.UtcNow,
                                    SubPointOfDiscussionId = subPointToBeAdded.Id
                                };
                                tempMomUserMappingListToBeAdded.Add(momUserMappingToBeAdded);
                            }

                        }
                    }

                }
            }


            await _dataRepository.AddRangeAsync(tempSubPointOfDiscussionListToBeAdded);
            await _dataRepository.SaveChangesAsync();

            await _dataRepository.AddRangeAsync(tempMomUserMappingListToBeAdded);
            await _dataRepository.SaveChangesAsync();

            await _dataRepository.AddRangeAsync(momUserMappingPersonResposibleListToBeAddedInEdit);
            await _dataRepository.SaveChangesAsync();

            _dataRepository.UpdateRange(tempMainDiscussionPointList);
            await _dataRepository.SaveChangesAsync();

            _dataRepository.UpdateRange(tempSubPointOfDiscussionListToBeUpdated);
            await _dataRepository.SaveChangesAsync();

            _dataRepository.UpdateRange(momUserMappingPersonResposibleList);
            await _dataRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Method for removing team/client participant data that are not exist on client side
        /// </summary>
        /// <param name="updatedMomDetails">Application class object of mom</param>
        /// <param name="teamUserList">List of team and client participant from db</param>
        /// <returns>Task</returns>
        public async Task UpdateMomTeamData(MomAC updatedMomDetails, List<MomUserMapping> teamUserList)
        {

            List<Guid> teamUserIdList = teamUserList.Select(x => x.UserId).ToList();

            // all the updated user id list from client side
            List<Guid> updatedUserIdList = updatedMomDetails.InternalUserList.Select(x => x.UserId).ToList();
            updatedUserIdList.AddRange(updatedMomDetails.ExternalUserList.Select(x => x.UserId).ToList());

            // delete user id which are not in client side
            List<Guid> deleteUserIdList = teamUserIdList.Except(updatedUserIdList).ToList();

            // add user id which are added in client side
            List<Guid> addUserIdList = updatedUserIdList.Except(teamUserIdList).ToList();



            #region Delete MomTeam
            List<MomUserMapping> momTeamDeleteList = teamUserList.Where(x => deleteUserIdList.Contains(x.UserId)).ToList();
            for (int u = 0; u < momTeamDeleteList.Count; u++)
            {
                momTeamDeleteList[u].IsDeleted = true;
                momTeamDeleteList[u].UpdatedDateTime = DateTime.UtcNow;
                momTeamDeleteList[u].UpdatedBy = currentUserId;
            }
            _dataRepository.UpdateRange(momTeamDeleteList);

            #endregion

            #region Add MomTeam list
            List<MomUserMappingAC> momTeamACList = updatedMomDetails.InternalUserList.Where(x => addUserIdList.Contains(x.UserId)).ToList();
            momTeamACList.AddRange(updatedMomDetails.ExternalUserList.Where(x => addUserIdList.Contains(x.UserId)).ToList());

            List<MomUserMapping> momTeamAddList = _mapper.Map<List<MomUserMapping>>(momTeamACList);

            momTeamAddList.ForEach(x =>
            {
                x.MomId = (Guid)updatedMomDetails.Id;
                x.CreatedDateTime = DateTime.UtcNow;
                x.CreatedBy = currentUserId;
            });
            await _dataRepository.AddRangeAsync(momTeamAddList);
            await _dataRepository.SaveChangesAsync();
            #endregion
        }

        /// <summary>
        /// Method for deleting mom
        /// </summary>
        /// <param name="Id">Id of mom</param>
        /// <returns>Task</returns>
        public async Task DeleteMomAsync(Guid Id)
        {
            using (_dataRepository.BeginTransaction())
            {
                try
                {
                    Mom deleteMom = await _dataRepository.Where<Mom>(x => x.Id == Id && !x.IsDeleted)
                .Include(w => w.WorkProgram)
                .Include(m => m.MainDiscussionPointCollection)
                .ThenInclude(s => s.SubPointDiscussionCollection).ThenInclude(f => f.PersonResponsibleCollection).AsNoTracking().FirstOrDefaultAsync();
                    var momUserDetail = await _dataRepository.FirstAsync<User>(x => !x.IsDeleted);
                    deleteMom.IsDeleted = true;
                    deleteMom.UpdatedDateTime = DateTime.UtcNow;
                    deleteMom.UpdatedBy = currentUserId;
                    _dataRepository.Update(deleteMom);
                    await _dataRepository.SaveChangesAsync();

                    List<MainDiscussionPoint> mainDiscussionPointList = deleteMom.MainDiscussionPointCollection.ToList();
                    List<SubPointOfDiscussion> subPointOfDiscussionDeleteList = new List<SubPointOfDiscussion>();
                    momUserMappingList = new List<MomUserMapping>();
                    for (int j = 0; j < mainDiscussionPointList.Count; j++)
                    {
                        mainDiscussionPointList[j].IsDeleted = true;
                        mainDiscussionPointList[j].UpdatedDateTime = DateTime.UtcNow;
                        mainDiscussionPointList[j].UpdatedBy = currentUserId;
                        if (mainDiscussionPointList.ToList()[j].SubPointDiscussionCollection != null)
                        {
                            subPointOfDiscussionDeleteList = mainDiscussionPointList.ToList()[j].SubPointDiscussionCollection.ToList();
                            for (int k = 0; k < mainDiscussionPointList[j].SubPointDiscussionCollection.Count; k++)
                            {
                                if (!subPointOfDiscussionDeleteList[k].IsDeleted)
                                {
                                    subPointOfDiscussionDeleteList[k].IsDeleted = true;
                                    subPointOfDiscussionDeleteList[k].UpdatedDateTime = DateTime.UtcNow;
                                    subPointOfDiscussionDeleteList[k].UpdatedBy = currentUserId;
                                }
                                momUserMappingList = mainDiscussionPointList.ToList()[j].SubPointDiscussionCollection.ToList()[k].PersonResponsibleCollection;
                                for (int i = 0; i < mainDiscussionPointList.ToList()[j].SubPointDiscussionCollection.ToList()[k].PersonResponsibleCollection.Count; i++)
                                {
                                    if (!momUserMappingList[i].IsDeleted)
                                    {
                                        momUserMappingList[i].IsDeleted = true;
                                        momUserMappingList[i].UpdatedDateTime = DateTime.UtcNow;
                                        momUserMappingList[i].UpdatedBy = currentUserId;
                                    }
                                }
                            }
                        }
                    }
                    _dataRepository.UpdateRange(mainDiscussionPointList);
                    await _dataRepository.SaveChangesAsync();
                    _dataRepository.UpdateRange(subPointOfDiscussionDeleteList);
                    await _dataRepository.SaveChangesAsync();
                    _dataRepository.UpdateRange(momUserMappingList);
                    await _dataRepository.SaveChangesAsync();

                    _dataRepository.CommitTransaction();
                }
                catch (Exception)
                {
                    _dataRepository.RollbackTransaction();
                    throw;

                }
            }
        }

        /// <summary>
        /// Method for getting predefined data for mom
        /// </summary>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>List of users and workprogram</returns>
        public async Task<MomAC> GetPredefinedDataForMomAsync(Guid entityId)
        {
            List<WorkProgramAC> workProgramACList = await _workProgramRepository.GetAllWorkProgramsAsync(entityId);
            List<UserAC> userACList = await _userRepository.GetAllUsersOfEntityAsync(entityId);
            MomAC momAc = new MomAC();
            momAc.WorkProgramCollection = workProgramACList;
            momAc.TeamCollection = userACList.Where(x => x.UserType == UserType.Internal).ToList();
            momAc.ClientParticipantCollection = userACList.Where(x => x.UserType == UserType.External).ToList();
            momAc.AllPersonResposibleACDataCollection = userACList;
            return momAc;
        }

        /// <summary>
        /// Export Moms to Excel
        /// </summary>
        /// <param name="entityId">Id of entity</param>
        /// <param name="offset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> ExportMomsAsync(string entityId, int offset)
        {
            List<Mom> momList = await _dataRepository.Where<Mom>(a => !a.IsDeleted && a.EntityId.ToString() == entityId)
                .Include(w => w.WorkProgram).Include(m => m.MainDiscussionPointCollection)
                .ThenInclude(s => s.SubPointDiscussionCollection).ThenInclude(f => f.PersonResponsibleCollection)
                .Include(z => z.MomUserMappingCollection).ThenInclude(b => b.User)
                .OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();

            //export mom list

            List<MomAC> exportMomsList = _mapper.Map<List<MomAC>>(momList);
            if (exportMomsList.Count == 0)
            {
                MomAC reportAC = new MomAC();
                exportMomsList.Add(reportAC);
            }

            exportMomsList.ForEach(a =>
            {
                a.WorkProgramString = a.Id != null ? a.WorkProgramAc.Name.ToString() : string.Empty;
                a.MomDateToString = (a.Id != null && a.MomDateToString != null) ? a.MomDate.AddMinutes(-1 * offset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                a.StartTime = a.Id != null ? a.MomStartTime.AddMinutes(-1 * offset).ToString(StringConstant.DateTimeFormatOnlyTime) : string.Empty;
                a.EndTime = a.Id != null ? a.MomEndTime.AddMinutes(-1 * offset).ToString(StringConstant.DateTimeFormatOnlyTime) : string.Empty;
                a.ClosureMeetingDateToString = a.Id != null ? a.ClosureMeetingDate.AddMinutes(-1 * offset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                a.Agenda = a.Id != null ? a.Agenda : string.Empty;
                a.CreatedDate = (a.Id != null && a.CreatedDateTime != null) ? a.CreatedDateTime.Value.AddMinutes(-1 * offset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                a.UpdatedDate = (a.Id != null && a.UpdatedDateTime != null) ? a.UpdatedDateTime.Value.AddMinutes(-1 * offset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            List<MainDiscussionPointAC> exportMainPointACList = new List<MainDiscussionPointAC>();
            if (exportMomsList[0].Id == null && exportMainPointACList.Count == 0)
            {
                MainDiscussionPointAC mainDiscussionPointAC = new MainDiscussionPointAC();
                exportMainPointACList.Add(mainDiscussionPointAC);
            }
            List<MomUserMappingAC> exportMomTeamUserList = new List<MomUserMappingAC>();
            if (exportMomsList[0].Id == null && exportMomTeamUserList.Count == 0)
            {
                MomUserMappingAC momUserMappingAC = new MomUserMappingAC();
                exportMomTeamUserList.Add(momUserMappingAC);
            }
            if (exportMomsList.Count > 0)
            {
                // merge 3 lists, main point ,sub point and person responsible  and create new list with repeated data
                for (int i = 0; i < exportMomsList.Count; i++)
                {
                    MomAC addedRepeatedMom = _mapper.Map<MomAC>(exportMomsList[i]);
                    if (exportMomsList[i].MainDiscussionPointACCollection != null)
                    {
                        List<MainDiscussionPointAC> repeatedMainPointList = _mapper.Map<List<MainDiscussionPointAC>>(exportMomsList[i].MainDiscussionPointACCollection.Where(v => !v.IsDeleted).OrderByDescending(y => y.CreatedDateTime).ToList());

                        List<MomUserMappingAC> repeatedPersonList = _mapper.Map<List<MomUserMappingAC>>(momList[i].MomUserMappingCollection.Where(x => x.SubPointOfDiscussionId != null && !x.IsDeleted).OrderByDescending(y => y.CreatedDateTime).ToList());

                        if (repeatedMainPointList.Count != 0)
                        {
                            for (int l = 0; l < exportMomsList[i].MainDiscussionPointACCollection.Count; l++)
                            {
                                for (int s = 0; s < exportMomsList[i].MainDiscussionPointACCollection[l].SubPointDiscussionACCollection.Count; s++)
                                {
                                    for (int p = 0; p < exportMomsList[i].MainDiscussionPointACCollection[l].SubPointDiscussionACCollection[s].PersonResponsibleACCollection.Count; p++)
                                    {
                                        MainDiscussionPointAC getRepeatedMainPointList = new MainDiscussionPointAC();
                                        getRepeatedMainPointList.Id = addedRepeatedMom.MainDiscussionPointACCollection[l].Id;
                                        getRepeatedMainPointList.MainPoint = addedRepeatedMom.MainDiscussionPointACCollection[l].MainPoint;
                                        getRepeatedMainPointList.Agenda = addedRepeatedMom.Agenda;
                                        getRepeatedMainPointList.WorkProgram = addedRepeatedMom.WorkProgramString;
                                        getRepeatedMainPointList.CreatedBy = addedRepeatedMom.CreatedBy;
                                        getRepeatedMainPointList.CreatedDateTime = addedRepeatedMom.CreatedDateTime;
                                        getRepeatedMainPointList.IsDeleted = addedRepeatedMom.IsDeleted;
                                        getRepeatedMainPointList.MomId = addedRepeatedMom.Id;

                                        getRepeatedMainPointList.UpdatedBy = addedRepeatedMom.UpdatedBy;
                                        getRepeatedMainPointList.UpdatedDateTime = addedRepeatedMom.UpdatedDateTime;
                                        getRepeatedMainPointList.SubPointDiscussionACCollection = addedRepeatedMom.MainDiscussionPointACCollection[l].SubPointDiscussionACCollection;
                                        getRepeatedMainPointList.SubPoint = addedRepeatedMom.MainDiscussionPointACCollection[l].SubPointDiscussionACCollection[s].SubPoint;
                                        getRepeatedMainPointList.StatusString = getRepeatedMainPointList.Id != null && exportMomsList[i].MainDiscussionPointACCollection[l].SubPointDiscussionACCollection[s].Status.ToString() == StringConstant.CompletedStatusString ? StringConstant.CompletedStatusString : exportMomsList[i].MainDiscussionPointACCollection[l].SubPointDiscussionACCollection[s].Status.ToString() == StringConstant.PendingStatusString ? StringConstant.PendingStatusString : StringConstant.InProgressStatusString;
                                        getRepeatedMainPointList.TargetDateToString = (getRepeatedMainPointList.Id != null && exportMomsList[i].MainDiscussionPointACCollection[l].SubPointDiscussionACCollection[s].TargetDate != null) ? exportMomsList[i].MainDiscussionPointACCollection[l].SubPointDiscussionACCollection[s].TargetDate.AddMinutes(-1 * offset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                                        getRepeatedMainPointList.PersonResponsibleName = repeatedPersonList != null ? repeatedPersonList.FirstOrDefault(x => x.UserId == exportMomsList[i].MainDiscussionPointACCollection[l].SubPointDiscussionACCollection[s].PersonResponsibleACCollection[p].UserId && x.SubPointOfDiscussionId == exportMomsList[i].MainDiscussionPointACCollection[l].SubPointDiscussionACCollection[s].Id)?.Name : string.Empty;
                                        getRepeatedMainPointList.Designation = repeatedPersonList != null ? repeatedPersonList.FirstOrDefault(x => x.UserId == exportMomsList[i].MainDiscussionPointACCollection[l].SubPointDiscussionACCollection[s].PersonResponsibleACCollection[p].UserId && x.SubPointOfDiscussionId == exportMomsList[i].MainDiscussionPointACCollection[l].SubPointDiscussionACCollection[s].Id)?.Designation : string.Empty;
                                        getRepeatedMainPointList.CreatedDate = (getRepeatedMainPointList.Id != null && getRepeatedMainPointList.CreatedDateTime != null) ? getRepeatedMainPointList.CreatedDateTime.Value.AddMinutes(-1 * offset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                                        getRepeatedMainPointList.UpdatedDate = (getRepeatedMainPointList.Id != null && getRepeatedMainPointList.UpdatedDateTime != null) ? getRepeatedMainPointList.UpdatedDateTime.Value.AddMinutes(-1 * offset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                                        exportMainPointACList.Add(getRepeatedMainPointList);
                                    }
                                }
                            }
                        }
                    }
                }


                // all team and client participant of mom

                for (int i = 0; i < exportMomsList.Count; i++)
                {
                    if (momList.Count > 0 && momList[i].MomUserMappingCollection != null)
                    {
                        List<MomUserMappingAC> momTeamUserList = _mapper.Map<List<MomUserMappingAC>>(momList[i].MomUserMappingCollection.Where(x => x.SubPointOfDiscussionId == null && !x.IsDeleted).OrderByDescending(y => y.CreatedDateTime).ToList());
                        for (int j = 0; j < momTeamUserList.Count; j++)
                        {
                            MomUserMappingAC getRepeatedTeamList = new MomUserMappingAC();
                            getRepeatedTeamList.Id = momTeamUserList[j].Id != null ? momTeamUserList[j].Id : Guid.Empty;
                            getRepeatedTeamList.MomId = momTeamUserList[j].Id != null ? momTeamUserList[j].MomId : Guid.Empty;
                            getRepeatedTeamList.Name = momTeamUserList[j].Id != null ? momTeamUserList[j]?.Name : string.Empty;
                            getRepeatedTeamList.Type = momTeamUserList[j].Id != null && momTeamUserList[j].User.UserType.ToString() == StringConstant.InternalTypeString ? StringConstant.TeamString : momTeamUserList[j].User.UserType.ToString() == StringConstant.ExternalTypeString ? StringConstant.ClientString : StringConstant.SuperadminTypeString;
                            getRepeatedTeamList.Designation = momTeamUserList[j].Id != null ? momTeamUserList[j]?.Designation : string.Empty;
                            getRepeatedTeamList.WorkProgram = momTeamUserList[j].Id != null ? momList[i].WorkProgram?.Name : string.Empty;
                            getRepeatedTeamList.Agenda = momTeamUserList[j].Id != null ? momList[i]?.Agenda : string.Empty;
                            getRepeatedTeamList.CreatedDateTime = momTeamUserList[j].CreatedDateTime;
                            getRepeatedTeamList.UpdatedDateTime = momTeamUserList[j].UpdatedDateTime;
                            getRepeatedTeamList.CreatedDate = (momTeamUserList.Count > 0 && getRepeatedTeamList.CreatedDateTime != null) ? getRepeatedTeamList.CreatedDateTime.Value.AddMinutes(-1 * offset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                            getRepeatedTeamList.UpdatedDate = (momTeamUserList.Count > 0 && getRepeatedTeamList.UpdatedDateTime != null) ? getRepeatedTeamList.UpdatedDateTime.Value.AddMinutes(-1 * offset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                            exportMomTeamUserList.Add(getRepeatedTeamList);
                        }
                    }
                }
            }
            //crete dynamic directory
            dynamic dynamicDictionary = new DynamicDictionary<string, dynamic>();
            dynamicDictionary.Add(StringConstant.MomModuleName, exportMomsList);
            dynamicDictionary.Add(StringConstant.MainPointModule, exportMainPointACList);
            dynamicDictionary.Add(StringConstant.MomTeamModule, exportMomTeamUserList);

            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFileWithMultipleTable(dynamicDictionary, StringConstant.MomModule + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method for generating pdf file
        /// </summary>
        /// <param name="momId">Id of mom</param>
        /// <param name="offset">offset of client</param>
        /// <returns>Data of file in memory stream</returns>
        public async Task<MemoryStream> DownloadMomPDFAsync(string momId, double offset)
        {

            Mom getMomAcById = await _dataRepository.Where<Mom>(x => x.Id == Guid.Parse(momId) && !x.IsDeleted)
                  .Include(w => w.WorkProgram)
                  .Include(m => m.MainDiscussionPointCollection)
                  .ThenInclude(s => s.SubPointDiscussionCollection).ThenInclude(f => f.PersonResponsibleCollection)
                  .Include(z => z.MomUserMappingCollection).AsNoTracking().FirstAsync();

            MomAC momAC = _mapper.Map<MomAC>(getMomAcById);
            momAC.MainDiscussionPointACCollection = _mapper.Map<List<MainDiscussionPointAC>>(getMomAcById.MainDiscussionPointCollection.Where(x => !x.IsDeleted).OrderBy(y => y.CreatedDateTime));
            List<MomUserMapping> momUserList = await _dataRepository.Where<MomUserMapping>(x =>
                                      x.MomId == Guid.Parse(momId) && !x.IsDeleted).Include(x => x.User).AsNoTracking().ToListAsync();

            momAC.InternalUserList = _mapper.Map<List<MomUserMappingAC>>(momUserList.Where(x => x.SubPointOfDiscussionId == null && x.User.UserType == UserType.Internal).ToList());
            momAC.ExternalUserList = _mapper.Map<List<MomUserMappingAC>>(momUserList.Where(x => x.SubPointOfDiscussionId == null && x.User.UserType == UserType.External).ToList());

            momAC.MomDate = momAC.MomDate.AddMinutes(-1 * offset);
            momAC.ClosureMeetingDate = momAC.ClosureMeetingDate.AddMinutes(-1 * offset);
            momAC.MomStartTime = momAC.MomStartTime.AddMinutes(-1 * offset);
            momAC.MomEndTime = momAC.MomEndTime.AddMinutes(-1 * offset);

            var result = await _viewRenderService.RenderToStringAsync(StringConstant.MomPdfFilePath, momAC, StringConstant.MomPdfFileName + momId);
            var headerResult = await _viewRenderService.RenderToStringAsync(StringConstant.MomHeaderPdfFilePath, momAC, StringConstant.MomHeaderPdfFileName + momId);
            momAC.PdfFileString = result;

            var requestUrl = _iConfig.GetValue<string>("PdfGenerationRequestUrl");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var jsonContent = JsonConvert.SerializeObject(new { html = momAC.PdfFileString, pdfOptions = new { headerTemplate = headerResult, footerTemplate= " ", displayHeaderFooter = true, margin = new { top = "100px", bottom = "100px", right = "30px", left = "30px" } } });
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync(requestUrl, httpContent);

            var memoryStream = new MemoryStream();

            await httpResponse.Content.CopyToAsync(memoryStream);

            memoryStream.Position = 0;

            return memoryStream;
        }


        #endregion
    }
}
