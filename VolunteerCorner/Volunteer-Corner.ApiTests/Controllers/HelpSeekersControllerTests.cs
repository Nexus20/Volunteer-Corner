using System.Security.Claims;
using FluentAssertions;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Http;
using Volunteer_Corner.API.Controllers;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Requests.HelpSeekers;
using Volunteer_Corner.Business.Models.Results.HelpSeekers;
using Microsoft.AspNetCore.Mvc;
using Volunteer_Corner.Business;
using Volunteer_Corner.Business.Models.Results.HelpRequests;

namespace Volunteer_Corner.ApiTests.Controllers
{
    [TestFixture]
    public class HelpSeekersControllerTests
    {
        private Mock<IHelpSeekerService> _mockHelpSeekerService = null!;
        private Mock<IHelpRequestService> _mockHelpRequestService = null!;
        private HelpSeekersController _helpSeekersController;

        [SetUp]
        public void SetUp()
        {
            _mockHelpSeekerService = new Mock<IHelpSeekerService>();
            _mockHelpRequestService = new Mock<IHelpRequestService>();
            _helpSeekersController = new HelpSeekersController(_mockHelpSeekerService.Object, _mockHelpRequestService.Object);
        }

        [Test]
        public async Task MethodGetAll_Always_ReturnsHelpSeekers()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }, "TestAuthentication"));
            _helpSeekersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            _mockHelpSeekerService.Setup(h => h.GetAllHelpSeekers(It.IsAny<GetAllHelpSeekersRequest>()))
                .ReturnsAsync(GetSeeker());

            // Act
            var result = await _helpSeekersController.Get(new GetAllHelpSeekersRequest()) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(GetSeeker());
        }

        [Test]
        public async Task MethodGetById_Always_ReturnsHelpSeeker()
        {
            // Arrange
            string testHelpSeekerId = "1";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }, "TestAuthentication"));
            _helpSeekersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            _mockHelpSeekerService.Setup(h => h.GetHelpSeekerById(It.IsAny<string>()))
                .ReturnsAsync(GetSeeker().FirstOrDefault(result => result.Id == testHelpSeekerId));

            // Act
            var result = await _helpSeekersController.GetById(testHelpSeekerId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(GetSeeker().First());
        }

        [Test]
        public async Task MethodChangeApprovalStatus_Always_ChangeStatus()
        {
            // Arrange
            _mockHelpSeekerService.Setup(h => h.ChangeApprovalStatus(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _helpSeekersController.ChangeApprovalStatus("1") as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            Assert.AreEqual(true, result.Value);
        }


        [Test]
        public async Task MethodGetById_Always_ReturnsOwnHelpRequests()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(CustomClaimTypes.HelpSeekerId, "1"),
            }, "TestAuthentication"));
            _helpSeekersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            _mockHelpSeekerService.Setup(h => h.GetOwnHelpRequestsAsync(It.IsAny<string>()))
                .ReturnsAsync(GetRequest);

            // Act
            var result = await _helpSeekersController.GetOwnHelpRequests() as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(GetRequest());

        }

        [Test]
        public async Task MethodGetAll_Always_ReturnsOwnHelpRequest()
        {
            string testHelpSeekerId = "1";
            string testHelpRequestId = "1";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(CustomClaimTypes.HelpSeekerId, "1"),
            }, "TestAuthentication"));
            _helpSeekersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            _mockHelpSeekerService.Setup(h => h.GetOwnHelpRequestByIdAsync(It.IsAny<string>(),It.IsAny<string>()))
                .ReturnsAsync(GetRequestWithHelpResponses().FirstOrDefault(x => x.Id == "1"));

            // Act
            var result = await _helpSeekersController.GetOwnHelpRequest(testHelpRequestId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(GetRequestWithHelpResponses().First());
        }

        private List<HelpSeekerResult> GetSeeker()
        {
            return new List<HelpSeekerResult>()
            {
                new HelpSeekerResult() {Id = "1", LastName = "Anderson"},
                new HelpSeekerResult() {Id = "2", LastName = "Lovecraft"},
                new HelpSeekerResult() {Id = "3", LastName = "Morison"}
            };
        }

        private List<HelpRequestResult> GetRequest()
        {
            return new List<HelpRequestResult>
            {
                new HelpRequestResult { Id = "1", Name = "One" },
                new HelpRequestResult { Id = "2", Name = "Two" },
                new HelpRequestResult { Id = "3", Name = "Three" }
            };
        }

        private List<HelpRequestWithHelpResponsesResult> GetRequestWithHelpResponses()
        {
            return new List<HelpRequestWithHelpResponsesResult>
            {
                new HelpRequestWithHelpResponsesResult { Id = "1", Name = "TestOne" },
                new HelpRequestWithHelpResponsesResult { Id = "2", Name = "TestTwo" },
                new HelpRequestWithHelpResponsesResult { Id = "3", Name = "TestThree" }
            };
        }

    }
}
