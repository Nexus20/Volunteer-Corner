using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Volunteer_Corner.Business.Exceptions;
using Volunteer_Corner.Business.Interfaces.Infrastructure;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Requests.HelpRequests;
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
    private Mock<IFileStorageService> _fileStorageService = null!;


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
            new HelpRequestService(_helpRequestRepository.Object, _mapper, _helpSeekerRepository.Object, new Mock<ILogger<HelpRequestService>>().Object, new Mock<IFileStorageService>().Object);
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
                Status = HelpRequestStatus.Active
            }
        };

        _helpRequestRepository.Setup(m => m.GetAsync(
            It.IsAny<Expression<Func<HelpRequest, bool>>?>(),
            It.IsAny<Func<IQueryable<HelpRequest>, IOrderedQueryable<HelpRequest>>?>(),
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

    [Test]

    public async Task ChangeStatusAsync_WhenHelpRequestIsNotFound_ThrowNotFoundException()
    {
        // Arrange

        var updateHelpRequestStatus = new UpdateHelpRequestStatus
        {
            NewStatus = HelpRequestStatus.Canceled
        };

        const string helpRequestId = "3";

        HelpRequest helpRequest = null;

        _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);

        var expectedResult = $"Entity \"{nameof(HelpRequest)}\" ({helpRequestId}) was not found.";

        // Act

        var action = async () =>
        {
            await _helpRequestService.ChangeStatusAsync(helpRequestId, updateHelpRequestStatus);
        };

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage(expectedResult);

    }

    [Test]
    public async Task ChangeStatusAsync_WhenHelpRequestStatusTryingToChangeFromCanceledToClosed_ThrowValidationException()
    {
        // Arrange

        var updateHelpRequestStatus = new UpdateHelpRequestStatus
        {
            NewStatus = HelpRequestStatus.Closed
        };

        const string helpRequestId = "3";

        var helpRequest = new HelpRequest()
        {
            Id = "3",
            Status = HelpRequestStatus.Canceled
        };

        _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);


        var expectedResult = $"Canceled request can't be set to closed";

        // Act

        var action = async () =>
        {
            await _helpRequestService.ChangeStatusAsync(helpRequestId, updateHelpRequestStatus);
        };

        // Assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage(expectedResult);

    }

    [Test]
    public async Task ChangeStatusAsync_WhenHelpRequestStatusTryingToChangeFromClosedToAny_ThrowValidationException()
    {
        // Arrange

        var updateHelpRequestStatus = new UpdateHelpRequestStatus
        {
            NewStatus = HelpRequestStatus.Active
        };

        const string helpRequestId = "3";

        var helpRequest = new HelpRequest()
        {
            Id = "3",
            Status = HelpRequestStatus.Closed
        };

        _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);


        const string expectedResult = "You can't change status of the closed request";

        // Act

        var action = async () =>
        {
            await _helpRequestService.ChangeStatusAsync(helpRequestId, updateHelpRequestStatus);
        };

        // Assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage(expectedResult);

    }

    [Test]
    public async Task ChangeStatusAsync_WhenOwnerIsUpdatingRequestStatus_ReturnsResultAccordingToRequest()
    {
        // Arrange

        const HelpRequestStatus expectedResult = HelpRequestStatus.Closed;

        var updateHelpRequestStatus = new UpdateHelpRequestStatus
        {
            NewStatus = expectedResult
        };

        const string helpRequestId = "3";

        var helpRequest = new HelpRequest()
        {
            Id = "3",
            Status = HelpRequestStatus.Active
        };

        _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);

        _helpRequestRepository.Setup(m => m.UpdateAsync(It.IsAny<HelpRequest>()));

        // Act

        var actualResult = await _helpRequestService.ChangeStatusAsync(helpRequestId, updateHelpRequestStatus);

        // Assert

        actualResult.Should().Be(expectedResult);
    }

    [Test]
    public async Task UpdateAsync_WhenHelpRequestIsNotFound_ThrowNotFoundException()
    {
        // Arrange

        var updateHelpRequestRequest = new UpdateHelpRequestRequest
        {
            Name = "Name",
            Location = "Location",
            Description = "Description"
        };

        const string helpRequestId = "3";

        HelpRequest helpRequest = null;
        _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);

        const string expectedResult = $"Entity \"HelpRequest\" ({helpRequestId}) was not found.";

        // Act

        var action = async () =>
        {
            await _helpRequestService.UpdateAsync(helpRequestId, updateHelpRequestRequest);
        };

        // Assert

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage(expectedResult);
    }

    [Test]
    public async Task UpdateAsync_WhenOwnerIsUpdatingRequest_ReturnsResultAccordingToRequest()
    {
        // Arrange

        var updateHelpRequestStatus = new UpdateHelpRequestRequest
        {
            Name = "Name",
            Location = "Location",
            Description = "Description"
        };

        const string helpRequestId = "3";

        var time = DateTime.Now;

        var helpRequest = new HelpRequest()
        {
            Id = helpRequestId,
            Status = HelpRequestStatus.Active,
            CreatedDate = time
        };

        var temporaryResult = _mapper.Map<UpdateHelpRequestRequest, HelpRequest>(updateHelpRequestStatus, helpRequest);

        var expectedResult = _mapper.Map<HelpRequest, HelpRequestResult>(temporaryResult);

        _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);

        // Act

        var action = await _helpRequestService.UpdateAsync(helpRequestId, updateHelpRequestStatus);

        // Assert

        action.Should().BeEquivalentTo(expectedResult);
    }

    //[Test]
    //public async Task AddDocumentsAsync_WhenRequestIsNotFound_ThrowNotFoundException()
    //{
    //    // Arrange

    //    var formFiles = new FormFileCollection
    //    {
    //        UnitTestsHelper.GetMockFormFile("file", "file1.txt")
    //    };

    //    const string helpRequestId = "3";

    //    const string directory = "directory";

    //    HelpRequest request = null;

    //    _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(request);
       
    //    const string expectedResult = $"Entity \"HelpRequest\" ({helpRequestId}) was not found.";

    //    // Act

    //    var action = async () =>
    //    {
    //        await _helpRequestService.AddDocumentsAsync(helpRequestId, formFiles, directory);
    //    };

    //    // Assert

    //    await action.Should().ThrowAsync<NotFoundException>()
    //        .WithMessage(expectedResult);

    //}

    //[Test]
    //public async Task AddDocumentsAsync_WhenOwnerIsUpdatingDocuments_ReturnsResultAccordingToRequest()
    //{
    //    // Arrange

    //    var formFiles = new FormFileCollection
    //    {
    //        UnitTestsHelper.GetMockFormFile("file", "file1.txt")
    //    };

    //    const string helpRequestId = "3";

    //    const string directory = "directory";

    //    HelpRequest request = new HelpRequest()
    //    {
    //        Id = "3",
    //        OwnerId = "3"
    //    };

    //    _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(request);

    //    _helpRequestRepository.Setup(m => m.AddDocumentsAsync(It.IsAny<List<HelpRequestDocument>>()));
    //    var expectedResult = _mapper.Map<List<HelpRequestDocument>, List<HelpRequestDocumentResult>>(request.AdditionalDocuments);

    //    // Act

    //    var action = await _helpRequestService.AddDocumentsAsync(helpRequestId, formFiles, directory);
       

    //    // Assert

    //    action.Should().BeEquivalentTo(expectedResult);
    //}

    //[Test]
    //public async Task DeleteDocumentsAsync_WhenHelpRequestIsNotFound_ThrowNotFoundException()
    //{
    //    // Arrange

    //    const string helpRequestId = "3";

    //    const string directory = "directory";

    //    DeleteHelpRequestDocumentsRequest deleteHelpRequestDocuments = null;

    //    HelpRequest helpRequest = null;

    //    _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);

    //    const string expectedResult = $"Entity \"HelpRequest\" ({helpRequestId}) was not found.";

    //    // Act

    //    var action = async () =>
    //    {
    //        await _helpRequestService.DeleteDocumentsAsync(helpRequestId, deleteHelpRequestDocuments, directory);
    //    };

    //    // Assert

    //    await action.Should().ThrowAsync<NotFoundException>()
    //        .WithMessage(expectedResult);
    //}
    //[Test]
    //public async Task DeleteDocumentsAsync_WhenHelpRequestIsNotFound_ThrowValidationException()
    //{
    //    // Arrange

    //    const string helpRequestId = "3";

    //    const string directory = "directory";

    //    var deleteHelpRequestDocuments = new DeleteHelpRequestDocumentsRequest()
    //    {
    //        DocumentsIds = new List<string>
    //        {
    //            "2"
    //        }
    //    };

    //    HelpRequest helpRequest = new HelpRequest()
    //    {
    //        Id = "3",
    //        AdditionalDocuments = new List<HelpRequestDocument>()
    //        {

    //        }
    //    };

    //    _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);

    //    const string expectedResult = $"No documents to delete";

    //    // Act

    //    var action = async () =>
    //    {
    //        await _helpRequestService.DeleteDocumentsAsync(helpRequestId, deleteHelpRequestDocuments, directory);
    //    };

    //    // Assert

    //    await action.Should().ThrowAsync<ValidationException>()
    //        .WithMessage(expectedResult);
    //}

    //[Test]
    //public async Task DeleteDocumentsAsync_WhenHelpRequestDocumentsIsInvalid_ThrowValidationException()
    //{
    //    // Arrange

    //    const string helpRequestId = "3";

    //    const string directory = "directory";

    //    var deleteHelpRequestDocuments = new DeleteHelpRequestDocumentsRequest()
    //    {
    //        DocumentsIds = new List<string>
    //        {
    //            "2"
    //        }
    //    };

    //    HelpRequest helpRequest = new HelpRequest()
    //    {
    //        Id = "3",
    //        AdditionalDocuments = new List<HelpRequestDocument>()
    //        {
    //            new HelpRequestDocument()
    //            {
    //                Id = "1"
    //            }
    //        }
    //    };

    //    _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);

    //    const string expectedResult = $"One of the documents ids is invalid";

    //    // Act

    //    var action = async () =>
    //    {
    //        await _helpRequestService.DeleteDocumentsAsync(helpRequestId, deleteHelpRequestDocuments, directory);
    //    };

    //    // Assert

    //    await action.Should().ThrowAsync<ValidationException>()
    //        .WithMessage(expectedResult);
    //}


    //[Test]
    //public async Task DeleteDocumentsAsync_WhenHelpRequestDocuments_ReturnsResultAccordingToRequest()
    //{
    //    // Arrange

    //    const string helpRequestId = "3";

    //    const string directory = "directory";

    //    var deleteHelpRequestDocuments = new DeleteHelpRequestDocumentsRequest()
    //    {
    //        DocumentsIds = new List<string>
    //        {
    //            "2"
    //        }
    //    };

    //    HelpRequest helpRequest = new HelpRequest()
    //    {
    //        Id = "3",
    //        AdditionalDocuments = new List<HelpRequestDocument>()
    //        {
    //            new HelpRequestDocument()
    //            {
    //                Id = "2",
    //                FilePath = "Path"
    //            },
    //            new HelpRequestDocument()
    //            {
    //                Id = "3",
    //                FilePath = "Path"
    //            }
    //        }
    //    };

    //    _helpRequestRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(helpRequest);

    //    // Act

    //    var action = async () =>
    //    {
    //        await _helpRequestService.DeleteDocumentsAsync(helpRequestId, deleteHelpRequestDocuments, directory);
    //    };

    //    // Assert

    //    await action.Should().NotThrowAsync();
    //}
}