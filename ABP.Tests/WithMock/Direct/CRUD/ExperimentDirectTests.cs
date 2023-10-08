using ABP.AppCore.Entities;
using ABP.AppCore.Enums;
using ABP.Application.Models.Requests;
using ABP.Application.Models;
using ABP.Tests.DataOperator;
using ABP.Tests.Settings;
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
    public class ExperimentDirectTests
    {
        private IMapper _mapper;

        private Mock<IExperimentDirectRepository> _repo;

        private ExperimentCRUD_DirectController _controller;

        [SetUp]
        public void Setup()
        {
            _repo = ExperimentDataOperator.CreateMockDirectRepo();

            _mapper = MapperConfig.InitializeAutomapper();

            _controller = new ExperimentCRUD_DirectController(_repo.Object, _mapper);
        }

        [Test]
        public void GetExperimentListTest()
        {
            // Arrange
            var expected = ExperimentDataOperator.MapExperimentList(DBStore.GetExperimentEntityList()).AsEnumerable();

            // Act
            var response = (_controller.GetExperimentList().Result.Result as OkObjectResult);

            var result = response.Value as IEnumerable<Experiment>;

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(result);
            Assert.AreEqual(expected.Count(), result.Count());

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.AreEqual(expected.ToArray()[i].Id, result.ToArray()[i].Id);
                Assert.AreEqual(expected.ToArray()[i].KeyValue, result.ToArray()[i].KeyValue);
                Assert.AreEqual(expected.ToArray()[i].DeviceId, result.ToArray()[i].DeviceId);
                Assert.AreEqual(expected.ToArray()[i].Value, result.ToArray()[i].Value);
                Assert.AreEqual(expected.ToArray()[i].ForRegistered, result.ToArray()[i].ForRegistered);
            }

        }

        [Test]
        public void GetExperimentByIdTest()
        {
            // Arrange
            var id = 1;

            var expected = ExperimentDataOperator.GetExperimentById(id);

            _repo.Setup(_token_repo => _token_repo.GetExperimentByIdAsync(id).Result)
                .Returns(ExperimentDataOperator.GetExperimentById(id));

            // Act
            var response = (_controller.GetExperimentById(id).Result.Result as OkObjectResult);

            var result = response.Value as Experiment;

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(result);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.KeyValue, result.KeyValue);
            Assert.AreEqual(expected.DeviceId, result.DeviceId);
            Assert.AreEqual(expected.Value, result.Value);
            Assert.AreEqual(expected.ForRegistered, result.ForRegistered);
        }

        [Test]
        public void GetExperimentByRequestTest()
        {
            // Arrange
            var request = new ExperimentRequest()
            {
                DeviceId = 1,
                KeyValue = "ButtonExperiment",
                Value = "#00FF00",
            };

            var expected = ExperimentDataOperator.GetExperimentByRequest(request);

            _repo.Setup(_token_repo => _token_repo.GetExperimentByRequest(request).Result)
                .Returns(ExperimentDataOperator.GetExperimentByRequest(request));

            // Act
            var response = (_controller.GetExperimentByRequest(request).Result.Result as OkObjectResult);

            var result = response.Value as Experiment;

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(result);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.KeyValue, result.KeyValue);
            Assert.AreEqual(expected.DeviceId, result.DeviceId);
            Assert.AreEqual(expected.Value, result.Value);
            Assert.AreEqual(expected.ForRegistered, result.ForRegistered);
        }

        [Test]
        public void GetExperimentByDeviceIdTest()
        {
            // Arrange
            var id = 1;

            var expected = ExperimentDataOperator.GetExperimentByDeviceId(id);

            _repo.Setup(_token_repo => _token_repo.GetExperimentsByDeviceIdAsync(id).Result)
                .Returns(ExperimentDataOperator.GetExperimentByDeviceId(id));

            // Act
            var response = (_controller.GetExperimentByDeviceId(id).Result.Result as OkObjectResult);

            var result = response.Value as List<Experiment>; ;

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(result);

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.AreEqual(expected.ToArray()[i].Id, result.ToArray()[i].Id);
                Assert.AreEqual(expected.ToArray()[i].KeyValue, result.ToArray()[i].KeyValue);
                Assert.AreEqual(expected.ToArray()[i].DeviceId, result.ToArray()[i].DeviceId);
                Assert.AreEqual(expected.ToArray()[i].Value, result.ToArray()[i].Value);
                Assert.AreEqual(expected.ToArray()[i].ForRegistered, result.ToArray()[i].ForRegistered);
            }
        }

        [Test]
        public void CreateExperimentTest()
        {
            // Arrange
            var experiment = new Experiment()
            {
                DeviceId = 10,
                KeyValue = "ButtonExperiment",
                Value = "#00FF00",
                ForRegistered = true,
                ExperimentType = ExperimentType.PriceExperiment
            };

            var list = ExperimentDataOperator.MapExperimentList(DBStore.GetExperimentEntityList());

            var resultList = new List<ExperimentEntity>();

            _repo.Setup(_token_repo => _token_repo.Create(experiment).Result)
                .Returns(true);

            // Act
            var check = ExperimentDataOperator.CreateExperiment(experiment, ref resultList);

            _controller.CreateExperiment(_mapper.Map<ExperimentRequest>(experiment));

            var result = resultList.FirstOrDefault(x => x.DeviceId == experiment.DeviceId && x.KeyValue == experiment.KeyValue && x.Value == experiment.Value);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.IsTrue(check);
            Assert.AreEqual(experiment.Id, result.Id);
            Assert.AreEqual(experiment.KeyValue, result.KeyValue);
            Assert.AreEqual(experiment.DeviceId, result.DeviceId);
            Assert.AreEqual(experiment.Value, result.Value);
            Assert.AreEqual(experiment.ForRegistered, result.ForRegistered);
        }

        [Test]
        public void UpdateExperimentTest()
        {
            // Arrange
            var experiment = new Experiment()
            {
                Id = 1,
                DeviceId = 10,
                KeyValue = "ButtonExperiment",
                Value = "#00FF00",
                ForRegistered = true,
                ExperimentType = ExperimentType.PriceExperiment
            };

            var list = ExperimentDataOperator.MapExperimentList(DBStore.GetExperimentEntityList());

            var resultList = new List<ExperimentEntity>();

            _repo.Setup(_token_repo => _token_repo.UpdateAsync(experiment))
                .Returns(ExperimentDataOperator.UpdateExperiment(experiment, ref resultList));

            // Act
            _controller.UpdateExperiment((experiment));

            var result = resultList.FirstOrDefault(x => x.Id == experiment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.AreEqual(experiment.Id, result.Id);
            Assert.AreEqual(experiment.KeyValue, result.KeyValue);
            Assert.AreEqual(experiment.DeviceId, result.DeviceId);
            Assert.AreEqual(experiment.Value, result.Value);
            Assert.AreEqual(experiment.KeyValue, result.KeyValue);
        }

        [Test]
        public void DeleteExperimentTest()
        {
            // Arrange
            var id = 3;

            var list = ExperimentDataOperator.MapExperimentList(DBStore.GetExperimentEntityList());

            var resultList = new List<ExperimentEntity>();

            _repo.Setup(_token_repo => _token_repo.DeleteAsync(id))
                .Returns(ExperimentDataOperator.DeleteExperiment(id, ref resultList));

            // Act
            _controller.DeleteExperimentByID((id));

            var result = resultList.FirstOrDefault(x => x.Id == id);

            // Assert
            Assert.Null(result);
            Assert.AreEqual(5, resultList.Count);
        }
    }
}
