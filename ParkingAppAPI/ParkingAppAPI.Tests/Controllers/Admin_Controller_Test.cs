using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingAppAPI.Controllers;
using ParkingAppAPI.Models;
using ParkingAppAPI.Repository;
using ParkingAppAPI.ViewModels;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

namespace ParkingAppAPI.Tests
{
	[TestClass]
	public class Admin_Controller_Test
	{
        //Get All
        [TestMethod]
        public async Task GetAll_ShouldReturnAllAdmins()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            List<AdminViewModel> admins = new List<AdminViewModel>();
            repositoryMock.Setup(m => m.GetAllAdmins()).ReturnsAsync(admins);

            var controller = new AdminController(repositoryMock.Object);
            var ret = await controller.GetAll();

            var okResult = ret as OkObjectResult;
            var actualList = okResult.Value as List<AdminViewModel>;

            repositoryMock.Verify((m => m.GetAllAdmins()), Times.Once());
            Assert.AreEqual(admins, actualList);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnNotFoundForNullAdmins()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            repositoryMock.Setup(m => m.GetAllAdmins()).ReturnsAsync((List<AdminViewModel>)null);

            var controller = new AdminController(repositoryMock.Object);
            var ret = await controller.GetAll();

            repositoryMock.Verify((m => m.GetAllAdmins()), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnBadRequestIfError()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            repositoryMock.Setup(m => m.GetAllAdmins()).Throws(new Exception());

            var controller = new AdminController(repositoryMock.Object);
            var ret = await controller.GetAll();

            repositoryMock.Verify((m => m.GetAllAdmins()), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        //Add
        [TestMethod]
        public async Task Add_ShouldReturnOkWhenValidAdminInserted()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            int expected = 1;
            repositoryMock.Setup(m => m.AddAdmin(It.IsAny<Admin>())).ReturnsAsync(expected);

            var controller = new AdminController(repositoryMock.Object);
            Admin admin = new Admin();
            var ret = await controller.Add(admin);

            repositoryMock.Verify((m => m.AddAdmin(admin)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(OkResult));
        }

        [TestMethod]
        public async Task Add_ShouldReturnBadRequestIfError()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            repositoryMock.Setup(m => m.AddAdmin(It.IsAny<Admin>())).Throws(new Exception());

            var controller = new AdminController(repositoryMock.Object);
            Admin admin = new Admin();
            var ret = await controller.Add(admin);

            repositoryMock.Verify((m => m.AddAdmin(admin)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Add_ShouldReturnBadRequestIfInvalidPassIsPassedIn()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();

            var controller = new AdminController(repositoryMock.Object);
            controller.ModelState.AddModelError("Model Error", "Model Error");
            var ret = await controller.Add(new Admin());

            Assert.IsInstanceOfType(ret, typeof(BadRequestResult));
        }

        //Delete
        [TestMethod]
        public async Task Delete_ShouldReturnNotFoundWhenResultIsNotZero()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            int NotZero = 1;
            repositoryMock.Setup(m => m.DeleteAdmin(It.IsAny<String>())).ReturnsAsync(NotZero);

            var controller = new AdminController(repositoryMock.Object);
            String userName = "UserName";
            var ret = await controller.Delete(userName);

            repositoryMock.Verify((m => m.DeleteAdmin(userName)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ShouldReturnBadRequestWhenNullString()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            repositoryMock.Setup(m => m.DeleteAdmin(It.IsAny<String>()));

            var controller = new AdminController(repositoryMock.Object);
            String userName = null;
            var ret = await controller.Delete(userName);

            Assert.IsInstanceOfType(ret, typeof(BadRequestResult));
            repositoryMock.Verify((m => m.DeleteAdmin(userName)), Times.Never());
        }

        [TestMethod]
        public async Task Delete_ShouldReturnOkWhenResultIsZero()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            repositoryMock.Setup(m => m.DeleteAdmin(It.IsAny<String>())).ReturnsAsync(0);

            var controller = new AdminController(repositoryMock.Object);
            String userName = "UserName";
            var ret = await controller.Delete(userName);

            repositoryMock.Verify((m => m.DeleteAdmin(userName)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(OkResult));
        }

        [TestMethod]
        public async Task Delete_ShouldReturnBadRequestIfError()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            repositoryMock.Setup(m => m.DeleteAdmin(It.IsAny<String>())).Throws(new Exception());

            var controller = new AdminController(repositoryMock.Object);
            String userName = "UserName";
            var ret = await controller.Delete(userName);

            repositoryMock.Verify((m => m.DeleteAdmin(userName)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        //Update
        [TestMethod]
        public async Task Update_ShouldReturnOkWhenAdminIsUpdated()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            repositoryMock.Setup(m => m.UpdateAdmin(It.IsAny<Admin>()));

            var controller = new AdminController(repositoryMock.Object);
            Admin admin = new Admin();
            var ret = await controller.Update(admin);

            repositoryMock.Verify((m => m.UpdateAdmin(admin)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(OkResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnNotFoundWhenDBConcurrenctExceptionOccurs()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            repositoryMock.Setup(m => m.UpdateAdmin(It.IsAny<Admin>())).Throws(new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() }));

            var controller = new AdminController(repositoryMock.Object);
            Admin admin = new Admin();
            var ret = await controller.Update(admin);

            repositoryMock.Verify((m => m.UpdateAdmin(admin)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnBadRequestWhenNormalExceptionOccurs()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            repositoryMock.Setup(m => m.UpdateAdmin(It.IsAny<Admin>())).Throws(new Exception());

            var controller = new AdminController(repositoryMock.Object);
            Admin admin = new Admin();
            var ret = await controller.Update(admin);

            repositoryMock.Verify((m => m.UpdateAdmin(admin)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnBadRequestWhenModelStateIsInvalid()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();

            var controller = new AdminController(repositoryMock.Object);
            controller.ModelState.AddModelError("Model Error", "Model Error");
            var ret = await controller.Update(new Admin());

            Assert.IsInstanceOfType(ret, typeof(BadRequestResult));
        }

        //Login Validation
        [TestMethod]
        public async Task ValidateLogin_ShouldReturnOkWhenAdminIsValidated()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            int valStatus = 1;
            repositoryMock.Setup(m => m.ValidateLogin(It.IsAny<Admin>())).ReturnsAsync(valStatus);

            var controller = new AdminController(repositoryMock.Object);
            Admin admin = new Admin();
            var ret = await controller.ValidateLogin(admin);

            var okResult = ret as OkObjectResult;
            var actual = okResult.Value;

            repositoryMock.Verify((m => m.ValidateLogin(admin)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(OkObjectResult));
            Assert.AreEqual(valStatus, actual);
        }

        [TestMethod]
        public async Task ValidateLogin_ShouldReturnBadRequestWhenPasswordinIsInvalid()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            int valStatus = 0;
            repositoryMock.Setup(m => m.ValidateLogin(It.IsAny<Admin>())).ReturnsAsync(valStatus);

            var controller = new AdminController(repositoryMock.Object);
            Admin admin = new Admin();
            var ret = await controller.ValidateLogin(admin);

            var badRequestResult = ret as BadRequestObjectResult;
            var actual = badRequestResult.Value;

            repositoryMock.Verify((m => m.ValidateLogin(admin)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
            Assert.AreEqual("Invalid password", actual);
        }

        [TestMethod]
        public async Task ValidateLogin_ShouldReturnBadRequestWhenResultIsntZeroOrOne()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            int valStatus = 2;
            repositoryMock.Setup(m => m.ValidateLogin(It.IsAny<Admin>())).ReturnsAsync(valStatus);

            var controller = new AdminController(repositoryMock.Object);
            Admin admin = new Admin();
            var ret = await controller.ValidateLogin(admin);

            repositoryMock.Verify((m => m.ValidateLogin(admin)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task ValidateLogin_ShouldReturnBadRequestWhenError()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();
            repositoryMock.Setup(m => m.ValidateLogin(It.IsAny<Admin>())).Throws(new Exception());

            var controller = new AdminController(repositoryMock.Object);
            Admin admin = new Admin();
            var ret = await controller.ValidateLogin(admin);

            repositoryMock.Verify((m => m.ValidateLogin(admin)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ValidateLogin_ShouldReturnBadRequestIfModelDataIsInvalid()
        {
            Mock<IAdminRepository> repositoryMock = new Mock<IAdminRepository>();

            var controller = new AdminController(repositoryMock.Object);
            controller.ModelState.AddModelError("Model Error", "Model Error");
            var ret = await controller.ValidateLogin(new Admin());

            Assert.IsInstanceOfType(ret, typeof(BadRequestResult));
        }
    }
}
