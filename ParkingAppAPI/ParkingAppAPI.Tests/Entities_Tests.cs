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
	public class EntitiesTests
	{
        [TestMethod]
        public void AdminModelTest()
        {
            Admin admin = new Admin()
            {
                Id = 0,
                Username = "Test",
                Password = "test"
            };
            Assert.AreEqual(0, admin.Id);
            Assert.AreEqual("Test", admin.Username);
            Assert.AreEqual("test", admin.Password);
        }
        [TestMethod]
        public void ParkingLotModelTest()
        {
            TimeSpan time = DateTime.Now.TimeOfDay;
            ParkingLot parkingLot = new ParkingLot()
            {
                Id = 0,
                ShortDesc = "test",
                ExclusivePassStart = time,
                AnyPassStart = time,
                NoPassStart = time,
                Address = "test address",
                WeekendEnforcementFlag = true,
                Latitude = 1234,
                Longitude = 1234
            };
            Assert.AreEqual(0, parkingLot.Id);
            Assert.AreEqual("test", parkingLot.ShortDesc);
            Assert.AreEqual(time, parkingLot.ExclusivePassStart);
            Assert.AreEqual(time, parkingLot.AnyPassStart);
            Assert.AreEqual(time, parkingLot.NoPassStart);
            Assert.AreEqual("test address", parkingLot.Address);
            Assert.AreEqual(true, parkingLot.WeekendEnforcementFlag);
            Assert.AreEqual(1234, parkingLot.Latitude);
            Assert.AreEqual(1234, parkingLot.Longitude);
        }
        [TestMethod]
        public void ParkingMapModelTest()
        {
            ParkingMap parkingMap = new ParkingMap()
            {
                Id = 0,
                ParkingLotId = 0,
                ParkingPassCd = "blue",
                PrimaryFlag = true
            };
            Assert.AreEqual(0, parkingMap.Id);
            Assert.AreEqual(0, parkingMap.ParkingLotId);
            Assert.AreEqual("blue", parkingMap.ParkingPassCd);
            Assert.AreEqual(true, parkingMap.PrimaryFlag);
        }
        [TestMethod]
        public void ParkingPassCdModelTest()
        {
            ParkingPassCd pass = new ParkingPassCd()
            {
                ParkingPassCd1 = "test",
                ShortDesc = "test",
                DisplaySequence = 0,
                ActiveFlag = true
            };
            Assert.AreEqual("test", pass.ParkingPassCd1);
            Assert.AreEqual("test", pass.ShortDesc);
            Assert.AreEqual(0, pass.DisplaySequence);
            Assert.AreEqual(true, pass.ActiveFlag);
        }



        [TestMethod]
        public void AdminViewModelTest()
        {
            AdminViewModel admin = new AdminViewModel()
            {
                Id = 0,
                Username = "Test",
                Password = "test"
            };
            Assert.AreEqual(0, admin.Id);
            Assert.AreEqual("Test", admin.Username);
            Assert.AreEqual("test", admin.Password);
        }
        [TestMethod]
        public void ParkingLotViewModelTest()
        {
            TimeSpan time = DateTime.Now.TimeOfDay;
            ParkingLotViewModel parkingLot = new ParkingLotViewModel()
            {
                Id = 0,
                ShortDesc = "test",
                ExclusivePassStart = time,
                ExclusivePassEnd = time,
                AnyPassStart = time,
                AllPassStart = time,
                Address = "test address",
                WeekendEnforcementFlag = true,
                Latitude = 1234,
                Longitude = 1234
            };
            Assert.AreEqual(0, parkingLot.Id);
            Assert.AreEqual("test", parkingLot.ShortDesc);
            Assert.AreEqual(time, parkingLot.ExclusivePassStart);
            Assert.AreEqual(time, parkingLot.ExclusivePassEnd);
            Assert.AreEqual(time, parkingLot.AnyPassStart);
            Assert.AreEqual(time, parkingLot.AllPassStart);
            Assert.AreEqual("test address", parkingLot.Address);
            Assert.AreEqual(true, parkingLot.WeekendEnforcementFlag);
            Assert.AreEqual(1234, parkingLot.Latitude);
            Assert.AreEqual(1234, parkingLot.Longitude);
        }
        [TestMethod]
        public void ParkingMapViewModelTest()
        {
            MapViewModel parkingMap = new MapViewModel()
            {
                Id = 0,
                ParkingLotId = 0,
                ParkingPassCd = "blue",
                PrimaryFlag = true
            };
            Assert.AreEqual(0, parkingMap.Id);
            Assert.AreEqual(0, parkingMap.ParkingLotId);
            Assert.AreEqual("blue", parkingMap.ParkingPassCd);
            Assert.AreEqual(true, parkingMap.PrimaryFlag);
        }
        [TestMethod]
        public void ParkingPassCdViewModelTest()
        {
            ParkingPassCdViewModel pass = new ParkingPassCdViewModel()
            {
                ParkingPassCd1 = "test",
                ShortDesc = "test",
                DisplaySequence = 0,
                ActiveFlag = true
            };
            Assert.AreEqual("test", pass.ParkingPassCd1);
            Assert.AreEqual("test", pass.ShortDesc);
            Assert.AreEqual(0, pass.DisplaySequence);
            Assert.AreEqual(true, pass.ActiveFlag);
        }
    }
}
