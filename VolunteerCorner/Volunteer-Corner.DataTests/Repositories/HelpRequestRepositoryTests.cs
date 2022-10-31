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
    }
}
