using ABP.AppCore.Entities;
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
    public class TokenDirectTests
    {
        private IMapper _mapper;

        private Mock<IDeviceTokenDirectRepository> _repo;

        private DeviceTokenCRUD_DirectController _controller;

        [SetUp]
        public void Setup()
        {
            _repo = TokenDataOperator.CreateMockDirectRepo();

            _mapper = MapperConfig.InitializeAutomapper();

            _controller = new DeviceTokenCRUD_DirectController(_repo.Object, _mapper);
        }

        [Test]
        public void GetTokenListTest()
        {
            // Arrange
            var expected = TokenDataOperator.MapDeviceTokenList(DBStore.GetDeviceTokenEntityList()).AsEnumerable();

            // Act
            var response = (_controller.GetDeviceTokenList().Result.Result as OkObjectResult);

            var result = response.Value as IEnumerable<DeviceToken>;

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(result);
            Assert.AreEqual(expected.Count(), result.Count());

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.AreEqual(expected.ToArray()[i].Id, result.ToArray()[i].Id);
                Assert.AreEqual(expected.ToArray()[i].Name, result.ToArray()[i].Name);
                Assert.AreEqual(expected.ToArray()[i].DeviceId, result.ToArray()[i].DeviceId);
                Assert.AreEqual(expected.ToArray()[i].Value, result.ToArray()[i].Value);
                Assert.AreEqual(expected.ToArray()[i].Expired, result.ToArray()[i].Expired);
            }

        }

        [Test]
        public void GetTokenByIdTest()
        {
            // Arrange
            var id = 1;

            var expected = TokenDataOperator.GetDeviceTokenById(id);

            _repo.Setup(_token_repo => _token_repo.GetDeviceTokenByIdAsync(id).Result)
                .Returns(TokenDataOperator.GetDeviceTokenById(id));

            // Act
            var response = (_controller.GetDeviceTokenById(id).Result.Result as OkObjectResult);

            var result = response.Value as DeviceToken;

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(result);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.DeviceId, result.DeviceId);
            Assert.AreEqual(expected.Value, result.Value);
            Assert.AreEqual(expected.Expired, result.Expired);
        }

        [Test]
        public void GetTokenByRequestTest()
        {
            // Arrange
            var request = new DeviceTokenRequest()
            {
                DeviceId = 1,
                Name = "PriceExperiment",
                Value = "AFa0SxgQ7Iidu7cwCPm/Wr7VKBpAqYZsaCOb+R3UcyCXABDMcOnwbJi4QTR1uMOGDg==",
                Expired = false
            };

            var expected = TokenDataOperator.GetDeviceTokenByRequest(request);

            _repo.Setup(_token_repo => _token_repo.GetDeviceTokenByRequestAsync(request).Result)
                .Returns(TokenDataOperator.GetDeviceTokenByRequest(request));

            // Act
            var response = (_controller.GetDeviceTokenByRequest(request).Result.Result as OkObjectResult);

            var result = response.Value as DeviceToken;

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(result);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.DeviceId, result.DeviceId);
            Assert.AreEqual(expected.Value, result.Value);
            Assert.AreEqual(expected.Expired, result.Expired);
        }

        [Test]
        public void GetTokenByDeviceIdTest()
        {
            // Arrange
            var id = 1;

            var expected = TokenDataOperator.GetDeviceTokenByDeviceId(id);

            _repo.Setup(_token_repo => _token_repo.GetDeviceTokenByDeviceIdAsync(id).Result)
                .Returns(TokenDataOperator.GetDeviceTokenByDeviceId(id));

            // Act
            var response = (_controller.GetDeviceTokensByDeviceId(id).Result.Result as OkObjectResult);

            var result = response.Value as List<DeviceToken>; ;

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.NotNull(result);

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.AreEqual(expected.ToArray()[i].Id, result.ToArray()[i].Id);
                Assert.AreEqual(expected.ToArray()[i].Name, result.ToArray()[i].Name);
                Assert.AreEqual(expected.ToArray()[i].DeviceId, result.ToArray()[i].DeviceId);
                Assert.AreEqual(expected.ToArray()[i].Value, result.ToArray()[i].Value);
                Assert.AreEqual(expected.ToArray()[i].Expired, result.ToArray()[i].Expired);
            }
        }

        [Test]
        public void CreateTokenTest()
        {
            // Arrange
            var token = new DeviceToken()
            {
                DeviceId = 12,
                Name = "ButtonExperiment",
                Value = "ACbinsFVMi3tZUwV1h+CO0i8nnmGJlXWbhEST1XLhEdrf2ggwkocFYkKvtXVB+xSMw==",
                Expired = false
            };

            var list = TokenDataOperator.MapDeviceTokenList(DBStore.GetDeviceTokenEntityList());

            var resultList = new List<DeviceTokenEntity>();

            _repo.Setup(_token_repo => _token_repo.Create(token).Result)
                .Returns(true);

            // Act
            var check = TokenDataOperator.CreateToken(token, ref resultList);

            _controller.CreateDeviceToken(_mapper.Map<DeviceTokenRequest>(token));

            var result = resultList.FirstOrDefault(x => x.DeviceId == token.DeviceId && x.Name == token.Name && x.Value == token.Value);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.IsTrue(check);
            Assert.AreEqual(token.Id, result.Id);
            Assert.AreEqual(token.Name, result.Name);
            Assert.AreEqual(token.DeviceId, result.DeviceId);
            Assert.AreEqual(token.Value, result.Value);
            Assert.AreEqual(token.Expired, result.Expired);
        }

        [Test]
        public void UpdateTokenTest()
        {
            // Arrange
            var token = new DeviceToken()
            {
                Id = 1,
                DeviceId = 2,
                Name = "ButtonExperiment",
                Value = "ACbinsFVMi3tZUwV1h+CO0i8nnmGJlXWbhEST1XLhEdrf2ggwkocFYkKvtXVB+xSMw==",
                Expired = false
            };

            var list = TokenDataOperator.MapDeviceTokenList(DBStore.GetDeviceTokenEntityList());

            var resultList = new List<DeviceTokenEntity>();

            _repo.Setup(_token_repo => _token_repo.UpdateAsync(token))
                .Returns(TokenDataOperator.UpdateToken(token, ref resultList));

            // Act
            _controller.UpdateDeviceToken((token));

            var result = resultList.FirstOrDefault(x => x.Id == token.Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.AreEqual(token.Id, result.Id);
            Assert.AreEqual(token.Name, result.Name);
            Assert.AreEqual(token.DeviceId, result.DeviceId);
            Assert.AreEqual(token.Value, result.Value);
            Assert.AreEqual(token.Expired, result.Expired);
        }

        [Test]
        public void DeleteTokenTest()
        {
            // Arrange
            var id = 3;

            var list = TokenDataOperator.MapDeviceTokenList(DBStore.GetDeviceTokenEntityList());

            var resultList = new List<DeviceTokenEntity>();

            _repo.Setup(_token_repo => _token_repo.DeleteAsync(id))
                .Returns(TokenDataOperator.DeleteToken(id, ref resultList));

            // Act
            _controller.DeleteDeviceTokenByID((id));

            var result = resultList.FirstOrDefault(x => x.Id == id);

            // Assert
            Assert.Null(result);
            Assert.AreEqual(5, resultList.Count);
        }
    }
}
