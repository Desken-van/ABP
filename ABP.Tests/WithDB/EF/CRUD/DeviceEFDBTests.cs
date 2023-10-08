using ABP.AppCore.Entities;
using ABP.Application.Models;
using ABP.Repository.ContractImplementation.EFImplementations;
using ABP.Tests.DataOperator;
using ABP.Tests.Settings;
using ABP_Server.Controllers.EF.CRUD;
using ABP_Server.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using ABP.Repository.Repository.EFRepositories;

namespace ABP.Tests.WithDB.EF.CRUD
{
    public class DeviceEFDBTests
    {
        private IMapper _mapper;

        private IDeviceEFRepository _device_repo;

        private DeviceCRUD_EFController _controller;

        [SetUp]
        public void Setup()
        {
            var dbContext = DBStore.GetDatabaseContext().Result;

            _mapper = MapperConfig.InitializeAutomapper();

            _device_repo = new DeviceEFRepository(dbContext, _mapper);

            _controller = new DeviceCRUD_EFController(_device_repo, _mapper);
        }

        [Test]
        public void GetDeviceListTest()
        {
            // Arrange
            var expected = DeviceDataOperator.MapDeviceList(DBStore.GetDeviceEntityList()).AsEnumerable();

            // Act
            var response = (_controller.GetDeviceList().Result.Result as OkObjectResult);

            var result = response.Value as IEnumerable<Device>;

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(result);
            Assert.AreEqual(expected.Count(), result.Count());

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.AreEqual(expected.ToArray()[i].Id, result.ToArray()[i].Id);
                Assert.AreEqual(expected.ToArray()[i].DeviceName, result.ToArray()[i].DeviceName);
            }

        }

        [Test]
        public void GetDeviceByIdTest()
        {
            // Arrange
            var id = 1;

            var expected = DeviceDataOperator.GetDeviceById(id);

            // Act
            var response = (_controller.GetDeviceById(id).Result.Result as OkObjectResult);

            var result = response.Value as Device;

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(result);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.DeviceName, result.DeviceName);
        }

        [Test]
        public void GetDeviceByNameTest()
        {
            // Arrange
            var name = "Device1";

            var expected = DeviceDataOperator.GetDeviceByName(name);

            // Act
            var response = (_controller.GetDeviceByName(name).Result.Result as OkObjectResult);

            var result = response.Value as Device;

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(result);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.DeviceName, result.DeviceName);
        }

        [Test]
        public void CreateDeviceTest()
        {
            // Arrange
            var device = new Device()
            {
                DeviceName = "Device4"
            };

            var resultList = new List<DeviceEntity>();

            // Act
            var check = DeviceDataOperator.CreateDevice(device, ref resultList);

            _controller.CreateDevice(_mapper.Map<DeviceRequest>(device));

            var result = resultList.FirstOrDefault(x => x.DeviceName == device.DeviceName);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(device.DeviceName, result.DeviceName);
            Assert.NotNull(result.Id);
            Assert.IsTrue(check);
        }

        [Test]
        public void UpdateDeviceTest()
        {
            // Arrange
            var device = new Device()
            {
                Id = 2,
                DeviceName = "Dev"
            };

            // Act
            _controller.UpdateDevice((device));

            var result = _device_repo.GetDeviceListAsync().Result.FirstOrDefault(x => x.Id == device.Id);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(device.DeviceName, result.DeviceName);
            Assert.NotNull(result.Id);
        }
    }
}
