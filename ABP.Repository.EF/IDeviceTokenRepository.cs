using ABP.Application.Models;
using ABP.Application.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABP.Repository
{
    public interface IDeviceTokenRepository
    {
        Task<IEnumerable<DeviceToken>> GetDeviceTokenListAsync();
        Task<DeviceToken> GetDeviceTokenByIdAsync(int id);
        Task<List<DeviceToken>> GetDeviceTokenByDeviceIdAsync(int deviceId);
        Task<DeviceToken> GetDeviceTokenByRequestAsync(DeviceTokenRequest request);
        Task<bool> Create(DeviceToken token);
        Task UpdateAsync(DeviceToken device);
        Task DeleteAsync(int id);
    }
}
