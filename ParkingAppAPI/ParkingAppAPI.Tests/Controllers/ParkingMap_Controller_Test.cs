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
	public class ParkingMap_Controller_Test
	{
        //Get All
        [TestMethod]
        public async Task GetAll_ShouldReturnAllMapInstances()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            List<MapViewModel> maps = new List<MapViewModel>();
            repositoryMock.Setup(m => m.GetAllMaps()).ReturnsAsync(maps);

            var controller = new MapController(repositoryMock.Object);
            var ret = await controller.GetAll();

            var okResult = ret as OkObjectResult;
            var actualList = okResult.Value as List<MapViewModel>;

            repositoryMock.Verify((m => m.GetAllMaps()), Times.Once());
            Assert.AreEqual(maps, actualList);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnNotFoundForNullMappings()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.GetAllMaps()).ReturnsAsync((List<MapViewModel>)null);

            var controller = new MapController(repositoryMock.Object);
            var ret = await controller.GetAll();

            repositoryMock.Verify((m => m.GetAllMaps()), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnBadRequestIfError()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.GetAllMaps()).Throws(new Exception());

            var controller = new MapController(repositoryMock.Object);
            var ret = await controller.GetAll();

            repositoryMock.Verify((m => m.GetAllMaps()), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        //Add
        [TestMethod]
        public async Task Add_ShouldReturnResultWhenValidMappingIsInserted()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            int expected = 1;
            repositoryMock.Setup(m => m.AddMap(It.IsAny<ParkingMap>())).ReturnsAsync(expected);

            var controller = new MapController(repositoryMock.Object);
            ParkingMap map = new ParkingMap();
            var ret = await controller.Add(map);

            var okResult = ret as OkObjectResult;
            var actual = okResult.Value;

            repositoryMock.Verify((m => m.AddMap(map)), Times.Once());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Add_ShouldReturnNotFoundWhenResultIsZero()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.AddMap(It.IsAny<ParkingMap>())).ReturnsAsync(0);

            var controller = new MapController(repositoryMock.Object);
            ParkingMap map = new ParkingMap();
            var ret = await controller.Add(map);

            repositoryMock.Verify((m => m.AddMap(map)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Add_ShouldReturnBadRequestIfError()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.AddMap(It.IsAny<ParkingMap>())).Throws(new Exception());

            var controller = new MapController(repositoryMock.Object);
            ParkingMap map = new ParkingMap();
            var ret = await controller.Add(map);

            repositoryMock.Verify((m => m.AddMap(map)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Add_ShouldReturnBadRequestIfInvalidMappingIsPassedIn()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();

            var controller = new MapController(repositoryMock.Object);
            controller.ModelState.AddModelError("Model Error", "Model Error");
            var ret = await controller.Add(new ParkingMap());

            Assert.IsInstanceOfType(ret, typeof(BadRequestResult));
        }

        //Delete
        [TestMethod]
        public async Task Delete_ShouldReturnOkWhenMappingIsDeleted()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            int NotZero = 1;
            repositoryMock.Setup(m => m.DeleteMap(It.IsAny<int>())).ReturnsAsync(NotZero);

            var controller = new MapController(repositoryMock.Object);
            int id = 1;
            var ret = await controller.Delete(id);

            repositoryMock.Verify((m => m.DeleteMap(id)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(OkResult));
        }

        [TestMethod]
        public async Task Delete_ShouldReturnNotFoundWhenResultIsZero()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.DeleteMap(It.IsAny<int>())).ReturnsAsync(0);

            var controller = new MapController(repositoryMock.Object);
            int id = 1;
            var ret = await controller.Delete(id);

            repositoryMock.Verify((m => m.DeleteMap(id)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ShouldReturnBadRequestIfError()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.DeleteMap(It.IsAny<int>())).Throws(new Exception());

            var controller = new MapController(repositoryMock.Object);
            int id = 1;
            var ret = await controller.Delete(id);

            repositoryMock.Verify((m => m.DeleteMap(id)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        //DeleteByLotId
        [TestMethod]
        public async Task DeleteByLotId_ShouldReturnOkWhenMappingIsDeleted()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.DeleteMapByLotId(It.IsAny<int>())).ReturnsAsync(0);

            var controller = new MapController(repositoryMock.Object);
            int id = 1;
            var ret = await controller.DeleteByLotId(id);

            repositoryMock.Verify((m => m.DeleteMapByLotId(id)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteByLotId_ShouldReturnNotFoundWhenMappingIsntFound()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            int notZero = 1;
            repositoryMock.Setup(m => m.DeleteMapByLotId(It.IsAny<int>())).ReturnsAsync(notZero);

            var controller = new MapController(repositoryMock.Object);
            int id = 1;
            var ret = await controller.DeleteByLotId(id);

            repositoryMock.Verify((m => m.DeleteMapByLotId(id)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteByLotId_ShouldReturnBadRequestIfError()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.DeleteMapByLotId(It.IsAny<int>())).Throws(new Exception());

            var controller = new MapController(repositoryMock.Object);
            int id = 1;
            var ret = await controller.DeleteByLotId(id);

            repositoryMock.Verify((m => m.DeleteMapByLotId(id)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        //DeleteByPassColor
        [TestMethod]
        public async Task DeleteByPassColor_ShouldReturnOkWhenMappingIsDeleted()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.DeleteMapByPassColor(It.IsAny<string>())).ReturnsAsync(0);

            var controller = new MapController(repositoryMock.Object);
            string cd = "cd";
            var ret = await controller.DeleteByPassColor(cd);

            repositoryMock.Verify((m => m.DeleteMapByPassColor(cd)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteByPassColor_ShouldReturnNotFoundWhenMappingIsntFound()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            int notZero = 1;
            repositoryMock.Setup(m => m.DeleteMapByPassColor(It.IsAny<string>())).ReturnsAsync(notZero);

            var controller = new MapController(repositoryMock.Object);
            string cd = "cd";
            var ret = await controller.DeleteByPassColor(cd);

            repositoryMock.Verify((m => m.DeleteMapByPassColor(cd)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteByPassColor_ShouldReturnBadRequestIfError()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.DeleteMapByPassColor(It.IsAny<string>())).Throws(new Exception());

            var controller = new MapController(repositoryMock.Object);
            string cd = "cd";
            var ret = await controller.DeleteByPassColor(cd);

            repositoryMock.Verify((m => m.DeleteMapByPassColor(cd)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        //Update
        [TestMethod]
        public async Task Update_ShouldReturnOkWhenMappingIsUpdated()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.UpdateMap(It.IsAny<ParkingMap>()));

            var controller = new MapController(repositoryMock.Object);
            ParkingMap map = new ParkingMap();
            var ret = await controller.Update(map);

            repositoryMock.Verify((m => m.UpdateMap(map)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(OkResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnNotFoundWhenDBConcurrenctExceptionOccurs()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.UpdateMap(It.IsAny<ParkingMap>())).Throws(new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() }));

            var controller = new MapController(repositoryMock.Object);
            ParkingMap map = new ParkingMap();
            var ret = await controller.Update(map);

            repositoryMock.Verify((m => m.UpdateMap(map)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnBadRequestWhenNormalExceptionOccurs()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();
            repositoryMock.Setup(m => m.UpdateMap(It.IsAny<ParkingMap>())).Throws(new Exception());

            var controller = new MapController(repositoryMock.Object);
            ParkingMap map = new ParkingMap();
            var ret = await controller.Update(map);

            repositoryMock.Verify((m => m.UpdateMap(map)), Times.Once());
            Assert.IsInstanceOfType(ret, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnBadRequestWhenModelStateIsInvalid()
        {
            Mock<IMapRepository> repositoryMock = new Mock<IMapRepository>();

            var controller = new MapController(repositoryMock.Object);
            controller.ModelState.AddModelError("Model Error", "Model Error");
            var ret = await controller.Update(new ParkingMap());

            Assert.IsInstanceOfType(ret, typeof(BadRequestResult));
        }
    }
}
