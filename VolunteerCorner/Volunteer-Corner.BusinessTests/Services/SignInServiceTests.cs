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
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results;
using Volunteer_Corner.Business.Services;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.BusinessTests.Services
{
    [TestFixture]
    public class SignInServiceTests
    {
        private IConfiguration _configuration;
        private IMapper _mapper;
        private ISignInService _signInService;
        private Mock<UserManager<User>> _mockedUserManager;
        private JwtHandler _jwtHandler;

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

        [Test]
        public async Task SignInAsync_WhenAuthenticationIsInvalid_ThrowInvalidMessage()
        {
            // Arrange

            var loginRequest = new LoginRequest()
            {
                UserName = "Some name",
                Password = "Some password"
            };

            var expectedResult = new LoginResult()
            {
                ErrorMessage = "Invalid Authentication"

            };



            _mockedUserManager.Setup(m => m.CheckPasswordAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act

            var action = await _signInService.SignInAsync(loginRequest);

            // Assert

            action.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task SignInAsync_WhenAllIsRight_ThrowsNoException()
        {
            // Arrange

            var loginRequest = new LoginRequest()
            {
                UserName = "SomeName",
                Password = "SomePassword"
            };


            var token =" new JwtSecurityTokenHandler().WriteToken(tokenOptions)";

            var expectedResult = new LoginResult() { IsAuthSuccessful = true, Token = token };

            var user = new User()
            {
                UserName = "SomeName",
                
            };

            _mockedUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _mockedUserManager.Setup(m => m.CheckPasswordAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act

            var action = await _signInService.SignInAsync(loginRequest);

            // Assert

            action.Should().BeEquivalentTo(expectedResult);
        }

    }
}
