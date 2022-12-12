using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Volunteer_Corner.Business.Exceptions;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Enums;
using Volunteer_Corner.Business.Models.Requests.Auth;
using Volunteer_Corner.Business.Models.Requests.Users;
using Volunteer_Corner.Business.Models.Results.Users;
using Volunteer_Corner.Business.Services;
using Volunteer_Corner.BusinessTests.EqualityComparers;
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

    [TearDown]
    public void TearDown()
    {
        // This method is called AFTER EVERY test had been launched
        // Do all stuff that needs to be applied after unit tests will end its work 
        _dbContext.Dispose();
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
    public async Task RegisterAsync_WhenAccountTypeIsInvalid_ThrowsValidationException()
    {
        // Arrange
        var request = new RegisterRequest()
        {
            AccountType = 0
        };

        const string expectedMessage = "Invalid account type";

        // Act
        var action = async () => { await _userService.RegisterAsync(request); };

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
        var action = async () => { await _userService.RegisterAsync(request); };

        // Assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage(expectedMessage);
    }

    [Test]
    public async Task RegisterAsync_WhenUserWithSuchEmailISAlreadyExist_ThrowsValidationException()
    {
        // Arrange

        const string existingEmail = $"Some exciting email";

        var request = new RegisterRequest()
        {
            AccountType = AccountType.HelpSeeker,
            Email = existingEmail
        };

        var message = $"User with such email {request.Email} already exists";

        _mockedUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new User());

        // Act

        var action = async () => { await _userService.RegisterAsync(request); };

        //Assert

        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage(message);
    }

    [Test]
    public async Task RegisterAsync_WhenUserTypeIsNotDefine_ThrowsIdentityException() //NAME OF THE METHOD
    {
        // Arrange

        const string password = "Some user password";
        const string userName = "Some user name";

        var identityResult = IdentityResult.Failed(new IdentityError()
        {
            Code = "123",
            Description = "Error with brain of new customer"
        });

        var request = new RegisterRequest()
        {
            AccountType = AccountType.HelpSeeker,
            UserName = userName,
            Password = password
        };

        _mockedUserManager.Setup(m => m.CreateAsync(
                It.IsAny<User>(),
                It.IsAny<string>()))
            .ReturnsAsync(identityResult);

        // Act

        var action = async () => { await _userService.RegisterAsync(request); };

        // Assert

        await action.Should().ThrowAsync<IdentityException>();
    }

    [Test]
    public async Task RegisterAsync_WhenAddToRolesAsyncFailed_ThrowsIdentityException() //NAME OF THE METHOD
    {
        // Arrange

        const string password = "Some user password";
        const string userName = "Some user name";
        const string userEmail = "Some user Email";


        var identityResult = IdentityResult.Failed(new IdentityError()
        {
            Code = "123",
            Description = "Error with brain of new customer"
        });

        var request = new RegisterRequest()
        {
            AccountType = AccountType.HelpSeeker,
            UserName = userName,
            Password = password,
            Email = userEmail,
        };

        _mockedUserManager.Setup(m => m.CreateAsync(
                It.IsAny<User>(),
                It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _mockedUserManager.Setup(m => m.AddToRolesAsync(
                It.IsAny<User>(),
                It.IsAny<List<string>>()))
            .ReturnsAsync(identityResult);

        // Act

        var action = async () =>
        {
            await _userService.RegisterAsync(request);
        };

        // Assert

        await action.Should().ThrowAsync<IdentityException>();
    }
    
    [Test]
    public async Task RegisterAsync_WhenAllIsRight_ReturnsCorrectResult()
    {
        //Arrange

        const string password = "Some user password";
        const string userName = "Some user name";
        const string userEmail = "Some user Email";

        var request = new RegisterRequest()
        {
            AccountType = AccountType.HelpSeeker,
            UserName = userName,
            Password = password,
            Email = userEmail
        };

        var user = _mapper.Map<RegisterRequest, User>(request);

        var expectedResult = _mapper.Map<User, UserResult>(user);

        _mockedUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(),
                It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _mockedUserManager.Setup(m => m.AddToRolesAsync(
                It.IsAny<User>(),
                It.IsAny<List<string>>()))
            .ReturnsAsync(IdentityResult.Success);
        
        // Act
        var actualResult = await _userService.RegisterAsync(request);

        // Assert
        actualResult.Should()
            .BeEquivalentTo(expectedResult, o => o.Using(new RegisterResultEqualityComparer()));
    }

    [Test]
    public async Task RegisterAsync_WhenAccountTimeSetToHelpSeeker_CreatesHelpSeekerProfile()
    {
        //Arrange

        const string password = "Some user password";
        const string userName = "Some user name";
        const string userEmail = "Some user Email";

        var request = new RegisterRequest()
        {
            AccountType = AccountType.HelpSeeker,
            UserName = userName,
            Password = password,
            Email = userEmail
        };

        var expectedHelpSeekersCount = await _dbContext.HelpSeekers.CountAsync() + 1;
        
        _mockedUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(),
                It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _mockedUserManager.Setup(m => m.AddToRolesAsync(
                It.IsAny<User>(),
                It.IsAny<List<string>>()))
            .ReturnsAsync(IdentityResult.Success);
        
        // Act
        await _userService.RegisterAsync(request);
        var actualHelpSeekersCount = await _dbContext.HelpSeekers.CountAsync();
        
        // Assert
        actualHelpSeekersCount.Should().Be(expectedHelpSeekersCount);
    }
    
    [Test]
    public async Task RegisterAsync_WhenAccountTimeSetToVolunteer_CreatesVolunteerProfile()
    {
        //Arrange

        const string password = "Some user password";
        const string userName = "Some user name";
        const string userEmail = "Some user Email";

        var request = new RegisterRequest()
        {
            AccountType = AccountType.Volunteer,
            UserName = userName,
            Password = password,
            Email = userEmail
        };

        var expectedVolunteersCount = await _dbContext.Volunteers.CountAsync() + 1;
        
        _mockedUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(),
                It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _mockedUserManager.Setup(m => m.AddToRolesAsync(
                It.IsAny<User>(),
                It.IsAny<List<string>>()))
            .ReturnsAsync(IdentityResult.Success);
        
        // Act
        await _userService.RegisterAsync(request);
        var actualVolunteersCount = await _dbContext.Volunteers.CountAsync();
        
        // Assert
        actualVolunteersCount.Should().Be(expectedVolunteersCount);
    }

    [Test]
    public async Task UpdateProfileAsync_WhenUserIdNotExist_NotFoundException()
    {
        //Arrange

        const string userId = "Some existing Id";
        const string firstName = "Some user firstname";
        const string lastName = "Some user lastname";
        const string userEmail = "Some user Email";
        const string phoneNumber = "Some user Email";

        var request = new UpdateOwnProfileRequest()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = userEmail,
            PhoneNumber = phoneNumber
        };

        //User user = new User();
        //user = null;
        User user = null;

        var expectedMessage = $"Entity \"User\" ({userId}) was not found.";

        _mockedUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);


        // Act
        var action = async () => { await _userService.UpdateOwnProfileAsync(userId,request); };

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage(expectedMessage);
    }

    [Test]
    public async Task UpdateProfileAsync_WhenAllIsRight_ReturnsCorrectResult()
    {
        //Arrange

        const string userId = "Some existing Id";
        const string firstName = "Some user firstname";
        const string lastName = "Some user lastname";
        const string userEmail = "Some user Email";
        const string phoneNumber = "Some user Email";


        var request = new UpdateOwnProfileRequest()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = userEmail,
            PhoneNumber = phoneNumber
        };

        var userToUpdate = _mapper.Map<UpdateOwnProfileRequest, User>(request);
        var expectedResult = _mapper.Map<User, UserResult>(userToUpdate);

        _mockedUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(userToUpdate);

        _mockedUserManager.Setup(m => m.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var actualResult = await _userService.UpdateOwnProfileAsync(userId, request);

        // Assert
        actualResult.Should().Be(expectedResult);
    }


    [Test]
    public async Task UpdateProfileAsync_WhenNotUpdate_IdentityException()
    {
        //Arrange

        const string userId = "Some existing Id";
        const string firstName = "Some user firstname";
        const string lastName = "Some user lastname";
        const string userEmail = "Some user Email";
        const string phoneNumber = "Some user Email";

        var request = new UpdateOwnProfileRequest()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = userEmail,
            PhoneNumber = phoneNumber
        };

        var identityResult = IdentityResult.Failed(new IdentityError()
        {
            Code = "123",
            Description = "Error with brain of new customer"
        });

        _mockedUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new User());
        _mockedUserManager.Setup(m => m.UpdateAsync(It.IsAny<User>())).ReturnsAsync(identityResult);


        // Act
        var action = async () => { await _userService.UpdateOwnProfileAsync(userId, request); };

        // Assert
        await action.Should().ThrowAsync<IdentityException>();
    }
}