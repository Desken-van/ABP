using System;
using System.Threading.Tasks;
using ABP.AppCore.Enums;
using ABP.Application.Models;
using ABP.Infrastructure.Helpers;
using ABP.Repository.ContractImplementation.EFImplementations;

namespace ABP.Infrastructure.Services.EF.Implementation
{
    public class DeviceEFService : IDeviceEFService
    {
        private readonly IDeviceTokenEFRepository _repo_ef_devicetoken;
        private readonly IDeviceEFRepository _repo_ef_device;

        public DeviceEFService(IDeviceTokenEFRepository repo_ef_devicetoken, IDeviceEFRepository repo_ef_device)
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
