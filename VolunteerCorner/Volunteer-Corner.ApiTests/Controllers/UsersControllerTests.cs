using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Volunteer_Corner.API.Controllers;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Requests.Auth;
using Volunteer_Corner.Business.Models.Requests.Users;
using Volunteer_Corner.Business.Models.Results.Auth;
using Volunteer_Corner.Business.Models.Results.Users;

namespace Volunteer_Corner.ApiTests.Controllers;

using NUnit.Framework;

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

    [Test]
    public async Task UpdateProfile_WhenUpdateProfileSuccessfully_ReturnsStatus200()
    {
        // Arrange
        const string firstName = "Some user firstname";
        const string lastName = "Some user lastname";
        const string userEmail = "Some user Email";
        const string phoneNumber = "Some user Email";
        const string id = "some id";

        var expectedResult = new UserResult()
        {
            Id = id,
            UserName = "test",
            Email = "emailTest",
            PhoneNumber = "test",
            FirstName = "FirstNameTest",
            LastName = "Test"

        };


        var request = new UpdateOwnProfileRequest()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = userEmail,
            PhoneNumber = phoneNumber
        };

        _mockedUserService.Setup(m => m.UpdateOwnProfileAsync(It.IsAny<string>(), It.IsAny<UpdateOwnProfileRequest>()))
            .ReturnsAsync(expectedResult);

        // Act
        var actualResult = await _usersController.UpdateProfile(Mock.Of<UpdateOwnProfileRequest>()) as ObjectResult;

        // Assert
        actualResult.Should().NotBeNull();
        actualResult!.Value.Should().BeEquivalentTo(request);
        actualResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

}