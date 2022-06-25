using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.AzureBlobStorage;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Utility.FileUtil;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InternalAuditSystem.Repository.Repository.AuditableEntityRepository.EntityDocumentRepository
{
    public class EntityDocumentRepository : IEntityDocumentRepository
    {
        #region Private variable(s)
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IGlobalRepository _globalRepository;
        private readonly IFileUtility _fileUtility;
        private readonly IAzureRepository _azureRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        #endregion

        #region Public method(s)
        public EntityDocumentRepository(
            IDataRepository dataRepository,
            IMapper mapper,
            IGlobalRepository globalRepository,
             IFileUtility fileUtility,
            IAzureRepository azureRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _globalRepository = globalRepository;
            _fileUtility = fileUtility;
            _azureRepository = azureRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
        }

        /// <summary>
        /// Get EntityDocument List
        /// </summary>
        /// <param name="pagination">Pagination object</param>
        /// <returns>Pagination object</returns>
        public async Task<Pagination<EntityDocumentAC>> GetEntityDocumentListAsync(Pagination<EntityDocumentAC> pagination)
        {
            // Apply pagination
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);

            IQueryable<EntityDocumentAC> entityDocumentList = _dataRepository.Where<EntityDocument>(x =>
                                                                 x.EntityId == pagination.EntityId
                                                                 && (!String.IsNullOrEmpty(pagination.searchText) ? x.Purpose.ToLower().Contains(pagination.searchText.ToLower()) : true)
                                                                 && !x.IsDeleted)
                                                                 .Select(x => new EntityDocumentAC
                                                                 {
                                                                     Id = x.Id,
                                                                     EntityId = x.EntityId,
                                                                     Purpose = x.Purpose,
                                                                     FileName = x.Path,
                                                                     Path = _azureRepository.DownloadFile(x.Path),
                                                                     CreatedDateTime = x.CreatedDateTime
                                                                 });

            //Get total count
            pagination.TotalRecords = entityDocumentList.Count();

            if (pagination.PageSize == 0)//Get all records
            {
                pagination.Items = await entityDocumentList.OrderByDescending(x => x.CreatedDateTime).Skip(0).Take(pagination.TotalRecords).ToListAsync();
            }
            else
            {
                pagination.Items = await entityDocumentList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToListAsync();

            }
            return pagination;
        }

        /// <summary>
        /// Get EntityDocument details
        /// </summary>
        /// <param name="id">EntityDocument id</param>
        /// <returns>EntityDocumentAC</returns>
        public async Task<EntityDocumentAC> GetEntityDocumentDetailsAsync(Guid id)
        {
            EntityDocument entityDocument = await _dataRepository.FirstOrDefaultAsync<EntityDocument>(x => x.Id == id && !x.IsDeleted);

            EntityDocumentAC entityDocumentAC = _mapper.Map<EntityDocumentAC>(entityDocument);

            entityDocumentAC.FileName = entityDocumentAC.Path;
            entityDocumentAC.Path = _azureRepository.DownloadFile(entityDocumentAC.Path);

            return entityDocumentAC;
        }

        /// <summary>
        /// Delete EntityDocument
        /// </summary>
        /// <param name="id">EntityDocument id</param>
        /// <returns>Void</returns>
        public async Task DeleteEntityDocumentAync(Guid id)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    EntityDocument entityDocument = await _dataRepository.FirstOrDefaultAsync<EntityDocument>(x => x.Id == id && !x.IsDeleted);
                    entityDocument.IsDeleted = true;
                    entityDocument.UpdatedBy = currentUserId;
                    entityDocument.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update<EntityDocument>(entityDocument);
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
        /// Update EntityDocument
        /// </summary>
        /// <param name="EntityDocumentAC">EntityDocument AC object</param>
        /// <returns>Void</returns>
        public async Task UpdateEntityDocumentAsync(EntityDocumentAC entityDocumentAC)
        {
            EntityDocument entityDocument = new EntityDocument();
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    entityDocument = _mapper.Map<EntityDocument>(entityDocumentAC);

                    entityDocument.UpdatedDateTime = DateTime.UtcNow;
                    entityDocument.UpdatedBy = currentUserId;

                    if (entityDocumentAC.EntityDocumentFiles != null && entityDocumentAC.EntityDocumentFiles.Count > 0)
                    {
                        List<string> filesUrl = new List<string>();

                        //validation

                        bool isValidFormate = _globalRepository.CheckFileExtention(entityDocumentAC.EntityDocumentFiles);

                        if (isValidFormate)
                        {
                            throw new InvalidFileFormate();
                        }

                        bool isFileSizeValid = _globalRepository.CheckFileSize(entityDocumentAC.EntityDocumentFiles);

                        if (isFileSizeValid)
                        {
                            throw new InvalidFileSize();
                        }

                        //Upload file to azure
                        filesUrl = await _fileUtility.UploadFilesAsync(entityDocumentAC.EntityDocumentFiles);

                        entityDocument.Path = filesUrl[0];
                    }
                    else
                    {
                        entityDocument.Path = entityDocumentAC.FileName;
                    }

                    _dataRepository.Update(entityDocument);
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
        /// Add EntityDocument to database
        /// </summary>
        /// <param name="EntityDocumentAC">EntityDocument model to be added</param>
        /// <returns>Added EntityDocument</returns>
        public async Task<EntityDocumentAC> AddEntityDocumentAsync(EntityDocumentAC entityDocumentAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    EntityDocument entityDocument = new EntityDocument();
                   
                    entityDocumentAC.Id = new Guid();
                    entityDocument = _mapper.Map<EntityDocument>(entityDocumentAC);
                    entityDocument.CreatedDateTime = DateTime.UtcNow;
                    entityDocument.CreatedBy = currentUserId;

                    List<string> filesUrl = new List<string>();

                    //validation

                    bool isValidFormate = _globalRepository.CheckFileExtention(entityDocumentAC.EntityDocumentFiles);

                    if (isValidFormate)
                    {
                        throw new InvalidFileFormate();
                    }

                    bool isFileSizeValid = _globalRepository.CheckFileSize(entityDocumentAC.EntityDocumentFiles);

                    if (isFileSizeValid)
                    {
                        throw new InvalidFileSize();
                    }


                    //Upload file to azure
                    filesUrl = await _fileUtility.UploadFilesAsync(entityDocumentAC.EntityDocumentFiles);

                    entityDocument.Path = filesUrl[0];

                    await _dataRepository.AddAsync(entityDocument);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    return _mapper.Map<EntityDocumentAC>(entityDocument);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        /// <summary>
        /// Method to download EntityDocument
        /// </summary>
        /// <param name="id">EntityDocument Id</param>
        /// <returns>Download url string</returns>
        public async Task<string> DownloadEntityDocumentAsync(Guid id)
        {
            EntityDocument entityDocument = await _dataRepository.FirstAsync<EntityDocument>(x => x.Id == id && !x.IsDeleted);
            return _fileUtility.DownloadFile(entityDocument.Path);
        }


        /// <summary>
        /// Method to Add EntityDocument In New Version
        /// </summary>
        /// <param name="EntityDocumentACList">EntityDocumentAC list</param>
        /// <param name="versionId">New version entityId</param>
        /// <returns>Void</returns>
        public async Task AddEntityDocumentInNewVersionAsync(List<EntityDocumentAC> entityDocumentACList, Guid versionId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {                 
                    List<Guid> entityDocumentIdList = entityDocumentACList.Select(x => x.Id ?? new Guid()).ToList();
                    List<EntityDocument> entityDocumentList = _mapper.Map<List<EntityDocument>>(entityDocumentACList);

                    for (var i = 0; i < entityDocumentList.Count(); i++)
                    {
                        entityDocumentList[i].Id = new Guid();
                        entityDocumentList[i].EntityId = versionId;
                        entityDocumentList[i].CreatedBy = currentUserId;
                        entityDocumentList[i].CreatedDateTime = DateTime.UtcNow;
                    }
                    await _dataRepository.AddRangeAsync(entityDocumentList);
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
        #endregion
    }
}
