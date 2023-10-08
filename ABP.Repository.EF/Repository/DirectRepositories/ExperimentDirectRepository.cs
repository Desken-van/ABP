using ABP.Application.Models.Requests;
using ABP.Application.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using ABP.AppCore.Enums;
using ABP.Repository.ContractImplementation.DirectImplementations;

namespace ABP.Repository.Repository.DirectRepositories
{
    public class ExperimentDirectRepository : IExperimentDirectRepository
    {
        private readonly IConfiguration configuration;
        private readonly IMapper _mapper;
        private readonly string connectionString;

        public ExperimentDirectRepository(IConfiguration config, IMapper mapper)
        {
            configuration = config;
            _mapper = mapper;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Experiment>> GetExperimentListAsync()
        {
            var result = new List<Experiment>();

            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = "SELECT * FROM Experiments";

            var command = new SqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var experiment = new Experiment()
                {
                    Id = (int)reader["Id"],
                    DeviceId = (int)reader["DeviceId"],
                    KeyValue = reader["KeyValue"].ToString(),
                    Value = reader["Value"].ToString(),
                    ForRegistered = Convert.ToBoolean(reader["ForRegistered"]),
                    ExperimentType = (ExperimentType)(int)reader["ExperimentType"]
                };

                result.Add(experiment);
            }

            return result.AsEnumerable();
        }

        public async Task<Experiment> GetExperimentByIdAsync(int id)
        {
            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = $"SELECT * FROM Experiments WHERE Id = {id}";

            var command = new SqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var experiment = new Experiment()
                {
                    Id = (int)reader["Id"],
                    DeviceId = (int)reader["DeviceId"],
                    KeyValue = reader["KeyValue"].ToString(),
                    Value = reader["Value"].ToString(),
                    ForRegistered = Convert.ToBoolean(reader["ForRegistered"]),
                    ExperimentType = (ExperimentType)(int)reader["ExperimentType"]
                };

                return experiment;
            }

            return null;
        }

        public async Task<List<Experiment>> GetExperimentsByDeviceIdAsync(int deviceId)
        {
            var result = new List<Experiment>();

            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = $"SELECT * FROM Experiments WHERE DeviceId = {deviceId} AND ForRegistered = 1";

            var command = new SqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var experiment = new Experiment()
                {
                    Id = (int)reader["Id"],
                    DeviceId = (int)reader["DeviceId"],
                    KeyValue = reader["KeyValue"].ToString(),
                    Value = reader["Value"].ToString(),
                    ForRegistered = Convert.ToBoolean(reader["ForRegistered"]),
                    ExperimentType = (ExperimentType)(int)reader["ExperimentType"]
                };

                result.Add(experiment);
            }

            return result;
        }

        public async Task<Experiment> GetExperimentByRequest(ExperimentRequest request)
        {
            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = $"SELECT * FROM Experiments WHERE DeviceId = {request.DeviceId} AND KeyValue = '{request.KeyValue}' AND Value = '{request.Value}'";

            var command = new SqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var experiment = new Experiment()
                {
                    Id = (int)reader["Id"],
                    DeviceId = (int)reader["DeviceId"],
                    KeyValue = reader["KeyValue"].ToString(),
                    Value = reader["Value"].ToString(),
                    ForRegistered = Convert.ToBoolean(reader["ForRegistered"]),
                    ExperimentType = (ExperimentType)(int)reader["ExperimentType"]
                };

                return experiment;
            }

            return null;
        }

        public async Task<bool> Create(Experiment experiment)
        {
            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = $"INSERT INTO Experiments (DeviceId, KeyValue, Value, ForRegistered, ExperimentType) " +
                        $"SELECT @DeviceId, @KeyValue, @Value, @ForRegistered, @ExperimentType  " +
                        $"WHERE NOT EXISTS ( SELECT * FROM Experiments WHERE DeviceId = @DeviceId AND KeyValue = @KeyValue AND Value = @Value AND ForRegistered = @ForRegistered AND ExperimentType = @ExperimentType)";

            var command = new SqlCommand(query, connection);

            var DeviceId = command.CreateParameter();

            DeviceId.ParameterName = "@DeviceId";
            DeviceId.Direction = ParameterDirection.Input;
            DeviceId.Value = experiment.DeviceId;

            var KeyValue = command.CreateParameter();

            KeyValue.ParameterName = "@KeyValue";
            KeyValue.Direction = ParameterDirection.Input;
            KeyValue.Value = experiment.KeyValue;

            var Value = command.CreateParameter();

            Value.ParameterName = "@Value";
            Value.Direction = ParameterDirection.Input;
            Value.Value = experiment.Value;

            var ForRegistered = command.CreateParameter();

            ForRegistered.ParameterName = "@ForRegistered";
            ForRegistered.Direction = ParameterDirection.Input;
            ForRegistered.Value = 0;

            var ExperimentType = command.CreateParameter();

            ExperimentType.ParameterName = "@ExperimentType";
            ExperimentType.Direction = ParameterDirection.Input;
            ExperimentType.Value = (int)experiment.ExperimentType;

            command.Parameters.Add(DeviceId);
            command.Parameters.Add(KeyValue);
            command.Parameters.Add(Value);
            command.Parameters.Add(ForRegistered);
            command.Parameters.Add(ExperimentType);

            command.ExecuteNonQuery();

            var result = await GetExperimentByRequest(_mapper.Map<ExperimentRequest>(experiment));

            var mappedResult = _mapper.Map<ExperimentRequest>(result);

            var mappedExperiment = _mapper.Map<ExperimentRequest>(experiment);

            if (mappedResult.DeviceId == mappedExperiment.DeviceId && mappedResult.KeyValue == mappedExperiment.KeyValue && mappedResult.Value == mappedExperiment.Value && mappedResult.ExperimentType == mappedExperiment.ExperimentType)
            {
                return true;
            }

            return false;
        }

        public async Task UpdateAsync(Experiment experiment)
        {
            var original = await GetExperimentByIdAsync(experiment.Id);

            if (original != null)
            {
                var connection = new SqlConnection(connectionString);

                connection.Open();

                var query = $"UPDATE Experiments " +
                            $"SET DeviceId = {experiment.DeviceId}," +
                            $"KeyValue = '{experiment.KeyValue}'," +
                            $"Value = '{experiment.Value}'," +
                            $"ForRegistered = {Convert.ToInt32(experiment.ForRegistered)}," +
                            $"ExperimentType = {(int)experiment.ExperimentType} " +
                            $"WHERE Id={original.Id}";

                var command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var original = await GetExperimentByIdAsync(id);

            if (original != null)
            {
                var connection = new SqlConnection(connectionString);

                connection.Open();

                var query = $"DELETE FROM Experiments WHERE Id={id}'";

                var command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }
    }
}
