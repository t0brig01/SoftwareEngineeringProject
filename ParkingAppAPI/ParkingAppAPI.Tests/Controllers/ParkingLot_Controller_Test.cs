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
    public class ParkingLot_Controller_Test
    {
        //Get All
        [TestMethod]
        public async Task GetAllLots_ShouldReturnAllLots()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            List<ParkingLotViewModel> lots = new List<ParkingLotViewModel>();
            repositoryMock.Setup(m => m.GetAllLots()).ReturnsAsync(lots);

            var controller = new LotsController(repositoryMock.Object);
            var ret = await controller.GetAll();

            var okResult = ret as OkObjectResult;
            var actualList = okResult.Value as List<ParkingLotViewModel>;

            repositoryMock.Verify((m => m.GetAllLots()), Times.Once());
            Assert.AreEqual(lots, actualList);
        }

        [TestMethod]
        public async Task GetAllLot_ShouldReturnNotFoundForNullLots()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.GetAllLots()).ReturnsAsync((List<ParkingLotViewModel>)null);

            var controller = new LotsController(repositoryMock.Object);
            var ret = await controller.GetAll();

            repositoryMock.Verify((m => m.GetAllLots()), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetAllLot_ShouldReturnBadRequestIfError()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.GetAllLots()).Throws(new Exception());

            var controller = new LotsController(repositoryMock.Object);
            var ret = await controller.GetAll();

            repositoryMock.Verify((m => m.GetAllLots()), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        //Get By Pass
        [TestMethod]
        public async Task GetLotsByPass_ShouldReturnAssociatedLots()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            List<ParkingLotViewModel> lots = new List<ParkingLotViewModel>();
            repositoryMock.Setup(m => m.GetLotsByPass(It.IsAny<String>())).ReturnsAsync(lots);

            var controller = new LotsController(repositoryMock.Object);
            var ret = await controller.GetByPass("PassCd");

            var okResult = ret as OkObjectResult;
            var actualList = okResult.Value as List<ParkingLotViewModel>;

            repositoryMock.Verify((m => m.GetLotsByPass("PassCd")), Times.Once());
            Assert.AreEqual(lots, actualList);
        }

        [TestMethod]
        public async Task GetLotsByPass_ShouldReturnNotFoundForNullLots()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.GetLotsByPass(It.IsAny<String>())).ReturnsAsync((List<ParkingLotViewModel>)null);

            var controller = new LotsController(repositoryMock.Object);
            var ret = await controller.GetByPass("PassCd");

            repositoryMock.Verify((m => m.GetLotsByPass("PassCd")), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetLotsByPass_ShouldReturnBadRequestIfError()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.GetLotsByPass(It.IsAny<String>())).Throws(new Exception());

            var controller = new LotsController(repositoryMock.Object);
            var ret = await controller.GetByPass("PassCd");

            repositoryMock.Verify((m => m.GetLotsByPass("PassCd")), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        //Get
        [TestMethod]
        public async Task GetLot_ShouldReturnAssociatedLot()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            ParkingLotViewModel lot = new ParkingLotViewModel();
            repositoryMock.Setup(m => m.GetLot(It.IsAny<int>())).ReturnsAsync(lot);

            var controller = new LotsController(repositoryMock.Object);
            var ret = await controller.Get(1);

            var okResult = ret as OkObjectResult;
            var actualLot = okResult.Value as ParkingLotViewModel;

            repositoryMock.Verify((m => m.GetLot(1)), Times.Once());
            Assert.AreEqual(lot, actualLot);
        }

        [TestMethod]
        public async Task GetLot_ShouldReturnNotFoundForNullLot()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.GetLot(It.IsAny<int>())).ReturnsAsync((ParkingLotViewModel)null);

            var controller = new LotsController(repositoryMock.Object);
            var ret = await controller.Get(1);

            repositoryMock.Verify((m => m.GetLot(1)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetLot_ShouldReturnBadRequestIfError()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.GetLot(It.IsAny<int>())).Throws(new Exception());

            var controller = new LotsController(repositoryMock.Object);
            var ret = await controller.Get(1);

            repositoryMock.Verify((m => m.GetLot(1)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        //Add
        [TestMethod]
        public async Task Add_ShouldReturnNewLotIdWhenValidLotIsInserted()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            int expected = 1;
            repositoryMock.Setup(m => m.AddLot(It.IsAny<ParkingLot>())).ReturnsAsync(expected);

            var controller = new LotsController(repositoryMock.Object);
            ParkingLot lot = new ParkingLot();
            var ret = await controller.Add(lot);

            var okResult = ret as OkObjectResult;
            var actual = okResult.Value;

            repositoryMock.Verify((m => m.AddLot(lot)), Times.Once());
            Assert.AreEqual((long)expected, actual);
        }

        [TestMethod]
        public async Task Add_ShouldReturnNotFoundWhenResultIsZero()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.AddLot(It.IsAny<ParkingLot>())).ReturnsAsync(0);

            var controller = new LotsController(repositoryMock.Object);
            ParkingLot lot = new ParkingLot();
            var ret = await controller.Add(lot);

            var okResult = ret as OkObjectResult;

            repositoryMock.Verify((m => m.AddLot(lot)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Add_ShouldReturnBadRequestIfError()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.AddLot(It.IsAny<ParkingLot>())).Throws(new Exception());

            var controller = new LotsController(repositoryMock.Object);
            ParkingLot lot = new ParkingLot();
            var ret = await controller.Add(lot);

            repositoryMock.Verify((m => m.AddLot(lot)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Add_ShouldReturnBadRequestIfInvalidLotIsPassedIn()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();

            var controller = new LotsController(repositoryMock.Object);
            controller.ModelState.AddModelError("Model Error", "Model Error");
            var ret = await controller.Add(new ParkingLot());

            Assert.IsInstanceOfType(ret, typeof(BadRequestResult));
        }

        //Delete
        [TestMethod]
        public async Task Delete_ShouldReturnOkWhenLotIsDeleted()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            int NotZero = 1;
            repositoryMock.Setup(m => m.DeleteLot(It.IsAny<int>())).ReturnsAsync(NotZero);

            var controller = new LotsController(repositoryMock.Object);
            int id = 1;
            var ret = await controller.Delete(id);

            repositoryMock.Verify((m => m.DeleteLot(id)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(OkResult));
        }

        [TestMethod]
        public async Task Delete_ShouldReturnNotFoundWhenResultIsZero()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.DeleteLot(It.IsAny<int>())).ReturnsAsync(0);

            var controller = new LotsController(repositoryMock.Object);
            int id = 1;
            var ret = await controller.Delete(id);

            repositoryMock.Verify((m => m.DeleteLot(id)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ShouldReturnBadRequestIfError()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.DeleteLot(It.IsAny<int>())).Throws(new Exception());

            var controller = new LotsController(repositoryMock.Object);
            int id = 1;
            var ret = await controller.Delete(id);

            repositoryMock.Verify((m => m.DeleteLot(id)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        //Update
        [TestMethod]
        public async Task Update_ShouldReturnOkWhenLotIsUpdated()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.UpdateLot(It.IsAny<ParkingLot>()));

            var controller = new LotsController(repositoryMock.Object);
            ParkingLot lot = new ParkingLot();
            var ret = await controller.Update(lot);

            repositoryMock.Verify((m => m.UpdateLot(lot)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(OkResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnNotFoundWhenDBConcurrenctExceptionOccurs()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.UpdateLot(It.IsAny<ParkingLot>())).Throws(new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() }));

            var controller = new LotsController(repositoryMock.Object);
            ParkingLot lot = new ParkingLot();
            var ret = await controller.Update(lot);

            repositoryMock.Verify((m => m.UpdateLot(lot)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnBadRequestWhenNormalExceptionOccurs()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();
            repositoryMock.Setup(m => m.UpdateLot(It.IsAny<ParkingLot>())).Throws(new Exception());

            var controller = new LotsController(repositoryMock.Object);
            ParkingLot lot = new ParkingLot();
            var ret = await controller.Update(lot);

            repositoryMock.Verify((m => m.UpdateLot(lot)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnBadRequestWhenModelStateIsInvalid()
        {
            Mock<ILotsRepository> repositoryMock = new Mock<ILotsRepository>();

            var controller = new LotsController(repositoryMock.Object);
            controller.ModelState.AddModelError("Model Error", "Model Error");
            var ret = await controller.Update(new ParkingLot());

            Assert.IsInstanceOfType(ret, typeof(BadRequestResult));
        }
    }
}
