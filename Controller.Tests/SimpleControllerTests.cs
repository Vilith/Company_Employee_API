using Companies.Presentation.Controllers;
using Controller.Tests.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Controller.Tests
{
    public class SimpleControllerTests
    {
        [Fact]
        public async Task GetCompany_Should_Return400()
        {
            var sut = new SimpleController();
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

            var sut = new SimpleController();
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

            var sut = new SimpleController();

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
            var sut = new SimpleController();
            sut.SetUserIsAuth(true);

            var result = await sut.GetCompany();

            var resultType = result.Result as OkObjectResult;
            
            Assert.IsType<OkObjectResult>(resultType);


        }

    }
}