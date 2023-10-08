using ABP.Application.Models;
using ABP.Application.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABP.Repository
{
    public interface IExperimentRepository
    {
        Task<IEnumerable<Experiment>> GetExperimentListAsync();
        Task<Experiment> GetExperimentByIdAsync(int id);
        Task<List<Experiment>> GetExperimentsByDeviceIdAsync(int deviceId);
        Task<Experiment> GetExperimentByRequest(ExperimentRequest request);
        Task<bool> Create(Experiment experiment);
        Task UpdateAsync(Experiment experiment);
        Task DeleteAsync(int id);
    }
}
