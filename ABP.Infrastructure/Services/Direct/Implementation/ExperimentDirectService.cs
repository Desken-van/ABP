using ABP.AppCore.Enums;
using ABP.Application.Models.Requests;
using ABP.Application.Models;
using ABP.Infrastructure.Factory.Base;
using ABP.Infrastructure.Helpers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABP.Repository.ContractImplementation.DirectImplementations;
using Microsoft.EntityFrameworkCore;
using ABP.Application.Models.Table;

namespace ABP.Infrastructure.Services.Direct.Implementation
{
    public class ExperimentDirectService : IExperimentDirectService
    {
        private readonly IDeviceTokenDirectRepository _repo_ef_devicetoken;
        private readonly IDeviceDirectRepository _repo_ef_device;
        private readonly IExperimentDirectRepository _repo_ef_experiment;
        private readonly IMapper _mapper;

        public ExperimentDirectService(IDeviceTokenDirectRepository repo_ef_devicetoken, IDeviceDirectRepository repo_ef_device, IExperimentDirectRepository repo_ef_experiment, IMapper mapper)
        {
            _repo_ef_devicetoken = repo_ef_devicetoken;
            _repo_ef_device = repo_ef_device;
            _repo_ef_experiment = repo_ef_experiment;
            _mapper = mapper;
        }

        public async Task<TableData> GetTableDataAsync(ExperimentType type)
        {
            var experimentList = await _repo_ef_experiment.GetExperimentListAsync();

            var deviceIdList = (from elem in experimentList
                where elem.ExperimentType == type && elem.ForRegistered == true
                select elem.DeviceId).Distinct();

            var deviceList = new List<Device>();

            foreach (var id in deviceIdList)
            {
                var device = await _repo_ef_device.GetDeviceByIdAsync(id);

                deviceList.Add(device);
            }

            var deviceDataList = new List<DeviceData>();

            foreach (var device in deviceList)
            {
                var experiment = experimentList.FirstOrDefault(x => x.ExperimentType == type && x.DeviceId == device.Id);

                deviceDataList.Add(new DeviceData()
                {
                    Device = device,
                    Experiment = experiment
                });
            }

            var experimentator = ExperimentFactory.CreateExperiment(type);

            var table = experimentator.GetOptionData(deviceDataList);

            return table;
        }

        public async Task<bool> MakeExperimentAsync(DeviceToken request)
        {
            if (request != null)
            {
                var device = await _repo_ef_device.GetDeviceByIdAsync(request.DeviceId);

                if (device != null)
                {
                    var token = await _repo_ef_devicetoken.GetDeviceTokenByRequestAsync(
                        _mapper.Map<DeviceTokenRequest>(request));

                    var hashKey = TokenCreator.VerifyHashedPassword(request.Value, device.DeviceName + token.Name);

                    if (hashKey)
                    {
                        ExperimentType type;

                        Enum.TryParse<ExperimentType>(token.Name, out type);

                        var experimentator = ExperimentFactory.CreateExperiment(type);

                        var experimentsResults = await _repo_ef_experiment.GetExperimentsByDeviceIdAsync(device.Id);

                        var registerResult = experimentsResults.FirstOrDefault(x => x.ExperimentType == type);

                        var value = string.Empty;

                        if (registerResult != null)
                        {
                            value = experimentator.Calculate(registerResult.Value);
                        }
                        else
                        {
                            value = experimentator.Calculate(null);
                        }

                        if (value != null)
                        {
                            var experiment = new Experiment()
                            {
                                DeviceId = device.Id,
                                ExperimentType = type,
                                KeyValue = token.Name,
                                Value = value
                            };

                            var response = await _repo_ef_experiment.Create(experiment);

                            experiment =
                                await _repo_ef_experiment.GetExperimentByRequest(_mapper.Map<ExperimentRequest>(experiment));

                            if (!token.Expired && experiment != null && response)
                            {
                                experiment.ForRegistered = true;

                                await _repo_ef_experiment.UpdateAsync(experiment);

                                token.Expired = true;

                                await _repo_ef_devicetoken.UpdateAsync(token);

                            }

                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
