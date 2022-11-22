using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Volunteer_Corner.Business.Exceptions;
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results.HelpRequests;
using Volunteer_Corner.Business.Services;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Enums;
using Volunteer_Corner.Data.Interfaces;
using Volunteer_Corner.Data.Repositories;

namespace Volunteer_Corner.BusinessTests.Services;

[TestFixture]
public class HelpRequestServiceTests
{
    private IMapper _mapper = null!;
    private Mock<IHelpRequestRepository> _helpRequestRepository = null!;
    private Mock<IRepository<HelpSeeker>> _helpSeekerRepository = null!;
    private Mock<IFormFileCollection> _formFileCollection = null!;
    private IHelpRequestService _helpRequestService = null!;


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
        new Mock<ILogger<HelpRequestRepository>>();
        _helpSeekerRepository = new Mock<IRepository<HelpSeeker>>();
        _helpRequestRepository = new Mock<IHelpRequestRepository>();
        _formFileCollection = new Mock<IFormFileCollection>();
        _helpRequestService =
            new HelpRequestService(_helpRequestRepository.Object, _mapper, _helpSeekerRepository.Object, new Mock<ILogger<HelpRequestService>>().Object);
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
    public async Task GetAllHelpRequests_WhenSearchingByUserRequest_ReturnsDataAccordingToRequest()
    {
        // Arrange

        var request = new GetAllHelpRequestsRequest
        {
            SearchString = "",
            Status = HelpRequestStatus.Active,
            StartDate = null,
            EndDate = null
        };

        var source = new List<HelpRequest>()
        {
            new HelpRequest()
            {
                Id = "1",
                OwnerId = "1",
                Name = "Name 2",
                Description = "Description 2",
                Status = HelpRequestStatus.Active
            },
            new HelpRequest()
            {
                Id = "2",
                OwnerId = "2",
                Name = "Name 2",
                Description = "Description 2",
                Status = HelpRequestStatus.Closed
            }
        };

        _helpRequestRepository.Setup(m => m.GetAsync(
            It.IsAny<Expression<Func<HelpRequest, bool>>?>(),
            It.IsAny<Func<IQueryable<HelpRequest>, IOrderedQueryable<HelpRequest>>?>(),
            It.IsAny<List<Expression<Func<HelpRequest, object>>>?>(),
            It.IsAny<bool>())).ReturnsAsync(source);

        var expectedResult = _mapper.Map<List<HelpRequest>, List<HelpRequestResult>>(source);

        // Act
        var action = await _helpRequestService.GetAllHelpRequests(request);

        // Assert
        action.Should().NotBeNull();
        action.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task GetHelpRequestById_WhenSearchingById_ReturnsDataAccordingToRequest()
    {
        // Arrange

        var requestId = "3";

        var helpRequest = new HelpRequest()
        {
            Id = "3",
            OwnerId = "3",
            Name = "Leha",
            Description = "Nunya"
        };

        _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);


        var expectedResult = _mapper.Map<HelpRequest, HelpRequestResult>(helpRequest);

        // Act
        var action = await _helpRequestService.GetHelpRequestById(requestId);

        // Assert
        action.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task CreateAsync_WhenOwnerIsNotFound_ThrowsNotFoundException()
    {
        // Arrange
        const string helpRequestOwnerId = "3";
        var helpRequest = new CreateHelpRequestRequest()
        {
            Name = "Leha",
            Description = "Nunya"
        };

        const string directory = "directory";

        var owner = new HelpRequest();

        _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(owner);

        var expectedResult = $"Entity \"{nameof(HelpSeeker)}\" ({helpRequestOwnerId}) was not found.";

        // Act

        var action = async () =>
        {
            await _helpRequestService.CreateAsync(helpRequest, helpRequestOwnerId, _formFileCollection.Object, directory);
        };

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage(expectedResult);
    }

    [Test]
    public async Task CreateAsync_WhenOwnerIsCreatingRequest_ReturnsResultAccordingToRequest()
    {
        // Arrange

        var formFiles = new FormFileCollection
        {
            UnitTestsHelper.GetMockFormFile("file", "file1.txt")
        };
        
        const string helpRequestOwnerId = "3";
        var helpRequest = new CreateHelpRequestRequest()
        {
            Name = "Leha",
            Description = "Nunya"
        };

        const string directory = "directory";

        var owner = new HelpSeeker()
        {
            Id = "3"
        };

        _helpSeekerRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(owner);

        var temporaryResult = _mapper.Map<CreateHelpRequestRequest, HelpRequest>(helpRequest);
        temporaryResult.Status = HelpRequestStatus.Active;
        temporaryResult.Owner = owner;
        var expectedResult = _mapper.Map<HelpRequest, HelpRequestResult>(temporaryResult);

        // Act

        var actualResult = await _helpRequestService.CreateAsync(helpRequest, helpRequestOwnerId, formFiles, directory);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult, o => o.Excluding(x => x.Id));
    }
}