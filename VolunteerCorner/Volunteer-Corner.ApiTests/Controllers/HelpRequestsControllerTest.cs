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
using Volunteer_Corner.Business.Models.Results.HelpRequests;

[TestFixture]
public class HelpRequestsControllerTest
{

        private Mock<IHelpRequestService> _mockedIHelpRequestService = null!;
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

        _mockedIHelpRequestService = new Mock<IHelpRequestService>();
        _helpRequestsController = new HelpRequestsController(_mockedIHelpRequestService.Object);

        }

        [Test]
        public async Task Get_Always_ReturnsHelpRequestResult()
        {
            // Arrange
            var result = new List<HelpRequestResult>
                {
                   new HelpRequestResult{ Name = "sdadas"},
                   new HelpRequestResult{ Name = "sdadas"},
                   new HelpRequestResult{ Name = "sdadas"},
                   new HelpRequestResult{ Name = "sdadas"}
                };

            _mockedIHelpRequestService.Setup(m => m.GetAllHelpRequests(It.IsAny<GetAllHelpRequestsRequest>()))
            .ReturnsAsync(result);

            // Act
            var actualResult = await _helpRequestsController.Get(Mock.Of <GetAllHelpRequestsRequest> ()) as ObjectResult;

            //Assert
            actualResult.Should().NotBeNull();
            actualResult.Value.Should().NotBeNull();
            actualResult.Value.Should().BeEquivalentTo(result);
            actualResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task Get_Always_GetHelpRequestById()
        {
            // Arrange
            var result = new HelpRequestResult
            {
                Name = "asdasd"
            };

            _mockedIHelpRequestService.Setup(m => m.GetHelpRequestById(It.IsAny<string> ()))
            .ReturnsAsync(result);

            // Act
            var actualResult = await _helpRequestsController.Get(Mock.Of<GetAllHelpRequestsRequest>()) as ObjectResult;

        //Assert
            actualResult.Value.Should().BeEquivalentTo(result);
            actualResult.Should().NotBeNull();
            actualResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
}

