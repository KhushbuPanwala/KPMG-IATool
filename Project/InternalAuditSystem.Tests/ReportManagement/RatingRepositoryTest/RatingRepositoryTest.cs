using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Repository.Repository.RatingRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;


namespace InternalAuditSystem.Test.ReportManagement.RatingRepositoryTest
{
    [Collection("Register Dependency")]

    public class RatingRepositoryTest : BaseTest
    {

        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IRatingRepository _ratingRepository;
        private IExportToExcelRepository _exportToExcelRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private readonly string searchString = null;
        private readonly string ratingId = "fa25b374-e348-4364-9dce-bb0ff40c08b8";
        private readonly string entityId = "7d5875d9-ab95-46b5-b961-ec92f1376bf0";
        #endregion

        #region Constructor
        public RatingRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _ratingRepository = bootstrap.ServiceProvider.GetService<IRatingRepository>();
            _exportToExcelRepository = bootstrap.ServiceProvider.GetService<IExportToExcelRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set Ratings testing data
        /// </summary>
        /// <returns>List of Ratings data</returns>
        private List<Rating> SetRatingsInitData()
        {
            List<Rating> ratingMappingObj = new List<Rating>()
            {
                new Rating()
                {
                    Id=new Guid(ratingId),
                    Ratings="test",
                    EntityId=new Guid(),
                    QualitativeFactors="test",
                    QuantitativeFactors="test",
                    Score=1,
                    Legend="red",
                    IsDeleted=false,
                }
            };
            return ratingMappingObj;
        }

        /// <summary>
        /// Set RatingAC testing data
        /// </summary>
        /// <returns>List of RatingAC data</returns>
        private List<RatingAC> SetRatingsACInitData()
        {
            List<RatingAC> ratingACMappingObj = new List<RatingAC>()
            {
                new RatingAC()
                {
                    Id= new Guid(),
                    Ratings="test",
                    EntityId=new Guid(),
                    QualitativeFactors="test",
                    QuantitativeFactors="test",
                    Score=1,
                    Legend="red",
                }
            };
            return ratingACMappingObj;
        }

        /// <summary>
        /// Set User testing data
        /// </summary>
        /// <returns>List of Users data</returns>
        private List<User> SetUserInitData()
        {
            List<User> userMappingObj = new List<User>()
            {
                new User()
                {
                    Id=new Guid(ratingId),
                    IsDeleted=false,
                }
            };
            return userMappingObj;
        }

        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetRatingsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRatingsAsync_ReturnRatingAC()
        {
            List<Rating> ratingMappingObj = SetRatingsInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Rating, bool>>>()))
                     .Returns(ratingMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _ratingRepository.GetRatingsAsync(page, pageSize, searchString, entityId);
            Assert.Equal(ratingMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetRatingsAsync search string
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRatingsAsync_SearchString()
        {
            List<Rating> ratingMappingObj = SetRatingsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Rating, bool>>>()))
                     .Returns(ratingMappingObj.Where(x => !x.IsDeleted && x.Ratings == "test").AsQueryable().BuildMock().Object);

            var result = await _ratingRepository.GetRatingsAsync(page, pageSize, "test", entityId);
            Assert.Equal(ratingMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetRatingsAsync search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRatingsAsync_SearchRatingAC()
        {
            List<Rating> ratingMappingObj = SetRatingsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Rating, bool>>>()))
                     .Returns(ratingMappingObj.Where(x => !x.IsDeleted && x.Ratings == "test").AsQueryable().BuildMock().Object);

            List<RatingAC> result = await _ratingRepository.GetRatingsAsync(page, pageSize, "test", entityId);
            Assert.Equal(ratingMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetRatingsAsync for invalid search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRatingsAsync_InvalidSearchRating()
        {
            List<Rating> ratingMappingObj = SetRatingsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Rating, bool>>>()))
                     .Returns(ratingMappingObj.Where(x => !x.IsDeleted && x.Ratings == "rating").AsQueryable().BuildMock().Object);

            List<RatingAC> result = await _ratingRepository.GetRatingsAsync(page, pageSize, "rating", entityId);
            Assert.NotEqual(ratingMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetRatingCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRatingCountAsync_ReturnCount()
        {
            List<Rating> ratingMappingObj = SetRatingsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Rating, bool>>>()))
                .Returns(ratingMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            int result = await _ratingRepository.GetRatingCountAsync(searchString, entityId);
            Assert.Equal(ratingMappingObj.Count, result);

        }
        /// <summary>
        /// Test case to verify GetRatingCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRatingCountAsync_SearchReturnCount()
        {
            List<Rating> ratingMappingObj = SetRatingsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Rating, bool>>>()))
                .Returns(ratingMappingObj.Where(x => !x.IsDeleted && x.Ratings == "test").AsQueryable().BuildMock().Object);

