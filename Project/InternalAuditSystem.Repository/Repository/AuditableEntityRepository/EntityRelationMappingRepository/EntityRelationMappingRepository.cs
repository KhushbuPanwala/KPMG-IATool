using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Repository.Repository.RelationshipTypeRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditableEntityRepository.EntityRelationMappingRepository
{
    public class EntityRelationMappingRepository : IEntityRelationMappingRepository
    {
        #region Private variable(s)
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IGlobalRepository _globalRepository;
        public IRelationshipTypeRepository _relationshipTypeRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        #endregion

        #region Public method(s)
        public EntityRelationMappingRepository(
            IDataRepository dataRepository,
            IMapper mapper,
            IGlobalRepository globalRepository,
           IRelationshipTypeRepository relationshipTypeRepository,
           IHttpContextAccessor httpContextAccessor)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _globalRepository = globalRepository;
            _relationshipTypeRepository = relationshipTypeRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
        }

        /// <summary>
        /// Get EntityRelationMapping List
        /// </summary>
        /// <param name="pagination">Pagination object</param>
        /// <returns>Pagination object</returns>
        public async Task<Pagination<EntityRelationMappingAC>> GetEntityRelationMappingListAsync(Pagination<EntityRelationMappingAC> pagination)
        {
            // Apply pagination
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);

            IQueryable<EntityRelationMappingAC> entityRelationMappingList = _dataRepository.Where<EntityRelationMapping>(x =>
                                                                 x.EntityId == pagination.EntityId
                                                                 && (!String.IsNullOrEmpty(pagination.searchText) ? x.RelatedAuditableEntity.Name.ToLower().Contains(pagination.searchText.ToLower()) : true)
                                                                 && !x.IsDeleted).Include(x => x.AuditableEntity)
                                                                 .Include(x => x.RelationshipType)
                                                                 .Select(x => new EntityRelationMappingAC
                                                                 {
                                                                     Id = x.Id,
                                                                     EntityId = x.EntityId,
                                                                     RelatedEntityId = x.RelatedEntityId,
                                                                     RelatedEntityName = x.RelatedAuditableEntity.Name,
                                                                     RelationTypeId = x.RelationTypeId,
                                                                     RelationName = x.RelationshipType.Name,
                                                                     CreatedDateTime = x.CreatedDateTime
                                                                 });

            //Get total count
            pagination.TotalRecords = entityRelationMappingList.Count();

            if (pagination.PageSize == 0)//Get all records
            {
                pagination.Items = await entityRelationMappingList.OrderByDescending(x => x.CreatedDateTime).Skip(0).Take(pagination.TotalRecords).ToListAsync();
            }
            else
            {
                pagination.Items = await entityRelationMappingList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToListAsync();

                if (pagination.Items == null || pagination.Items.Count() == 0)
                {
                    pagination.Items.Add(new EntityRelationMappingAC());
                }
                //For entity dropdown 
                Pagination<AuditableEntityAC> auditableEntityPagination = new Pagination<AuditableEntityAC>()
                {
                    PageSize = 0
                };
                List<AuditableEntity> auditableEntityList = await _dataRepository.Where<AuditableEntity>(x => x.Id != pagination.EntityId && x.IsStrategyAnalysisDone && !x.IsDeleted).ToListAsync();
                pagination.Items[0].AuditableEntityACList = _mapper.Map<List<AuditableEntityAC>>(auditableEntityList);

                //For relationship type dropdown
                pagination.Items[0].RelationshipTypeACList = await _relationshipTypeRepository.GetAllRelationshipTypeByEntityIdAsync(pagination.EntityId);
            }
            return pagination;
        }



        /// <summary>
        /// Delete EntityRelationMapping
        /// </summary>
        /// <param name="id">EntityRelationMapping id</param>
        /// <returns>Void</returns>
        public async Task DeleteEntityRelationMappingAync(Guid id)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    EntityRelationMapping entityRelationMapping = await _dataRepository.FirstOrDefaultAsync<EntityRelationMapping>(x => x.Id == id && !x.IsDeleted);
                    entityRelationMapping.IsDeleted = true;
                    entityRelationMapping.UpdatedBy = currentUserId;
                    entityRelationMapping.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update<EntityRelationMapping>(entityRelationMapping);
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
        /// Update EntityRelationMapping
        /// </summary>
        /// <param name="EntityRelationMappingAC">EntityRelationMapping AC object</param>
        /// <returns>Void</returns>
        public async Task UpdateEntityRelationMappingAsync(EntityRelationMappingAC entityRelationMappingAC)
        {
            EntityRelationMapping entityRelationMapping = new EntityRelationMapping();
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    if (CheckIfEntityRelationMappingExistAsync(entityRelationMappingAC))
                    {
                        throw new DuplicateDataException(StringConstant.EntityRelationModuleName, entityRelationMappingAC.RelatedEntityName);
                    }
                    entityRelationMapping = _mapper.Map<EntityRelationMapping>(entityRelationMappingAC);

                    entityRelationMapping.UpdatedDateTime = DateTime.UtcNow;
                    entityRelationMapping.UpdatedBy = currentUserId;

                    _dataRepository.Update(entityRelationMapping);
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
        /// Add EntityRelationMapping to database
        /// </summary>
        /// <param name="EntityRelationMappingAC">EntityRelationMapping model to be added</param>
        /// <returns>Added EntityRelationMapping</returns>
        public async Task<EntityRelationMappingAC> AddEntityRelationMappingAsync(EntityRelationMappingAC entityRelationMappingAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    if (CheckIfEntityRelationMappingExistAsync(entityRelationMappingAC))
                    {
                        throw new DuplicateDataException(StringConstant.EntityRelationModuleName, entityRelationMappingAC.RelatedEntityName);
                    }
                    EntityRelationMapping entityRelationMapping = new EntityRelationMapping();

                    entityRelationMappingAC.Id = new Guid();
                    entityRelationMapping = _mapper.Map<EntityRelationMapping>(entityRelationMappingAC);

                    entityRelationMapping.CreatedDateTime = DateTime.UtcNow;
                    entityRelationMapping.CreatedBy = currentUserId;

                    await _dataRepository.AddAsync(entityRelationMapping);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    return _mapper.Map<EntityRelationMappingAC>(entityRelationMapping);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        /// <summary>
        /// Method to Add EntityRelationMapping In New Version
        /// </summary>
        /// <param name="EntityRelationMappingACList">EntityRelationMappingAC list</param>
        /// <param name="versionId">New version entityId</param>
        /// <returns>Void</returns>
        public async Task AddEntityRelationMappingInNewVersionAsync(List<EntityRelationMappingAC> entityRelationMappingACList, Guid versionId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    List<Guid> entityRelationMappingIdList = entityRelationMappingACList.Select(x => x.Id ?? new Guid()).ToList();
                    List<EntityRelationMapping> entityRelationMappingList = _mapper.Map<List<EntityRelationMapping>>(entityRelationMappingACList);

                    for (var i = 0; i < entityRelationMappingList.Count(); i++)
                    {
                        entityRelationMappingList[i].Id = new Guid();
                        entityRelationMappingList[i].EntityId = versionId;
                        entityRelationMappingList[i].CreatedBy = currentUserId;
                        entityRelationMappingList[i].CreatedDateTime = DateTime.UtcNow;
                    }
                    await _dataRepository.AddRangeAsync(entityRelationMappingList);
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

        #region Private method(s)

        /// <summary>
        /// Check if EntityRelationMapping name exist
        /// </summary>
        /// <param name="entityRelationMappingAC">entityRelationMappingAC details</param>
        /// <returns>Returns if details are duplicate or not</returns>
        private bool CheckIfEntityRelationMappingExistAsync(EntityRelationMappingAC entityRelationMappingAC)
        {
            bool isRecordExists;
            if (entityRelationMappingAC.Id != null)
            {
                isRecordExists = _dataRepository.Any<EntityRelationMapping>(x => x.Id != entityRelationMappingAC.Id && x.EntityId == entityRelationMappingAC.EntityId && !x.IsDeleted && x.RelatedEntityId == entityRelationMappingAC.RelatedEntityId && x.RelationTypeId == entityRelationMappingAC.RelationTypeId);
            }
            else
            {
                isRecordExists = _dataRepository.Any<EntityRelationMapping>(x => x.EntityId == entityRelationMappingAC.EntityId && !x.IsDeleted && x.RelatedEntityId == entityRelationMappingAC.RelatedEntityId && x.RelationTypeId == entityRelationMappingAC.RelationTypeId);

            }
            return isRecordExists;
        }

        #endregion
    }
}
