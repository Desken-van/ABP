using ABP.AppCore;
using ABP.Application.Models;
using ABP.Repository.ContractImplementation.EFImplementations;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ABP.AppCore.Entities;
using ABP.Application.Models.Requests;

namespace ABP.Repository.Repository.EFRepositories
{
    public class ExperimentEFRepository : IExperimentEFRepository
    {
        private readonly ABPContext _dbContext;

        private readonly IMapper _mapper;

        public ExperimentEFRepository(ABPContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Experiment>> GetExperimentListAsync()
        {
            var experimentList = await _dbContext.Experiments.ToListAsync();

            if (experimentList != null)
            {
                var result = new List<Experiment>();

                foreach (var experiment in experimentList)
                {
                    result.Add(_mapper.Map<Experiment>(experiment));
                }

                return result;
            }

            return null;
        }

        public async Task<Experiment> GetExperimentByIdAsync(int id)
        {
            var experiment = await _dbContext.Experiments.FirstOrDefaultAsync(x => x.Id == id);

            if (experiment != null)
            {
                var result = _mapper.Map<Experiment>(experiment);

                return result;
            }

            return null;
        }

        public async Task<List<Experiment>> GetExperimentsByDeviceIdAsync(int deviceId)
        {
            var list = await _dbContext.Experiments.ToListAsync();

            var entityList = list.Where(x => x.DeviceId == deviceId);

            if (entityList != null)
            {
                var result = new List<Experiment>();

                foreach (var experiment in entityList)
                {
                    if (experiment.ForRegistered)
                    {
                        result.Add(_mapper.Map<Experiment>(experiment));
                    }
                }

                return result;
            }

            return null;
        }

        public async Task<Experiment> GetExperimentByRequest(ExperimentRequest request)
        {
            var experiment = await _dbContext.Experiments.FirstOrDefaultAsync(x => x.DeviceId == request.DeviceId && x.KeyValue == request.KeyValue && x.Value == request.Value);

            if (experiment != null)
            {
                var result = _mapper.Map<Experiment>(experiment);

                return result;
            }

            return null;
        }

        public async Task<bool> Create(Experiment experiment)
        {
            if (experiment != null)
            {
                var result = _mapper.Map<ExperimentEntity>(experiment);

                result.ForRegistered = false;

                await _dbContext.Experiments.AddAsync(result);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task UpdateAsync(Experiment experiment)
        {
            var original = await _dbContext.Experiments.FindAsync(experiment.Id);

            if (original != null)
            {
                _dbContext.Entry(original).CurrentValues.SetValues(experiment);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var experiment = await _dbContext.Devices.FindAsync(id);

            if (experiment != null)
                await Task.Run(() => _dbContext.Devices.Remove(experiment));

            await _dbContext.SaveChangesAsync();
        }
    }
}
