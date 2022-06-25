using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.AuditTypeRepository;
using InternalAuditSystem.Repository.Repository.RatingRepository;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore.Storage;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
using InternalAuditSystem.Repository.Exceptions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.AuditPlanModule.Masters.AuditTypeTest
{
    [Collection("Register Dependency")]
    public class AuditTypeRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IAuditTypeRepository _auditTypeRepository;
        private Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private string auditTypeId = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private string auditTypeId2 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";
        private string auditTypeId3 = "a96d7084-011e-446c-ba9a-157499113c72";

        private string enittyId1 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";
        private string enittyId2 = "a96d7084-011e-446c-ba9a-157499113c72";

        #endregion

        #region Constructor
        public AuditTypeRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _auditTypeRepository = bootstrap.ServiceProvider.GetService<IAuditTypeRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set audit types testing data
        /// </summary>
        /// <returns>List of audit types</returns>
        private List<AuditType> SetAuditTypeInitialData()
        {
            List<AuditType> auditTypeObjList = new List<AuditType>()
            {
                new AuditType()
                {
                    Id=new Guid(auditTypeId),
                    Name="Type-1",
                    EntityId=new Guid(enittyId1),                   
                    IsDeleted=false,
                },
                new AuditType()
                {
                    Id=new Guid(auditTypeId2),
                    Name="audit-2",
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },               
                 new AuditType()
                {
                    Id=new Guid(),
                    Name="Type-3",
                    EntityId=new Guid(enittyId2),
                    IsDeleted=false,
                },
                 new AuditType()
                {
                    Id=new Guid(auditTypeId3),
                    Name="yml",
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
            };
            return auditTypeObjList;
        }

        /// <summary>
        /// Set audit plans testing data
        /// </summary>
        /// <returns>List of audit plans</returns>
        private List<AuditPlan> SetAuditPlanInitialData()
        {
            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            List<AuditPlan> auditPlanObjList = new List<AuditPlan>()
            {
                new AuditPlan()
                {
                    Id=new Guid(),
                    SelectedTypeId = auditTypeList[0].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },

                new AuditPlan()
                {
                    Id=new Guid(),
                    SelectedTypeId = auditTypeList[1].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
               
            };
            return auditPlanObjList;
        }
        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllAuditTypesPageWiseAndSearchWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditTypesPageWiseAndSearchWise_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = "";

            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditType, bool>>>()))
                     .Returns(auditTypeList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _auditTypeRepository.GetAllAuditTypesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllAuditTypesPageWiseAndSearchWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditTypesPageWiseAndSearchWise_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid().ToString();

            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditType, bool>>>()))
                     .Returns(auditTypeList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _auditTypeRepository.GetAllAuditTypesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllAuditTypesPageWiseAndSearchWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditTypesPageWiseAndSearchWise_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = enittyId1;

            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditType, bool>>>()))
                     .Returns(auditTypeList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _auditTypeRepository.GetAllAuditTypesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Equal(3, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllAuditTypesPageWiseAndSearchWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditTypesPageWiseAndSearchWise_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = enittyId1;

            List<AuditType> auditTypeList = SetAuditTypeInitialData();
            searchString = "Type";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditType, bool>>>()))
                     .Returns(auditTypeList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.Name.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);

            var result = await _auditTypeRepository.GetAllAuditTypesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Single(result);
        }


        /// <summary>
        /// Test case to verify GetAllAuditTypeByEntityId
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditTypebyEntityId_ReturnAllTypesUnderEntity()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditType, bool>>>()))
                     .Returns(auditTypeList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _auditTypeRepository.GetAllAuditTypeByEntityIdAsync(passedAuditableEntity);

            Assert.Equal(3, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllAuditTypeByEntityId
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditTypebyEntityId_ReturnNoElementUnderEntity()
        {
            var passedAuditableEntity = new Guid();

            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditType, bool>>>()))
                     .Returns(auditTypeList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _auditTypeRepository.GetAllAuditTypeByEntityIdAsync(passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAuditTypeById
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAuditTypeById_ReturnAuditType()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<AuditType> auditTypeList = SetAuditTypeInitialData();
            var auditTypeId = auditTypeList[0].Id;

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<AuditType, bool>>>()))
                     .ReturnsAsync(auditTypeList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id.ToString() == auditTypeId.ToString()));

            var result = await _auditTypeRepository.GetAuditTypeByIdAsync(auditTypeId.ToString(),passedAuditableEntity.ToString());

            Assert.Equal(result.Name, auditTypeList[0].Name);
        }

        /// <summary>
        /// Test case to verify GetAuditTypeById
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAuditTypeById_NoAuditTypeFound_ThrowInvalidOperationException()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<AuditType> auditTypeList = SetAuditTypeInitialData();
            var auditTypeId = new Guid();

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<AuditType, bool>>>()))
                     .Throws<InvalidOperationException>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                                        await _auditTypeRepository.GetAuditTypeByIdAsync(auditTypeId.ToString(), passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify AddAuditType
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddAuditType_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            AuditTypeAC auditTypeAcObj = new AuditTypeAC();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<AuditType, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _auditTypeRepository.AddAuditTypeAsync(auditTypeAcObj, passedAuditableEntity.ToString()));
          
        }

        /// <summary>
        /// Test case to verify AddAuditType
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddAuditType_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            AuditTypeAC auditTypeAcObj = new AuditTypeAC()
            {
                Name = "audit-2"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<AuditType, bool>>>()))
                            .ReturnsAsync(auditTypeList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == auditTypeAcObj.Name.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _auditTypeRepository.AddAuditTypeAsync(auditTypeAcObj, passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify AddAuditType
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddAuditType_AddnewDaat()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            AuditTypeAC auditTypeAcObj = new AuditTypeAC()
            {
                Name = "audit-3"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<AuditType, bool>>>()))
                            .ReturnsAsync(auditTypeList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == auditTypeAcObj.Name.ToLower()));

            var dbData = new AuditType()
            {
                Id = new Guid(),
                Name = auditTypeAcObj.Name

            };

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<AuditType>())).ReturnsAsync(dbData);

            var result = await _auditTypeRepository.AddAuditTypeAsync(auditTypeAcObj, passedAuditableEntity.ToString());

        }

        /// <summary>
        /// Test case to verify UpdateAuditType
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateAuditType_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            AuditTypeAC auditTypeAcObj = new AuditTypeAC()
            {
                Name = "audit-2"
            };

            //mock db calls
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditType, bool>>>()))
                         .Returns(auditTypeList.Where(x => !x.IsDeleted && x.Id == auditTypeList[0].Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<AuditType, bool>>>()))
                            .ReturnsAsync(auditTypeList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == auditTypeAcObj.Name.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _auditTypeRepository.UpdateAuditTypeAsync(auditTypeAcObj, passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify UpdateAuditType
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateAuditType_UpdateData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            AuditTypeAC auditTypeAcObj = new AuditTypeAC()
            {
                Name = "Type new"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<AuditType, bool>>>()))
                            .ReturnsAsync(auditTypeList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == auditTypeAcObj.Name.ToLower()));

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditType, bool>>>()))
                           .Returns(auditTypeList.Where(x => !x.IsDeleted && x.Id == auditTypeList[0].Id).AsQueryable().BuildMock().Object);

            
             await _auditTypeRepository.UpdateAuditTypeAsync(auditTypeAcObj, passedAuditableEntity.ToString());

            _dataRepository.Verify(v => v.Update(It.IsAny<AuditType>()), Times.Once);
            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);


        }


        /// <summary>
        /// Test case to verify DeleteAuditType
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteAuditType_SameEntryAdd_ThrowDeleteLinkedDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            List<AuditPlan> auditPlans = new List<AuditPlan>()
            {
                new AuditPlan()
                {
                    Id=new Guid(),
                    SelectedTypeId = auditTypeList[0].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                new AuditPlan()
                {
                    Id=new Guid(),
                    SelectedTypeId = auditTypeList[0].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                new AuditPlan()
                {
                    Id=new Guid(),
                    SelectedTypeId = auditTypeList[1].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },

            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
                            .ReturnsAsync(auditPlans.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.SelectedTypeId == auditTypeList[0].Id));

            await Assert.ThrowsAsync<DeleteLinkedDataException>(async () =>
            await _auditTypeRepository.DeleteAuditTypeAsync(auditTypeList[0].Id.ToString(), passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify DeleteAuditType
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteAuditType_DeleteSuccessfully()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            List<AuditPlan> auditPlans = SetAuditPlanInitialData();

            var typeId = auditTypeList[3].Id;

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
                            .ReturnsAsync(auditPlans.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.SelectedTypeId == typeId));
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<AuditType, bool>>>()))
                     .ReturnsAsync(auditTypeList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id.ToString() == typeId.ToString()));


            await _auditTypeRepository.DeleteAuditTypeAsync(typeId.ToString(), passedAuditableEntity.ToString());

        }

        // <summary>
        /// Test case to verify GetTotalCountOfAuditTypeSearchStringWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfAuditTypeSearchStringWise_ReturnTotalRecordsCount_WithoutSerachString()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<AuditType, bool>>>()))
                            .ReturnsAsync(auditTypeList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));



            var result = await _auditTypeRepository.GetTotalCountOfAuditTypeSearchStringWiseAsync(null, passedAuditableEntity.ToString());

            Assert.Equal(3, result);

        }

        // <summary>
        /// Test case to verify GetTotalCountOfAuditTypeSearchStringWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfAuditTypeSearchStringWise_ReturnTotalRecordsCount_SerachStringWise()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            List<AuditType> auditTypeList = SetAuditTypeInitialData();
            var searchString = "audit-2";

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<AuditType, bool>>>()))
                            .ReturnsAsync(auditTypeList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower().Contains(searchString)));


            var result = await _auditTypeRepository.GetTotalCountOfAuditTypeSearchStringWiseAsync(searchString, passedAuditableEntity.ToString());

            Assert.Equal(1,result);

        }

        #endregion

    }
}
