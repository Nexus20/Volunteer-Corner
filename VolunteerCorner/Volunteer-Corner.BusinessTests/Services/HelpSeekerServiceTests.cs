using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Volunteer_Corner.Business.Exceptions;
using Volunteer_Corner.Business.Interfaces.Infrastructure;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Requests.HelpRequests;
using Volunteer_Corner.Business.Models.Requests.HelpSeekers;
using Volunteer_Corner.Business.Models.Results.HelpRequests;
using Volunteer_Corner.Business.Models.Results.HelpSeekers;
using Volunteer_Corner.Business.Services;
using Volunteer_Corner.Data;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Enums;
using Volunteer_Corner.Data.Interfaces;
using Volunteer_Corner.Data.Repositories;

namespace Volunteer_Corner.BusinessTests.Services;

[TestFixture]
public class HelpSeekerServiceTests
{
    private IMapper _mapper = null!;
    private Mock<IHelpSeekerRepository> _mockedHelpSeekerRepository = null!;
    private Mock<IHelpRequestRepository> _mockedHelpRequestRepository = null!;
    private Mock<ILogger<HelpSeekerService>> _logger;
    private IHelpSeekerService _helpSeekerService;


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
        _mockedHelpSeekerRepository = new Mock<IHelpSeekerRepository>();
        _mockedHelpRequestRepository = new Mock<IHelpRequestRepository>();
        _logger = new Mock<ILogger<HelpSeekerService>>();
        _helpSeekerService = new HelpSeekerService(_mapper, _mockedHelpSeekerRepository.Object, _logger.Object,
            _mockedHelpRequestRepository.Object);
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


    //TODO: why request is not working
    [Test]
    public async Task GetAllHelpSeekers_WhenSearchingByUserRequest_ReturnsDataAccordingToRequest()
    {
        // Arrange

        var request = new GetAllHelpSeekersRequest
        {
            SearchString = "",

            IsApproved = true
        };

        var source = new List<HelpSeeker>()
        {
            new HelpSeeker()
            {
                Id = "1",
                IsApproved = true
            },
            new HelpSeeker()
            {
                Id = "2",
                IsApproved = false
            }
        };

        _mockedHelpSeekerRepository.Setup(m => m.GetAsync(
            It.IsAny<Expression<Func<HelpSeeker, bool>>?>())).ReturnsAsync(source);

        var expectedResult = _mapper.Map<List<HelpSeeker>, List<HelpSeekerResult>>(source);

        // Act
        var action = await _helpSeekerService.GetAllHelpSeekers(request);

        // Assert
        action.Should().NotBeNull();
        action.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task GetAllHelpSeekers_WhenRequestIsNull_ReturnsDataAccordingToRequest()
    {
        // Arrange

        var request = new GetAllHelpSeekersRequest
        {
            SearchString = ""
        };

        var source = new List<HelpSeeker>()
        {
            new HelpSeeker()
            {
                Id = "1",
                IsApproved = true
            },
            new HelpSeeker()
            {
                Id = "2",
                IsApproved = false
            }
        };

        _mockedHelpSeekerRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(source);

        var expectedResult = _mapper.Map<List<HelpSeeker>, List<HelpSeekerResult>>(source);

        // Act
        var action = await _helpSeekerService.GetAllHelpSeekers(request);

        // Assert
        action.Should().NotBeNull();
        action.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task GetHelpSeekerById_WhenUserInNotFound_ThrowNotFoundException()
    {
        // Arrange

        const string userId = "1";

        HelpSeeker helpSeeker = null;

        _mockedHelpSeekerRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpSeeker);

        var expectedResult = new NotFoundException(nameof(HelpSeeker), userId).ToString();

        // Act
        var action = async () => { return await _helpSeekerService.GetHelpSeekerById(userId); };

        // Assert
        await action.Should().ThrowAsync<NotFoundException>().WithMessage(expectedResult);
    }

    [Test]
    public async Task GetHelpSeekerById_WhenUserIsFounded_ReturnsResponseAccordingToRequest()
    {
        // Arrange

        const string userId = "1";

        HelpSeeker helpSeeker = new HelpSeeker()
        {
            Id = "1",
            UserId = "2"
        };

        _mockedHelpSeekerRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpSeeker);

        var expectedResult = _mapper.Map<HelpSeeker, HelpSeekerResult>(helpSeeker);

        // Act
        var action = await _helpSeekerService.GetHelpSeekerById(userId);

        // Assert
        action.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task ChangeApprovalStatus_WhenUserInNotFound_ThrowNotFoundException()
    {
        // Arrange

        const string userId = "1";

        HelpSeeker helpSeeker = null;

        _mockedHelpSeekerRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpSeeker);

        var expectedResult = new NotFoundException(nameof(HelpSeeker), userId).ToString();

        // Act
        var action = async () => { await _helpSeekerService.ChangeApprovalStatus(userId); };

        // Assert
        await action.Should().ThrowAsync<NotFoundException>().WithMessage(expectedResult);
    }


