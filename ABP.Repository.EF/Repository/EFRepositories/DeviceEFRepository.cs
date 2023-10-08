using ABP.AppCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABP.AppCore.Entities;
using ABP.Application.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ABP.Repository.ContractImplementation.EFImplementations;

namespace ABP.Repository.Repository.EFRepositories
{
    public class DeviceEFRepository : IDeviceEFRepository
    {
        private readonly ABPContext _dbContext;

        private readonly IMapper _mapper;

        public DeviceEFRepository(ABPContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Device>> GetDeviceListAsync()
        {
            var deviceList = await _dbContext.Devices.ToListAsync();

            if (deviceList != null)
            {
                var result = new List<Device>();

                foreach (var device in deviceList)
                {
                    result.Add(_mapper.Map<Device>(device));
                }

                return result;
            }

            return null;
        }

        public async Task<Device> GetDeviceByIdAsync(int id)
        {
            var device = await _dbContext.Devices.FirstOrDefaultAsync(x => x.Id == id);

            if (device != null)
            {
                var result = _mapper.Map<Device>(device);

                return result;
            }

            return null;
        }

        public async Task<Device> GetDeviceByNameAsync(string name)
        {
            var device = await _dbContext.Devices.FirstOrDefaultAsync(x => x.DeviceName == name);

            if (device != null)
            {
                var result = _mapper.Map<Device>(device);

                return result;
            }

            return null;
        }

        public async Task<bool> Create(Device device)
        {
            if (device != null)
            {
                var list = await GetDeviceListAsync();

                var checkCopy = list.Any(d => d.DeviceName == device.DeviceName);

                if(!checkCopy)
                {
                    var result = _mapper.Map<DeviceEntity>(device);
                    await _dbContext.Devices.AddAsync(result);
                    await _dbContext.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        public async Task UpdateAsync(Device device)
        {
            var original = await _dbContext.Devices.FindAsync(device.Id);

            if (original != null)
            {
                _dbContext.Entry(original).CurrentValues.SetValues(device);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var device = await _dbContext.Devices.FindAsync(id);

            if (device != null)
                await Task.Run(() => _dbContext.Devices.Remove(device));

            await _dbContext.SaveChangesAsync();
        }
    }
}
