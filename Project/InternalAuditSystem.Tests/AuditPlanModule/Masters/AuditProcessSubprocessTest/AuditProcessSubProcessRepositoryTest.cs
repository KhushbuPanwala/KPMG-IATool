using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.AuditProcessSubProcessRepository;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using System.Linq;
using System.Linq.Expressions;
using MockQueryable.Moq;
using System.Threading.Tasks;

namespace InternalAuditSystem.Test.AuditPlanModule.Masters.AuditProcessSubprocessTest
{
    [Collection("Register Dependency")]
    public class AuditProcessSubProcessRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IAuditProcessSubProcessRepository _auditProcessSubProcessRepository;
        private Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private string processId1 = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private string processId2 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";
        private string subProcessId1 = "a96d7084-011e-446c-ba9a-157499113c72";
        private string subProcessId2 = "a96d7084-011e-446c-ba9a-157499113c72";
        private string enittyId = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";

        #endregion

        #region Constructor
        public AuditProcessSubProcessRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _auditProcessSubProcessRepository = bootstrap.ServiceProvider.GetService<IAuditProcessSubProcessRepository>();
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
        private List<Process> SetProcessSubProcessInitialData()
        {
            List<Process> processObjList = new List<Process>()
            {
                // process -1
                new Process()
                {
                    Id=new Guid(processId1),
                    Name="kpmg process",
                    EntityId=new Guid(enittyId),
                    ParentProcess = null,
                    IsDeleted=false,
                },
                // process -2
                new Process()
                {
                    Id=new Guid(processId2),
                    Name="promact process",
                    EntityId=new Guid(enittyId),
                    ParentProcess = null,
                    IsDeleted=false,
                },
                // sub process - 1
                 new Process()
                {
                     Id=new Guid(subProcessId1),
                    Name="accounts",
                    EntityId=new Guid(enittyId),
                    ParentId = Guid.Parse(processId1),
                    IsDeleted=false,
                },
                 // sub process - 2
                  new Process()
                {
                    Id=new Guid(subProcessId2),
                    Name="wirehouse",
                    EntityId=new Guid(enittyId),
                    ParentId = Guid.Parse(processId2),
                    IsDeleted=false,
                }
            };
            return processObjList;
        }
        #endregion

        #region Testing Methods Completed

        #region Audit Process Test
        /// <summary>
        /// Test case to verify GetAllAuditCategoriesPageWiseAndSearchWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditProcessesPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = string.Empty;

            List<Process> processList = SetProcessSubProcessInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Process, bool>>>()))
                     .Returns(processList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _auditProcessSubProcessRepository.GetAllAuditProcessesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        #endregion
        
        #endregion

    }
}
