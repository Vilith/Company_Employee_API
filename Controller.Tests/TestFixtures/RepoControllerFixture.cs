using AutoMapper;
using Companies.API.Data;
using Companies.Presentation.Controllers.ControllersForTestDemo;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Tests.TestFixtures
{
    public class RepoControllerFixture : IDisposable
    {
        public Mock<IServiceManager> MockServiceManager { get; }
        public Mapper Mapper { get; }
        public Mock<UserManager<ApplicationUser>> MockUserManager { get; }
        public RepositoryController Sut { get; }

        public RepoControllerFixture()

        {
            MockServiceManager = new Mock<IServiceManager>();

            Mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }));

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            MockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            Sut = new RepositoryController(MockServiceManager.Object, Mapper, MockUserManager.Object);
        }

        public List<ApplicationUser> GetUsers()
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "1",
                    Name = "Kalle",
                    Age = 12,
                    UserName = "Kalle"
                },

                new ApplicationUser
                {
                    Id = "2",
                    Name = "Kalle",
                    Age = 12,
                    UserName = "Kalle"
                },
            };
        }

        public void Dispose()
        {
            // Not used here
        }
    }
}
