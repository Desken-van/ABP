using ABP.AppCore.Entities;
using ABP.Application.Models;
using ABP.Application.Models.Requests;
using ABP.Repository.ContractImplementation.DirectImplementations;
using ABP.Repository.ContractImplementation.EFImplementations;
using ABP.Tests.Settings;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABP.Tests.DataOperator
{
    public static class ExperimentDataOperator
    {
        public static Mock<IExperimentEFRepository> CreateMockEFRepo()
        {
            var repo = new Mock<IExperimentEFRepository>();

            var _tokenEntities = DBStore.GetExperimentEntityList();

            repo.Setup(_token_repo => _token_repo.GetExperimentListAsync().Result)
                .Returns(MapExperimentList(_tokenEntities));

            return repo;
        }

        public static Mock<IExperimentDirectRepository> CreateMockDirectRepo()
        {
            var repo = new Mock<IExperimentDirectRepository>();

            var _tokenEntities = DBStore.GetExperimentEntityList();

            repo.Setup(_token_repo => _token_repo.GetExperimentListAsync().Result)
                .Returns(MapExperimentList(_tokenEntities));

            return repo;
        }

        public static Experiment GetExperimentById(int id)
        {
            var experiment = DBStore.GetExperimentEntityList().FirstOrDefault(e => e.Id == id);

            if (experiment != null)
            {
                var result = new Experiment
                {
                    Id = experiment.Id,
                    DeviceId = experiment.DeviceId,
                    KeyValue = experiment.KeyValue,
                    Value = experiment.Value,
                    ForRegistered = experiment.ForRegistered
                };

                return result;
            }

            return null;
        }

        public static List<Experiment> GetExperimentByDeviceId(int id)
        {
            var experiment = DBStore.GetExperimentEntityList().Where(e => e.DeviceId == id);

            var list = new List<Experiment>();

            if (experiment.Any())
            {
                foreach (var item in experiment)
                {
                    var result = new Experiment()
                    {
                        Id = item.Id,
                        DeviceId = item.DeviceId,
                        KeyValue = item.KeyValue,
                        Value = item.Value,
                        ForRegistered = item.ForRegistered
                    };

                    list.Add(result);
                }
            }

            return list;
        }

        public static Experiment GetExperimentByRequest(ExperimentRequest request)
        {
            var experiment = DBStore.GetExperimentEntityList().FirstOrDefault(x => x.DeviceId == request.DeviceId && x.KeyValue == request.KeyValue && x.Value == request.Value);

            if (experiment != null)
            {
                var result = new Experiment()
                {
                    Id = experiment.Id,
                    DeviceId = experiment.DeviceId,
                    KeyValue = experiment.KeyValue,
                    Value = experiment.Value,
                    ForRegistered = experiment.ForRegistered
                };  

                return result;
            }

            return null;
        }

        public static bool CreateExperiment(Experiment experiment, ref List<ExperimentEntity> result)
        {
            var list = DBStore.GetExperimentEntityList();

            var id = list.LastOrDefault().Id + 1;

            var check = list.FirstOrDefault(x => x.Id == id);

            if (check == null)
            {
                var entity = new ExperimentEntity()
                {
                    Id = experiment.Id,
                    DeviceId = experiment.DeviceId,
                    KeyValue = experiment.KeyValue,
                    Value = experiment.Value,
                    ForRegistered = experiment.ForRegistered
                };

                list.Add(entity);

                result = list;

                return true;
            }

            return false;
        }

        public static Task UpdateExperiment(Experiment experiment, ref List<ExperimentEntity> result)
        {
            var list = DBStore.GetExperimentEntityList();

            var entity = list.FirstOrDefault(x => x.Id == experiment.Id);

            if (entity != null)
            {
                entity.DeviceId = experiment.DeviceId;
                entity.KeyValue = experiment.KeyValue;
                entity.Value = experiment.Value;
                entity.ForRegistered = experiment.ForRegistered;
            }

            result = list;

            return Task.CompletedTask;
        }

        public static Task DeleteExperiment(int id, ref List<ExperimentEntity> result)
        {
            var list = DBStore.GetExperimentEntityList();

            var entity = list.FirstOrDefault(x => x.Id == id);

            if (entity != null)
            {
                list.Remove(entity);
            }

            result = list;

            return Task.CompletedTask;
        }


        public static List<Experiment> MapExperimentList(List<ExperimentEntity> list)
        {
            var result = new List<Experiment>();

            foreach (var experiment in list)
            {
                var elem = new Experiment
                {
                    Id = experiment.Id,
                    DeviceId = experiment.DeviceId,
                    KeyValue = experiment.KeyValue,
                    Value = experiment.Value,
                    ForRegistered = experiment.ForRegistered,
                };

                result.Add(elem);
            }

            return result;
        }
    }
}
