using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using Moq;
using Volunteer_Corner.Data;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Entities.Identity;
using Volunteer_Corner.Data.Enums;
using Volunteer_Corner.Data.Interfaces;
using Volunteer_Corner.Data.Repositories;

namespace Volunteer_Corner.DataTests.Repositories
{
    [TestFixture]
    public class RepositoryTests
    {
        private ApplicationDbContext _dbContext;

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
            _dbContext = new ApplicationDbContext(MockRepository.GetUnitTestDbOptions());
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
        public async Task GetAllAsync_WhenAllIsRight_ThrowsNoException()
        {
            // Arrange

            var Repository = MockRepository.GetMockOfHelpProposals_ForGetAllAsyncMethod();

            var conreoller = new Repository<HelpProposal>(_dbContext);

            //// Act

            var action = async () => { await conreoller.GetAllAsync(); };

            //// Assert

            await action.Should().NotThrowAsync();

        }
    }

   


    static class MockRepository
    {
        public static DbContextOptions<ApplicationDbContext> GetUnitTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new ApplicationDbContext(options);

            return options;
        }

        public static Mock<IRepository<HelpProposal>> GetMockOfHelpProposals_ForGetAllAsyncMethod()
        {
            var mock = new Mock<IRepository<HelpProposal>>();

            var helpProposals = new List<HelpProposal>()
            {
                new HelpProposal()
                {
                    OwnerId = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e").ToString(),

                    Owner = null,

                    Name = null,
                    Description = null,

                    Status = HelpProposalStatus.Active,

                    AdditionalPhotos = new List<HelpProposalPhoto>()
                    {
                        new HelpProposalPhoto()
                        {
                            HelpProposalId = new Guid().ToString(),

                            FilePath = "l/o/l"
                        }
                    }

                }
            };

            // setup the mock

            mock.Setup(m => m.GetAllAsync()).ReturnsAsync(helpProposals);

            return mock;
        }


        //public static Mock<IRepository<TEntity>> GetMock()
        //{

        //    //mock.Setup(m => m.GetAsync(It.IsAny<Expression<Func<TEntity, bool>>>()))
        //    //    .Returns(() => new Mock<Task<List<TEntity>>>().Object);

        //    //mock.Setup(m => m.GetAsync(It.IsAny<Expression<Func<TEntity, bool>>>(),
        //    //        It.IsAny<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>(),
        //    //        It.IsAny<string?>(),
        //    //        It.IsAny<bool>()))
        //    //    .Returns(() => new Mock<Task<List<TEntity>>>().Object);

        //    //mock.Setup(m => m.GetAsync(It.IsAny<Expression<Func<TEntity, bool>>>(),
        //    //        It.IsAny<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>(),
        //    //        It.IsAny<List<Expression<Func<TEntity,object>>>?>(),
        //    //        It.IsAny<bool>()))
        //    //    .Returns(() => new Mock<Task<List<TEntity>>>().Object);

        //    //mock.Setup(m => m.GetByIdAsync(It.IsAny<string>())).Returns((() => new Mock<Task<TEntity>>()));

        //    //mock.Setup(m => m.AddAsync(It.IsAny<TEntity>())).Returns((() => new Mock<Task<TEntity>>()));

        //    //mock.Setup(m => m.UpdateAsync(It.IsAny<TEntity>())).Returns((() => new Mock<Task>()));

        //    //mock.Setup(m => m.DeleteAsync(It.IsAny<TEntity>())).Returns((() => new Mock<Task>()));

        //    return mock;
        //}
}
}
