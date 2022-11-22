using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Volunteer_Corner.API.Controllers;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results;

namespace Volunteer_Corner.ApiTests.Controllers;

using NUnit.Framework;
using Volunteer_Corner.Business.Interfaces;

[TestFixture]
public class UsersControllerTests
{
    private Mock<IUserService> _mockedUserService = null!;
    private Mock<ISignInService> _mockedSignInService = null!;
    private UsersController _usersController = null!;

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
        _mockedUserService = new Mock<IUserService>();
        _mockedSignInService = new Mock<ISignInService>();
        _usersController = new UsersController(_mockedUserService.Object, _mockedSignInService.Object);
    }

    [Test]
    public async Task Register_WhenUserRegisteredSuccessfully_ReturnsStatus201()
    {
        // Arrange
        var expectedResult = new UserResult()
        {
            Id = "Some registered user id"
        };

        _mockedUserService.Setup(m => m.RegisterAsync(It.IsAny<RegisterRequest>()))
            .ReturnsAsync(expectedResult);

        // Act
        var actualResult = await _usersController.Register(Mock.Of<RegisterRequest>()) as ObjectResult;

        // Assert
        actualResult.Should().NotBeNull();
        actualResult!.Value.Should().BeEquivalentTo(expectedResult);
        actualResult.StatusCode.Should().Be(StatusCodes.Status201Created);
    }

    [Test]
    public async Task Login_WhenUserLoginSuccessfully_ReturnsStatus200()
    {
        // Arrange
        var expectedResult = new LoginResult()
        {
            IsAuthSuccessful = true
        };

        _mockedSignInService.Setup(m => m.SignInAsync(It.IsAny<LoginRequest>()))
            .ReturnsAsync(expectedResult);

        // Act
        var actualResult = await _usersController.Login(Mock.Of<LoginRequest>()) as ObjectResult;

        // Assert
        actualResult.Should().NotBeNull();
        actualResult!.Value.Should().BeEquivalentTo(expectedResult);
        actualResult.StatusCode.Should().NotBe(StatusCodes.Status401Unauthorized);
        actualResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }


    [Test]
    public async Task Login_WhenUserLoginFailed_ReturnsStatus401()
    {
        // Arrange
        var expectedResult = new LoginResult()
        {
            IsAuthSuccessful = false
        };

        _mockedSignInService.Setup(m => m.SignInAsync(It.IsAny<LoginRequest>()))
            .ReturnsAsync(expectedResult);

        // Act
        var actualResult = await _usersController.Login(Mock.Of<LoginRequest>()) as ObjectResult;

        // Assert
        actualResult.Should().NotBeNull();
        actualResult!.Value.Should().BeEquivalentTo(expectedResult);
        actualResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }
}