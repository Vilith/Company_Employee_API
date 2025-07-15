using AutoMapper;
using Companies.API.Data;
using Companies.Presentation.Controllers.ControllersForTestDemo;
using Companies.Shared.DTOs;
using Controller.Tests.Extensions;
using Controller.Tests.TestFixtures;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensions.Msal;
using Moq;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Tests
{
    public class RepositoryControllerTests : IClassFixture<RepoControllerFixture>
    {

        private readonly RepoControllerFixture _fixture;

        public RepositoryControllerTests(RepoControllerFixture fixture)
        {
            _fixture = fixture;

            //mockRepo = new Mock<IEmployeeRepository>();
            //var mockUow = new Mock<IUoW>();
            //mockServiceManager = new Mock<IServiceManager>();

            //mapper = new Mapper(new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<AutoMapperProfile>();
            //}));

            //var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            //var userManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            //sut = new RepositoryController(mockServiceManager.Object, mapper, userManager.Object);

        }

        [Fact]
        public async Task GetEmployees_ShouldReturnAllEmployees()
        {
            var users = _fixture.GetUsers();

            var dtos = _fixture.Mapper.Map<IEnumerable<EmployeeDTO>>(users);
            ApiBaseResponse baseResponse = new ApiOkResponse<IEnumerable<EmployeeDTO>>(dtos);


            //mockRepo.Setup(x => x.GetEmployeesAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(users);
            _fixture.MockServiceManager.Setup(x => x.EmployeeService.GetEmployeesAsync(It.IsAny<int>())).ReturnsAsync(baseResponse);


            _fixture.MockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new ApplicationUser { UserName = "Kalle" });

            var result = await _fixture.Sut.GetEmployees(1);

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsType<List<EmployeeDTO>>(okObjectResult.Value);
            Assert.Equal(items.Count, users.Count);

        }

        [Fact]
        public async Task GetEmployees_ShouldThrowExceptionIfUserNotFound()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await _fixture.Sut.GetEmployees(1));
        }
    }
}


