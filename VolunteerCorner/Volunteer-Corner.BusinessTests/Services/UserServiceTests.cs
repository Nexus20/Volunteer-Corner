using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Volunteer_Corner.Business.Exceptions;
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Models.Enums;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Services;
using Volunteer_Corner.Data;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.BusinessTests.Services;

[TestFixture]
public class UserServiceTests
{
    private IMapper _mapper;
    private IUserService _userService;
    private ApplicationDbContext _dbContext;
    private Mock<UserManager<User>> _mockedUserManager;
    private Mock<ILogger<UserService>> _mockedLogger;

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
        _mockedLogger = new Mock<ILogger<UserService>>();
        _dbContext = new ApplicationDbContext(UnitTestsHelper.GetUnitTestDbOptions());
        _mockedUserManager = UnitTestsHelper.GetUserManagerMock();

        _userService = new UserService(_mockedUserManager.Object, _mapper, _mockedLogger.Object, _dbContext);
    }
    
    // Please use Triple A convention (Arrange, Act, Assert)
    // Naming convention
    // <MethodName>_<WhenSomeActionOccurs>_<DoSomeExpectedResult>

    [Test]
    public async Task RegisterAsync_WhenAccountTypeIsInvalid_ThrowsValidationException()
    {
        // Arrange
        var request = new RegisterRequest()
        {
            AccountType = 0
        };

        const string expectedMessage = "Invalid account type";

        // Act
        var action = async () =>
        {
            await _userService.RegisterAsync(request);
        };
        
        // Assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage(expectedMessage);
    }
    
    [Test]
    public async Task RegisterAsync_WhenUserWithSuchLoginAlreadyExists_ThrowsValidationException()
    {
        // Arrange
        const string existingLogin = "Some existing login"; 
        
        var request = new RegisterRequest()
        {
            AccountType = AccountType.Volunteer,
            UserName = existingLogin
        };
        
        var expectedMessage = $"User with such login {request.UserName} already exists";

        _mockedUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User());

        // Act
        var action = async () =>
        {
            await _userService.RegisterAsync(request);
        };
        
        // Assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage(expectedMessage);
    }
}