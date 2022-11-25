using NUnit.Framework;
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Models.Results.HelpRequests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Volunteer_Corner.API.Controllers;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Requests.HelpRequests;

namespace Volunteer_Corner.ApiTests.Controllers;

[TestFixture]
public class HelpRequestsControllerTest
{
    private Mock<IHelpRequestService> _mockedHelpRequestService = null!;
    private HelpRequestsController _helpRequestsController = null!;


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

        _mockedHelpRequestService = new Mock<IHelpRequestService>();
        _helpRequestsController = new HelpRequestsController(_mockedHelpRequestService.Object);
    }

    [Test]
    public async Task Get_Always_ReturnsHelpRequests()
    {
        // Arrange
        var result = new List<HelpRequestResult>
        {
            new HelpRequestResult { Name = "sdadas" },
            new HelpRequestResult { Name = "sdadas" },
            new HelpRequestResult { Name = "sdadas" },
            new HelpRequestResult { Name = "sdadas" }
        };

        _mockedHelpRequestService.Setup(m => m.GetAllHelpRequests(It.IsAny<GetAllHelpRequestsRequest>()))
            .ReturnsAsync(result);

        // Act
        var actualResult = await _helpRequestsController.Get(Mock.Of<GetAllHelpRequestsRequest>()) as ObjectResult;

        //Assert
        actualResult.Should().NotBeNull();
        actualResult!.Value.Should().NotBeNull();
        actualResult.Value.Should().BeEquivalentTo(result);
        actualResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Test]
    public async Task Get_Always_ReturnsHelpRequestById()
    {
        // Arrange
        var result = new HelpRequestResult
        {
            Name = "asdasd"
        };

        _mockedHelpRequestService.Setup(m => m.GetHelpRequestById(It.IsAny<string>()))
            .ReturnsAsync(result);

        // Act
        var actualResult = await _helpRequestsController.Get(string.Empty) as OkObjectResult;

        //Assert
        actualResult.Should().NotBeNull();
        actualResult!.Value.Should().BeEquivalentTo(result);
        actualResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}