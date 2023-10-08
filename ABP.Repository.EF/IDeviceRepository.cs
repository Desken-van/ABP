using System.Collections.Generic;
using System.Threading.Tasks;
using ABP.Application.Models;

namespace ABP.Repository
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<Device>> GetDeviceListAsync();
        Task<Device> GetDeviceByIdAsync(int id);
        Task<Device> GetDeviceByNameAsync(string name);
        Task<bool> Create(Device device);
        Task UpdateAsync(Device device);
        Task DeleteAsync(int id);
    }
}
