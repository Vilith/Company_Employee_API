using Companies.Presentation.Controllers;
using Companies.Shared.DTOs;
using Controller.Tests.Extensions;
using Controller.Tests.TestFixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Controller.Tests
{
    public class SimpleControllerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public SimpleControllerTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetCompany_ShouldReturnExpectedCount()
        {
            var sut = _fixture.Sut;
            var expetedCount = _fixture.Context.Companies.Count();

            var result = await sut.GetCompany2();

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsType<List<CompanyDTO>>(okObjectResult.Value);

            Assert.Equal(expetedCount, items.Count);

        }

        [Fact]
        public async Task GetCompany_Should_Return400()
        {
            var sut = _fixture.Sut;                        
            sut.SetUserIsAuth(false); // Setting the state of the user to not authenticated to make sure we get a 400 Bad Request response.
            var result = await sut.GetCompany();

            var resultType = result.Result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(resultType);
            Assert.Equal(StatusCodes.Status400BadRequest, resultType.StatusCode);

        }

        [Fact]
        public async Task GetCompany_IfNotAuth_ShouldReturn400BadRequest()
        {
            var httpContextMock = new Mock<HttpContext>();

            httpContextMock.Setup(x => x.User.Identity.IsAuthenticated).Returns(false);

            var controllerContext = new ControllerContext()
            { HttpContext = httpContextMock.Object };

            var sut = _fixture.Sut;
            sut.ControllerContext = controllerContext;

            var res = await sut.GetCompany();
            
            var resultType = res.Result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(resultType);
            Assert.Equal("Is Not Authenticated", resultType.Value);
        }

        [Fact]
        public async Task GetCompany_IfNotAuth_ShouldReturn400BadRequest2()
        {
            //var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            //mockClaimsPrincipal.SetupGet(x => x.Identity.IsAuthenticated).Returns(false);

            var sut = _fixture.Sut;

            sut.SetUserIsAuth(false);
            //sut.ControllerContext = new ControllerContext
            //{
                //HttpContext = new DefaultHttpContext()
                //{
                    //User = mockClaimsPrincipal.Object
                //}
            //};

            var result = await sut.GetCompany();
            var resultType = result.Result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(resultType);
            Assert.Equal(StatusCodes.Status400BadRequest, resultType.StatusCode);
        }

        [Fact]
        public async Task GetCompany_IsAuth_ShouldReturn200()
        {
            var sut = _fixture.Sut;
            sut.SetUserIsAuth(true);

            var result = await sut.GetCompany();

            var resultType = result.Result as OkObjectResult;
            
            Assert.IsType<OkObjectResult>(resultType);
        }
    }
}