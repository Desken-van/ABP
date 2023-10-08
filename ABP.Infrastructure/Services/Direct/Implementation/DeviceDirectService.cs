using ABP.AppCore.Enums;
using ABP.Application.Models;
using ABP.Infrastructure.Helpers;
using System;
using System.Threading.Tasks;
using ABP.Repository.ContractImplementation.DirectImplementations;

namespace ABP.Infrastructure.Services.Direct.Implementation
{
    public class DeviceDirectService : IDeviceDirectService
    {
        private readonly IDeviceTokenDirectRepository _repo_ef_devicetoken;
        private readonly IDeviceDirectRepository _repo_ef_device;

        public DeviceDirectService(IDeviceTokenDirectRepository repo_ef_devicetoken, IDeviceDirectRepository repo_ef_device)
        {
            _repo_ef_devicetoken = repo_ef_devicetoken;
            _repo_ef_device = repo_ef_device;
        }

        public async Task<bool> AddDeviceAsync(string deviceName)
        {
            var device = new Device()
            {
                DeviceName = deviceName
            };

            var checkDevice = await _repo_ef_device.Create(device);

            if (checkDevice)
            {
                device = await _repo_ef_device.GetDeviceByNameAsync(device.DeviceName);

                foreach (var type in Enum.GetValues(typeof(ExperimentType)))
                {
                    var key = device.DeviceName + type;

                    var hashedKey = TokenCreator.HashPassword(key);

                    var token = new DeviceToken()
                    {
                        DeviceId = device.Id,
                        Name = type.ToString(),
                        Value = hashedKey,
                        Expired = false
                    };

                    await _repo_ef_devicetoken.Create(token);
                }

                return true;
            }

            return false;
        }
    }
}
