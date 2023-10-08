using System.Threading.Tasks;

namespace ABP.Infrastructure.Services
{
    public interface IDeviceService
    {
        Task<bool> AddDeviceAsync(string deviceName);
    }
}