    [Test]
    public async Task ChangeApprovalStatus_WhenUserIsFounded_ChangesOnOppositeStatus()
    {
        // Arrange

        const string userId = "1";

        HelpSeeker helpSeeker = new HelpSeeker()
        {
            Id = "1",
            UserId = "2",
            IsApproved = false
        };

        _mockedHelpSeekerRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpSeeker);

        var expectedResult = !helpSeeker.IsApproved;

        // Act
        var action = await _helpSeekerService.ChangeApprovalStatus(userId);

        // Assert
        action.Should().Be(expectedResult);
    }


    [Test]
    public async Task GetOwnHelpRequestsAsync_WhenUserIsFounded_ChangesOnOppositeStatus()
    {
        // Arrange

        const string userId = "1";

        var source = new List<HelpRequest>()
        {
            new HelpRequest()
            {
                Id = "1",
                OwnerId = "1",
                Name = "Name",
                Description = "Description"
            },
            new HelpRequest()
            {
                Id = "2",
                OwnerId = "2",
                Name = "Name",
                Description = "Description"
            }

        };

        _mockedHelpRequestRepository.Setup(m => m.GetAsync(It.IsAny<Expression<Func<HelpRequest, bool>>?>(),
            It.IsAny<Func<IQueryable<HelpRequest>, IOrderedQueryable<HelpRequest>>>(),
            It.IsAny<bool>())).ReturnsAsync(source);

        var expectedResult = _mapper.Map<List<HelpRequest>, List<HelpRequestResult>>(source);

        // Act
        var action = await _helpSeekerService.GetOwnHelpRequestsAsync(userId);

        // Assert
        action.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task GetOwnHelpRequestByIdAsync_WhenHelprequestIsNotFounded_ThrowNotFoundException()
    {
        // Arrange

        const string userId = "1";

        const string requestId = "2";

        HelpRequest helpRequest = null;

        _mockedHelpRequestRepository.Setup(m => m.GetByIdWithResponsesAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);

        var expectedResult = $"Entity \"helpRequest\" ({requestId}) was not found.";

        // Act
        var action = async () => { await _helpSeekerService.GetOwnHelpRequestByIdAsync(userId, requestId); };

        // Assert
        await action.Should().ThrowAsync<NotFoundException>().WithMessage(expectedResult);
    }

    [Test]
    public async Task GetOwnHelpRequestByIdAsync_WhenHelprequestOwnerIsNotUser_ThrowValidationException()
    {
        // Arrange

        const string userId = "1";

        const string requestId = "2";

        HelpRequest helpRequest = new HelpRequest()
        {
            Id = "2",
            OwnerId = "100",
            Name = "Name"
        };

        _mockedHelpRequestRepository.Setup(m => m.GetByIdWithResponsesAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);

        var expectedResult = "Owner id and help seeker id don't match";

        // Act
        var action = async () => { await _helpSeekerService.GetOwnHelpRequestByIdAsync(userId, requestId); };

        // Assert
        await action.Should().ThrowAsync<ValidationException>().WithMessage(expectedResult);
    }

    //TODO: mb ne yspeu
    [Test]
    public async Task GetOwnHelpRequestByIdAsync_WhenRequestIsFounded_ThrowValidationException()
    {
        // Arrange

        const string userId = "100";

        const string requestId = "2";

        HelpRequest helpRequest = new HelpRequest()
        {
            Id = "2",
            OwnerId = "100",
            Name = "Name"
        };

        _mockedHelpRequestRepository.Setup(m => m.GetByIdWithResponsesAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);

        var expectedResult = _mapper.Map<HelpRequest, HelpRequestWithHelpResponsesResult>(helpRequest);

        // Act
        var action =  await _helpSeekerService.GetOwnHelpRequestByIdAsync(userId, requestId);

        // Assert
        action.Should().BeEquivalentTo(expectedResult);
    }
}