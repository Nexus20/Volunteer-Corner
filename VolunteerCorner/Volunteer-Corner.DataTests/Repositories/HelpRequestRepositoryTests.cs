using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Volunteer_Corner.Data;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Enums;
using Volunteer_Corner.Data.Interfaces;
using Volunteer_Corner.Data.Repositories;

namespace Volunteer_Corner.DataTests.Repositories
{
    [TestFixture]
    public class HelpRequestRepositoryTests
    {
        private ApplicationDbContext _dbContext = null!;
        private IRepository<HelpRequest> _repository = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // This method is called BEFORE ANY OF THE tests will be launched
            // Do common initializing stuff
        }

        [SetUp]
        public void SetUp()
        {
            // This method is called BEFORE EACH OF THE tests will be launched
            // Do initializing stuff that needs to be applied before each test
            _dbContext = new ApplicationDbContext(UnitTestsHelper.GetUnitTestDbOptions());
            _repository = new Repository<HelpRequest>(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            // This method is called AFTER EVERY test had been launched
            // Do all stuff that needs to be applied after unit tests will end its work 
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // This method is called AFTER ANY OF THE tests had been launched
            // Do all stuff that needs to be applied after unit tests will end its work 
        }

        // Please use Triple A convention (Arrange, Act, Assert)
        // Naming convention
        // <MethodName>_<WhenSomeActionOccurs>_<DoSomeExpectedResult>

        [Test]
        public async Task GetAllAsync_Always_ReturnsData()
        {
            // Arrange
            var expectedResult = await _dbContext.HelpRequests.ToListAsync();

            // Act
            var actualResult = await _repository.GetAllAsync();

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
        
        [Test]
        public async Task GetAsync_WhenFilterIsApplied_ReturnsDataAccordingToFilter()
        {
            // Arrange
            Expression<Func<HelpRequest, bool>> predicate = x => x.Status == HelpRequestStatus.Active 
                                                                 && x.Name.Contains("request 1");

            var expectedResult = await _dbContext.HelpRequests
                .Where(predicate)
                .ToListAsync();

            // Act
            var actualResult = await _repository.GetAsync(predicate);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GetAsync_WhenFilterAndIncludesAreApplied_ReturnDataAccordingToFilters()
        {
            // Arrange
            Expression<Func<HelpRequest, bool>> predicate = x => x.Status == HelpRequestStatus.Active
                                                                 && (x.Name.Contains("request 1"));

            var includeString = new List<Expression<Func<HelpRequest, object>>>()
            {
                x => x.Owner
            };

            var query =  _dbContext.HelpRequests
                .Where(predicate);

            foreach (var include in includeString)
                query = query.Include(include);

            var expectedResult = query.AsNoTracking().ToList();

            // Act
            var actualResult = await _repository.GetAsync(predicate, includes: includeString);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult, o => o.IgnoringCyclicReferences());
        }

        [Test]
        public async Task GetAsync_WhenFilterAndSequenceAreApplied_ReturnDataAccordingToFilters()
        {
            // Arrange
            Expression<Func<HelpRequest, bool>> predicate = x => x.Status == HelpRequestStatus.Active
                                                                 && (x.Name.Contains("request 1") || x.Name.Contains("request 3"));
            Func<IQueryable<HelpRequest>, IOrderedQueryable<HelpRequest>> orderBy = x => x.OrderByDescending(x => x.OwnerId);

            var includeString = "Owner";

            var expectedResult = await _dbContext.HelpRequests
                .Where(predicate)
                .Include(includeString)
                .AsNoTracking()
                .OrderByDescending( x => x.OwnerId)
                .ToListAsync();

            // Act
            var actualResult = await _repository.GetAsync(predicate, orderBy, includeString);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult, o => o.IgnoringCyclicReferences());
        }

        [Test]
        public async Task GetById_WhenIdAreApplied_ReturnDataAccordingToId()
        {
            // Arrange

            var id = "1";

            var expectedResult = await _dbContext.HelpRequests
                .FirstOrDefaultAsync(i => i.Id == id); ;

            // Act
            var actualResult = await _repository.GetByIdAsync(id);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task AddAsync_WhenOwnerAddNewHelpRequest_NumberOfRequestsIncreasesByOne()
        {
            // Arrange
            var newHelpRequest = new HelpRequest()
            {
                Id = "4",
                Name = "Help request 4 Name",
                Description = "Help request 4 Description",
                Status = HelpRequestStatus.Active,
                OwnerId = "3"
            };

            var expectedCount = await _dbContext.HelpRequests.CountAsync() + 1;

            // Act
            var actualResult = await _repository.AddAsync(newHelpRequest);
            var actualCount = await _dbContext.HelpRequests.CountAsync();

            // Assert
            actualCount.Should().Be(expectedCount);
            newHelpRequest.Should().BeEquivalentTo(actualResult);
        }

        [Test]
        public async Task DeleteAsync_WhenOwnerDeleteHelpRequest_NumberOfRequestsDecreasesByOne()
        {
            // Arrange

            var helpRequest = new HelpRequest()
            {
                Id = "3"
            };

            var expectedCount = await _dbContext.HelpRequests.CountAsync() - 1;

            // Act
            await _repository.DeleteAsync(helpRequest);
            var actualCount = await _dbContext.HelpRequests.CountAsync();

            // Assert
            actualCount.Should().Be(expectedCount);
        }

        [Test]
        public async Task UpdateAsync_WhenOwnerUpdateHelpRequest_RequestHasModifiedAttributeAndNewData()
        {
            // Arrange

            var HelpRequest = new HelpRequest()
            {
                Id = "3",
                Name = "NEW NAME 3",
                Description = "NEW DESCRIPTION 3",
                Status = HelpRequestStatus.Closed,
                OwnerId = "3",
            };

            _dbContext.Entry(HelpRequest).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            Expression<Func<HelpRequest, bool>> predicate = x =>
               x.Name.Contains("request 3");

            var expectedResult = _dbContext.HelpRequests.Where(predicate).ToListAsync();

            // Act
            _repository.UpdateAsync(HelpRequest);
            var actualResult = _repository.GetAsync(predicate);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
