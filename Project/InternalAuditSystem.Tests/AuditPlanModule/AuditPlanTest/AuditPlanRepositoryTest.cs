using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.AuditPlanRepository;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;
using InternalAuditSystem.DomailModel.Enums;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.AuditPlanModule.AuditPlanTest
{
    [Collection("Register Dependency")]
    public class AuditPlanRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IAuditPlanRepository _auditPlanRepository;
        private Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private string planId1 = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private string enittyId2 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";
        private string enittyId1 = "a96d7084-011e-446c-ba9a-157499113c72";
        private string auditTypeId = "56be48af-bf4e-48a6-9a18-be81b31b04c8";
        #endregion

        #region Constructor
        public AuditPlanRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _auditPlanRepository = bootstrap.ServiceProvider.GetService<IAuditPlanRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();
        }
        #endregion


        #region Private Methods

        /// <summary>
        /// Set audit plan testing data
        /// </summary>
        /// <returns>List of audit plan</returns>
        private List<AuditPlan> SetAuditPlanInitialData()
        {
            List<AuditPlan> auditPlanObjList = new List<AuditPlan>()
            {
                new AuditPlan()
                {
                    Id=new Guid(planId1),
                    Title="audit-Plan-1",
                    Status= AuditPlanStatus.Active,
                    CreatedDateTime= DateTime.UtcNow,
                    TotalBudgetedHours=0,
                    Version =1.0,
                    SelectedTypeId =new Guid(auditTypeId),
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                new AuditPlan()
                {
                    Id=new Guid(),
                    Title="audit-Plan-2",
                    Status= AuditPlanStatus.Active,
                    CreatedDateTime= DateTime.UtcNow,
                    SelectedTypeId =Guid.NewGuid(),
                    Version =1.0,
                    TotalBudgetedHours=0,
                    EntityId = new Guid(enittyId1),
                    IsDeleted=false,
                },
                 new AuditPlan()
                {
                    Id=new Guid(),
                    Title="audit-Plan-3",
                    Version =1.0,
                    Status= AuditPlanStatus.Active,
                    CreatedDateTime= DateTime.UtcNow,
                    SelectedTypeId =Guid.NewGuid(),
                    TotalBudgetedHours=0,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                }
            };
            return auditPlanObjList;
        }

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
                    Id=new Guid(),
                    Name="type-2",
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
            };
            return auditTypeObjList;
        }
        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllAuditPlansPageWiseAndSearchWiseAsync AuditableEntitityNull__ReturnNoElement
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditPlansPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = string.Empty;

            List<AuditPlan> auditPlanList = SetAuditPlanInitialData();
            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
                     .Returns(auditPlanList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditType, bool>>>()))
                    .Returns(auditTypeList.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(passedAuditableEntity)).AsQueryable().BuildMock().Object);

            var result = await _auditPlanRepository.GetAllAuditPlansPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);
            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllAuditPlansPageWiseAndSearchWiseAsync_
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditPlansPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid().ToString();

            List<AuditPlan> auditPlanList = SetAuditPlanInitialData();
            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
                     .Returns(auditPlanList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditType, bool>>>()))
                    .Returns(auditTypeList.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(passedAuditableEntity)).AsQueryable().BuildMock().Object);

            var result = await _auditPlanRepository.GetAllAuditPlansPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllAuditPlansPageWiseAndSearchWiseAsync_SearchStringNull_ReturnsElementForAuditableEntity
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditPlansPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = enittyId1;

            List<AuditPlan> auditPlanList = SetAuditPlanInitialData();
            List<AuditType> auditTypeList = SetAuditTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
                     .Returns(auditPlanList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditType, bool>>>()))
                    .Returns(auditTypeList.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(passedAuditableEntity)).AsQueryable().BuildMock().Object);

            var result = await _auditPlanRepository.GetAllAuditPlansPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);
            Assert.Equal(3, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllAuditPlansPageWiseAndSearchWiseAsync_SearchStringNotNull_ReturnsElementForAuditableEntity
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditPlansPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = enittyId1;

            List<AuditPlan> auditPlanList = SetAuditPlanInitialData();
            List<AuditType> auditTypeList = SetAuditTypeInitialData();
            searchString = "audit-Plan";

             _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
                     .Returns(auditPlanList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.Title.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditType, bool>>>()))
                    .Returns(auditTypeList.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(passedAuditableEntity)).AsQueryable().BuildMock().Object);

            var result = await _auditPlanRepository.GetAllAuditPlansPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Equal(3,result.Count);
        }

        /// <summary>
        /// Test case to verify GetTotalCountOfAuditTypeSearchStringWise _ReturnTotalRecordsCount_WithoutSerachString
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfAuditPlansAsync_ReturnTotalRecordsCount_WithoutSerachString()
        {
            //create all mock objects for testing
            var passedAuditableEntity = enittyId1;
            List<AuditPlan> auditPlanList = SetAuditPlanInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
                            .ReturnsAsync(auditPlanList.Count(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity));



            var result = await _auditPlanRepository.GetTotalCountOfAuditPlansAsync(null, passedAuditableEntity);

            Assert.Equal(3, result);

        }

        // <summary>
        /// Test case to verify GetTotalCountOfAuditPlansAsync_ReturnTotalRecordsCount_SerachStringWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfAuditPlansAsync_ReturnTotalRecordsCount_SerachStringWise()
        {
            //create all mock objects for testing
            var passedAuditableEntity = enittyId1;
            List<AuditPlan> auditPlanList = SetAuditPlanInitialData();
            var searchString = "audit-Plan-1";

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
                            .ReturnsAsync(auditPlanList.Count(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.Title.ToLower().Contains(searchString.ToLower())));


            var result = await _auditPlanRepository.GetTotalCountOfAuditPlansAsync(searchString, passedAuditableEntity);

            Assert.Equal(1, result);

        }
        #endregion

    }
}
