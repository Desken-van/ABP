using ABP.AppCore.Entities;
using ABP.Application.Models;
using ABP.Repository.ContractImplementation.EFImplementations;
using ABP.Tests.DataOperator;
using ABP.Tests.Settings;
using ABP_Server.Controllers.EF.CRUD;
using ABP_Server.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using ABP.Repository.ContractImplementation.DirectImplementations;
using ABP_Server.Controllers.Direct.CRUD;

namespace ABP.Tests.WithMock.Direct.CRUD
{
    public class DeviceDirectTests
    {
        private IMapper _mapper;

        private Mock<IDeviceDirectRepository> _device_repo;

        private DeviceCRUD_DirectController _controller;

        [SetUp]
        public void Setup()
        {
            _device_repo = DeviceDataOperator.CreateMockDirectRepo();

            _mapper = MapperConfig.InitializeAutomapper();

            _controller = new DeviceCRUD_DirectController(_device_repo.Object, _mapper);
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

            _device_repo.Setup(_device_repo => _device_repo.GetDeviceByIdAsync(id).Result)
                .Returns(DeviceDataOperator.GetDeviceById(id));

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

            _device_repo.Setup(_device_repo => _device_repo.GetDeviceByNameAsync(name).Result)
                .Returns(DeviceDataOperator.GetDeviceByName(name));

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

            var list = DeviceDataOperator.MapDeviceList(DBStore.GetDeviceEntityList());

            var resultList = new List<DeviceEntity>();

            _device_repo.Setup(_device_repo => _device_repo.Create(device).Result)
                .Returns(true);

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

            var list = DeviceDataOperator.MapDeviceList(DBStore.GetDeviceEntityList());

            var resultList = new List<DeviceEntity>();

            _device_repo.Setup(_device_repo => _device_repo.UpdateAsync(device))
                .Returns(DeviceDataOperator.UpdateDevice(device, ref resultList));

            // Act
            _controller.UpdateDevice((device));

            var result = resultList.FirstOrDefault(x => x.Id == device.Id);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(device.DeviceName, result.DeviceName);
            Assert.NotNull(result.Id);
        }

        [Test]
        public void DeleteDeviceTest()
        {
            // Arrange
            var id = 3;

            var list = DeviceDataOperator.MapDeviceList(DBStore.GetDeviceEntityList());

            var resultList = new List<DeviceEntity>();

            _device_repo.Setup(_device_repo => _device_repo.DeleteAsync(id))
                .Returns(DeviceDataOperator.DeleteDevice(id, ref resultList));

            // Act
            _controller.DeleteDeviceByID((id));

            var result = resultList.FirstOrDefault(x => x.Id == id);

            // Assert
            Assert.Null(result);
            Assert.AreEqual(2, resultList.Count);
        }
    }
}
