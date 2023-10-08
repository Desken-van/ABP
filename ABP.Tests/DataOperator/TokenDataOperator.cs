using ABP.AppCore.Entities;
using ABP.Application.Models;
using ABP.Repository.ContractImplementation.EFImplementations;
using ABP.Tests.Settings;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABP.Application.Models.Requests;
using ABP.Repository.ContractImplementation.DirectImplementations;

namespace ABP.Tests.DataOperator
{
    public static class TokenDataOperator
    {
        public static Mock<IDeviceTokenEFRepository> CreateMockEFRepo()
        {
            var repo = new Mock<IDeviceTokenEFRepository>();

            var _tokenEntities = DBStore.GetDeviceTokenEntityList();

            repo.Setup(_token_repo => _token_repo.GetDeviceTokenListAsync().Result)
                .Returns(MapDeviceTokenList(_tokenEntities));

            return repo;
        }

        public static Mock<IDeviceTokenDirectRepository> CreateMockDirectRepo()
        {
            var repo = new Mock<IDeviceTokenDirectRepository>();

            var _tokenEntities = DBStore.GetDeviceTokenEntityList();

            repo.Setup(_token_repo => _token_repo.GetDeviceTokenListAsync().Result)
                .Returns(MapDeviceTokenList(_tokenEntities));

            return repo;
        }

        public static DeviceToken GetDeviceTokenById(int id)
        {
            var token = DBStore.GetDeviceTokenEntityList().FirstOrDefault(e => e.Id == id);

            if (token != null) 
            {
                var result = new DeviceToken
                {
                    Id = token.Id,
                    DeviceId = token.DeviceId,
                    Name = token.Name,
                    Value = token.Value,
                    Expired = token.Expired
                };

                return result;
            }

            return null;
        }

        public static List<DeviceToken> GetDeviceTokenByDeviceId(int id)
        {
            var token = DBStore.GetDeviceTokenEntityList().Where(e => e.DeviceId == id);

            var list = new List<DeviceToken>();

            if (token.Any())
            {
                foreach (var item in token)
                {
                    var result = new DeviceToken
                    {
                        Id = item.Id,
                        DeviceId = item.DeviceId,
                        Name = item.Name,
                        Value = item.Value,
                        Expired = item.Expired
                    };

                    list.Add(result);
                }
            }

            return list;
        }

        public static DeviceToken GetDeviceTokenByRequest(DeviceTokenRequest request)
        {
            var token = DBStore.GetDeviceTokenEntityList().FirstOrDefault(x => x.DeviceId == request.DeviceId && x.Name == request.Name && x.Value == request.Value);

            if (token != null)
            {
                var result = new DeviceToken
                {
                    Id = token.Id,
                    DeviceId = token.DeviceId,
                    Name = token.Name,
                    Value = token.Value,
                    Expired = token.Expired
                };

                return result;
            }

            return null;
        }

        public static bool CreateToken(DeviceToken token, ref List<DeviceTokenEntity> result)
        {
            var list = DBStore.GetDeviceTokenEntityList();

            var id = list.LastOrDefault().Id + 1;

            var check = list.FirstOrDefault(x => x.Id == id);

            if (check == null)
            {
                var entity = new DeviceTokenEntity()
                {
                    Id = token.Id,
                    DeviceId = token.DeviceId,
                    Name = token.Name,
                    Value = token.Value,
                    Expired = token.Expired
                };

                list.Add(entity);

                result = list;

                return true;
            }

            return false;
        }

        public static Task UpdateToken(DeviceToken token, ref List<DeviceTokenEntity> result)
        {
            var list = DBStore.GetDeviceTokenEntityList();

            var entity = list.FirstOrDefault(x => x.Id == token.Id);

            if (entity != null)
            {
                entity.DeviceId = token.DeviceId;
                entity.Name = token.Name;
                entity.Value = token.Value;
                entity.Expired = token.Expired;
            }

            result = list;

            return Task.CompletedTask;
        }

        public static Task DeleteToken(int id, ref List<DeviceTokenEntity> result)
        {
            var list = DBStore.GetDeviceTokenEntityList();

            var entity = list.FirstOrDefault(x => x.Id == id);

            if (entity != null)
            {
                list.Remove(entity);
            }

            result = list;

            return Task.CompletedTask;
        }

        public static List<DeviceToken> MapDeviceTokenList(List<DeviceTokenEntity> list)
        {
            var result = new List<DeviceToken>();

            foreach (var token in list)
            {
                var elem = new DeviceToken
                {
                    Id = token.Id,
                    DeviceId = token.DeviceId,
                    Name = token.Name,
                    Value = token.Value,
                    Expired = token.Expired
                };

                result.Add(elem);
            }

            return result;
        }
    }
}
