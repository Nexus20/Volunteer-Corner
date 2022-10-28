//using FluentAssertions;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using NUnit.Framework;
//using Volunteer_Corner.API.Controllers;
//using Volunteer_Corner.Business.Interfaces;
//using Volunteer_Corner.Business.Models.Requests;
//using Volunteer_Corner.Business.Models.Results;

//namespace Volunteer_Corner.ApiTests.Controllers;

//[TestFixture]
//public class UsersControllerTests
//{
//    private Mock<IUserService> _mockedUserService = null!;
//    private UsersController _usersController = null!;
    
//    [OneTimeSetUp]
//    public void OneTimeSetUp()
//    {
//        // This method is called BEFORE ANY OF THE tests will be launched
//        // Do common initializing stuff
//    }

//    [SetUp]
//    public void SetUp()
//    {
//        // This method is called BEFORE EACH OF THE tests will be launched
//        // Do initializing stuff that needs to be applied before each test
//        _mockedUserService = new Mock<IUserService>();
//        _usersController = new UsersController(_mockedUserService.Object);
//    }

//    [Test]
//    public async Task Register_WhenUserRegisteredSuccessfully_ReturnsStatus201()
//    {
//        // Arrange
//        var expectedResult = new RegisterResult()
//        {
//            Id = "Some registered user id"
//        };

//        _mockedUserService.Setup(m => m.RegisterAsync(It.IsAny<RegisterRequest>()))
//            .ReturnsAsync(expectedResult);
        
//        // Act
//        var actualResult = await _usersController.Register(Mock.Of<RegisterRequest>()) as ObjectResult;

//        // Assert
//        actualResult.Should().NotBeNull();
//        actualResult!.Value.Should().BeEquivalentTo(expectedResult);
//        actualResult.StatusCode.Should().Be(StatusCodes.Status201Created);
//    }
//}