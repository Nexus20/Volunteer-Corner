using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Volunteer_Corner.Business.Infrastructure;
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results;
using Volunteer_Corner.Business.Services;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Entities.Identity;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.BusinessTests.Services
{
    [TestFixture]
    public class HelpRequestServiceTests
    {
        private IConfiguration _configuration;
        private IMapper _mapper;
        private ISignInService _signInService;
        private Mock<UserManager<User>> _mockedUserManager;
        private JwtHandler _jwtHandler;
        private Mock<IRepository<HelpRequest>> _helpRequestRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // This method is called BEFORE ANY OF THE tests will be launched
            // Do common initializing stuff
            _mapper = UnitTestsHelper.GetMapper();
        }

        [SetUp]
        public void SetUp()
        {
            // This method is called BEFORE EACH OF THE tests will be launched
            // Do initializing stuff that needs to be applied before each test
            _mockedUserManager = UnitTestsHelper.GetUserManagerMock();
            _configuration = UnitTestsHelper.GetConfiguration();
            _jwtHandler = new JwtHandler(_configuration, _mockedUserManager.Object);
            _helpRequestRepository = new Mock<IRepository<HelpRequest>>();
            _signInService = new SignInService(_mockedUserManager.Object, _jwtHandler);
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


        //[Test]
        //public async Task RegisterAsync_WhenAccountTypeIsInvalid_ThrowsValidationException()
        //{
        //    // Arrange
        //    var predicate = CreateFilterPredicate(request);


        //    _mockedUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
        //        .ReturnsAsync(new User());

        //    // Act
        //    var action = async () => { await _userService.RegisterAsync(request); };

        //    // Assert
        //    await action.Should().ThrowAsync<ValidationException>()
        //        .WithMessage(expectedMessage);
        //}
    }
    
}
