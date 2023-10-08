using ABP.AppCore.Entities;
using ABP.Application.Models;
using ABP.Repository.ContractImplementation.EFImplementations;
using ABP.Tests.Settings;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABP.Repository.ContractImplementation.DirectImplementations;

namespace ABP.Tests.DataOperator
{
    public static class DeviceDataOperator
    {
        public static Mock<IDeviceEFRepository> CreateMockEFRepo()
        {
            var repo = new Mock<IDeviceEFRepository>();

            var _deviceEntities = DBStore.GetDeviceEntityList();

            repo.Setup(_device_repo => _device_repo.GetDeviceListAsync().Result)
                .Returns(MapDeviceList(_deviceEntities));

            return repo;
        }

        public static Mock<IDeviceDirectRepository> CreateMockDirectRepo()
        {
            var repo = new Mock<IDeviceDirectRepository>();

            var _deviceEntities = DBStore.GetDeviceEntityList();

            repo.Setup(_device_repo => _device_repo.GetDeviceListAsync().Result)
                .Returns(MapDeviceList(_deviceEntities));

            return repo;
        }

        public static Device GetDeviceById(int id)
        {
            var device = DBStore.GetDeviceEntityList().FirstOrDefault(e => e.Id == id);

            if (device != null)
            {
                var result = new Device
                {
                    Id = device.Id,
                    DeviceName = device.DeviceName,
                };

                return result;
            }
            
            return null;
        }

        public static Device GetDeviceByName(string name)
        {
            var device = DBStore.GetDeviceEntityList().FirstOrDefault(e => e.DeviceName == name);

            if (device != null)
            {
                var result = new Device
                {
                    Id = device.Id,
                    DeviceName = device.DeviceName,
                };

                return result;
            }
            
            return null;
        }

        public static bool CreateDevice(Device device, ref List<DeviceEntity> result)
        {
            var list = DBStore.GetDeviceEntityList();

            var id = list.LastOrDefault().Id + 1;

            var check = list.FirstOrDefault(x => x.Id == id);

            if (check == null)
            {
                var entity = new DeviceEntity()
                {
                    Id = id,
                    DeviceName = device.DeviceName
                };

                list.Add(entity);

                result = list;

                return true;
            }

            return false;
        }

        public static Task UpdateDevice(Device device, ref List<DeviceEntity> result)
        {
            var list = DBStore.GetDeviceEntityList();

            var entity = list.FirstOrDefault(x => x.Id == device.Id);

            if (entity != null)
            {
                entity.DeviceName = device.DeviceName;
            }

            result = list;

            return Task.CompletedTask;
        }

        public static Task DeleteDevice(int id, ref List<DeviceEntity> result)
        {
            var list = DBStore.GetDeviceEntityList();

            var entity = list.FirstOrDefault(x => x.Id == id);

            if (entity != null)
            {
                list.Remove(entity);
            }

            result = list;

            return Task.CompletedTask;
        }

        public static List<Device> MapDeviceList(List<DeviceEntity> list)
        {
            var result = new List<Device>();

            foreach (var device in list)
            {
                var elem = new Device
                {
                    Id = device.Id,
                    DeviceName = device.DeviceName,
                };

                result.Add(elem);
            }

            return result;
        }
    }
}
