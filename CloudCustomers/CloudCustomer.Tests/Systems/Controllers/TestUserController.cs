using CloudCustomers.API.Controllers;
using CloudCustomers.API.Models;
using CloudCustomers.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCustomer.Tests.Systems.Controllers
{
    public class TestUserController
    {
        [Fact]
        public async Task Get_OnSuccess_ReturnsStatusCode200()
        {
            //Arrang
            var mockUserService = new Mock<IUsersService>();
            mockUserService.Setup(service => service.GetAllUsers())
                .ReturnsAsync(new List<User>());
            var sut = new UsersController(mockUserService.Object);

            //Act
            var result = (OkObjectResult) await sut.Get();

            //Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Get_OnSuccess_InvokesUserService()
        {
            //Arrang
            var mockUserService = new Mock<IUsersService>();
            mockUserService.Setup(service => service.GetAllUsers())
                .ReturnsAsync(new List<User>());
            var sut = new UsersController(mockUserService.Object);

            //Act
            var result = (OkObjectResult)await sut.Get();

            //Assert
            mockUserService.Verify(service => service.GetAllUsers(), Times.Once());
        }
    }
}