            int result = await _ratingRepository.GetRatingCountAsync("test", entityId);
            Assert.Equal(ratingMappingObj.Count, result);

        }
        /// <summary>
        /// Test case to verify GetRatingCountAsync with no data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRatingCountAsync_NoReturnCount()
        {
            List<Rating> ratingMappingObj = SetRatingsInitData();
            ratingMappingObj[0].IsDeleted = true;
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Rating, bool>>>()))
                .Returns(ratingMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            int result = await _ratingRepository.GetRatingCountAsync(searchString, entityId);
            Assert.NotEqual(ratingMappingObj.Count, result);

        }

        /// <summary>
        /// Test case to verify GetRatingCountAsync with no data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRatingsByIdAsync_ValidData()
        {
            List<Rating> ratingMappingObj = SetRatingsInitData();
            List<RatingAC> ratingACMappingObj = SetRatingsACInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Rating, bool>>>()))
                  .Returns(() => Task.FromResult(ratingMappingObj[0]));
            RatingAC result = await _ratingRepository.GetRatingsByIdAsync(ratingMappingObj[0].Id.ToString());
            Assert.Equal(ratingACMappingObj[0].Ratings, result.Ratings);

        }

        /// <summary>
        /// Test case to verify Add Rating for valid data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRating_ValidData()
        {
            List<RatingAC> ratingACMappingObj = SetRatingsACInitData();

            List<Rating> ratingMappingObj = SetRatingsInitData();
            string entityId = ratingMappingObj[0].EntityId.ToString();


            List<User> userMappingObj = SetUserInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(() => Task.FromResult(userMappingObj[0]));

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Rating, bool>>>()))
                                .Returns(() => Task.FromResult(ratingMappingObj[0]));

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.AddAsync(ratingACMappingObj[0]))
                    .Returns((RatingAC model) => Task.FromResult((RatingAC)null));

            RatingAC result = await _ratingRepository.AddRatingAsync(ratingACMappingObj[0]);
            _dataRepository.Verify(x => x.AddAsync(It.IsAny<Rating>()), Times.Once);
            Assert.Equal(ratingACMappingObj[0].Ratings, result.Ratings);
        }

        /// <summary>
        /// Test case to verify Add Rating for invalid data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRating_InvalidData()
        {
            List<RatingAC> ratingACMappingObj = SetRatingsACInitData();
            ratingACMappingObj[0] = null;

            List<Rating> ratingMappingObj = SetRatingsInitData();
            ratingMappingObj = null;

            List<User> userMappingObj = SetUserInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Rating, bool>>>()))
                .Returns(() => Task.FromResult(ratingMappingObj[0]));

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<RatingAC>()))
                .Throws(new NullReferenceException());

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
               await _ratingRepository.AddRatingAsync(ratingACMappingObj[0]));
        }

        /// <summary>
        /// Test case to verify Update Rating for valid data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRating_ValidData()
        {
            List<RatingAC> ratingACMappingObj = SetRatingsACInitData();
            List<Rating> ratingMappingObj = SetRatingsInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Rating, bool>>>()))
                .Returns(() => Task.FromResult(ratingMappingObj[0]));

            List<User> userMappingObj = SetUserInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
               .Returns(() => Task.FromResult(userMappingObj[0]));

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.Update(It.IsAny<RatingAC>()))
            .Returns((RatingAC model) => (EntityEntry<RatingAC>)null);

            RatingAC result = await _ratingRepository.UpdateRatingAsync(ratingACMappingObj[0]);
            _dataRepository.Verify(x => x.Update(It.IsAny<Rating>()), Times.Once);

            Assert.Equal(ratingACMappingObj[0].Ratings, result.Ratings);
        }

        /// <summary>
        /// Test case to verify Update Rating withinvalid data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRating_InvalidData()
        {
            List<RatingAC> ratingACMappingObj = SetRatingsACInitData();
            ratingACMappingObj[0] = null;

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.Update(It.IsAny<RatingAC>()))
               .Throws(new NullReferenceException());

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                    await _ratingRepository.UpdateRatingAsync(ratingACMappingObj[0]));
        }

        /// <summary>
        /// Test case to verify Delete Rating for valid rating id
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRating_ValidRatingId()
        {
            List<Rating> ratingMappingObj = SetRatingsInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            List<User> userMappingObj = SetUserInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Rating, bool>>>()))
                    .Returns(() => Task.FromResult(ratingMappingObj[0]));

            await _ratingRepository.DeleteRatingAsync(ratingId);
            _dataRepository.Verify(x => x.SaveChangesAsync(), Times.Once);

        }

        /// <summary>
        /// Test case to verify Delete Rating for invalid rating id
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRating_InvalidRatingId()
        {
            List<Rating> ratingMappingObj = null;

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);


            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Rating, bool>>>()))
                   .Returns(() => Task.FromResult(ratingMappingObj[0]));

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                    await _ratingRepository.DeleteRatingAsync(ratingId));

        }
        #endregion

    }
}
