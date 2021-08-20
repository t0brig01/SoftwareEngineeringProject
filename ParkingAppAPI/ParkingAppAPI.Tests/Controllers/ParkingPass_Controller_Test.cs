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
	public class ParkingPass_Controller_Test
	{
        //Get All
        [TestMethod]
        public async Task GetAll_ShouldReturnAllPasses()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            List<ParkingPassCdViewModel> passes = new List<ParkingPassCdViewModel>();
            repositoryMock.Setup(m => m.GetAllParkingPasses()).ReturnsAsync(passes);

            var controller = new PassController(repositoryMock.Object);
            var ret = await controller.GetAll();

            var okResult = ret as OkObjectResult;
            var actualList = okResult.Value as List<ParkingPassCdViewModel>;

            repositoryMock.Verify((m => m.GetAllParkingPasses()), Times.Once());
            Assert.AreEqual(passes, actualList);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnNotFoundForNullPasses()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            repositoryMock.Setup(m => m.GetAllParkingPasses()).ReturnsAsync((List<ParkingPassCdViewModel>)null);

            var controller = new PassController(repositoryMock.Object);
            var ret = await controller.GetAll();

            repositoryMock.Verify((m => m.GetAllParkingPasses()), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnBadRequestIfError()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            repositoryMock.Setup(m => m.GetAllParkingPasses()).Throws(new Exception());

            var controller = new PassController(repositoryMock.Object);
            var ret = await controller.GetAll();

            repositoryMock.Verify((m => m.GetAllParkingPasses()), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        //Add
        [TestMethod]
        public async Task Add_ShouldReturnResultWhenValidPassIsInserted()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            int expected = 1;
            repositoryMock.Setup(m => m.AddPass(It.IsAny<ParkingPassCd>())).ReturnsAsync(expected);

            var controller = new PassController(repositoryMock.Object);
            ParkingPassCd pass = new ParkingPassCd();
            var ret = await controller.Add(pass);

            var okResult = ret as OkObjectResult;
            var actual = okResult.Value;

            repositoryMock.Verify((m => m.AddPass(pass)), Times.Once());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Add_ShouldReturnNotFoundWhenResultIsZero()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            repositoryMock.Setup(m => m.AddPass(It.IsAny<ParkingPassCd>())).ReturnsAsync(0);

            var controller = new PassController(repositoryMock.Object);
            ParkingPassCd pass = new ParkingPassCd();
            var ret = await controller.Add(pass);

            repositoryMock.Verify((m => m.AddPass(pass)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Add_ShouldReturnBadRequestIfError()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            repositoryMock.Setup(m => m.AddPass(It.IsAny<ParkingPassCd>())).Throws(new Exception());

            var controller = new PassController(repositoryMock.Object);
            ParkingPassCd pass = new ParkingPassCd();
            var ret = await controller.Add(pass);

            repositoryMock.Verify((m => m.AddPass(pass)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Add_ShouldReturnBadRequestIfInvalidPassIsPassedIn()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();

            var controller = new PassController(repositoryMock.Object);
            controller.ModelState.AddModelError("Model Error", "Model Error");
            var ret = await controller.Add(new ParkingPassCd());

            Assert.IsInstanceOfType(ret, typeof(BadRequestResult));
        }

        //Delete
        [TestMethod]
        public async Task Delete_ShouldReturnOkWhenPassIsDeleted()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            int NotZero = 1;
            repositoryMock.Setup(m => m.DeletePass(It.IsAny<String>())).ReturnsAsync(NotZero);

            var controller = new PassController(repositoryMock.Object);
            String cd = "PassCd";
            var ret = await controller.DeletePost(cd);

            repositoryMock.Verify((m => m.DeletePass(cd)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(OkResult));
        }

        [TestMethod]
        public async Task Delete_ShouldReturnBadRequestWhenNullString()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            repositoryMock.Setup(m => m.DeletePass(It.IsAny<String>()));

            var controller = new PassController(repositoryMock.Object);
            String cd = null;
            var ret = await controller.DeletePost(cd);

            Assert.IsInstanceOfType(ret, typeof(BadRequestResult));
            repositoryMock.Verify((m => m.DeletePass(cd)), Times.Never());
        }

        [TestMethod]
        public async Task Delete_ShouldReturnNotFoundWhenResultIsZero()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            repositoryMock.Setup(m => m.DeletePass(It.IsAny<String>())).ReturnsAsync(0);

            var controller = new PassController(repositoryMock.Object);
            String cd = "PassCd";
            var ret = await controller.DeletePost(cd);

            repositoryMock.Verify((m => m.DeletePass(cd)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ShouldReturnBadRequestIfError()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            repositoryMock.Setup(m => m.DeletePass(It.IsAny<String>())).Throws(new Exception());

            var controller = new PassController(repositoryMock.Object);
            String cd = "PassCd";
            var ret = await controller.DeletePost(cd);

            repositoryMock.Verify((m => m.DeletePass(cd)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        //Update
        [TestMethod]
        public async Task Update_ShouldReturnOkWhenPassIsUpdated()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            repositoryMock.Setup(m => m.UpdatePass(It.IsAny<ParkingPassCd>()));

            var controller = new PassController(repositoryMock.Object);
            ParkingPassCd pass = new ParkingPassCd();
            var ret = await controller.UpdatePost(pass);

            repositoryMock.Verify((m => m.UpdatePass(pass)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(OkResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnNotFoundWhenDBConcurrenctExceptionOccurs()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            repositoryMock.Setup(m => m.UpdatePass(It.IsAny<ParkingPassCd>())).Throws(new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() }));

            var controller = new PassController(repositoryMock.Object);
            ParkingPassCd pass = new ParkingPassCd();
            var ret = await controller.UpdatePost(pass);

            repositoryMock.Verify((m => m.UpdatePass(pass)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnBadRequestWhenNormalExceptionOccurs()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();
            repositoryMock.Setup(m => m.UpdatePass(It.IsAny<ParkingPassCd>())).Throws(new Exception());

            var controller = new PassController(repositoryMock.Object);
            ParkingPassCd pass = new ParkingPassCd();
            var ret = await controller.UpdatePost(pass);

            repositoryMock.Verify((m => m.UpdatePass(pass)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnBadRequestWhenModelStateIsInvalid()
        {
            Mock<IParkingPassRepository> repositoryMock = new Mock<IParkingPassRepository>();

            var controller = new PassController(repositoryMock.Object);
            controller.ModelState.AddModelError("Model Error", "Model Error");
            var ret = await controller.UpdatePost(new ParkingPassCd());

            Assert.IsInstanceOfType(ret, typeof(BadRequestResult));
        }
    }
}
