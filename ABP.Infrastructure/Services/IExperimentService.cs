using System.Threading.Tasks;
using ABP.AppCore.Enums;
using ABP.Application.Models;
using ABP.Application.Models.Table;

namespace ABP.Infrastructure.Services
{
    public interface IExperimentService
    {
        Task<bool> MakeExperimentAsync(DeviceToken token);
        Task<TableData> GetTableDataAsync(ExperimentType type);
    }
}
