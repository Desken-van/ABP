using ABP.Application.Models.Requests;
using ABP.Application.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using ABP.Repository.ContractImplementation.DirectImplementations;

namespace ABP.Repository.Repository.DirectRepositories
{
    public class DeviceTokenDirectRepository : IDeviceTokenDirectRepository
    {
        private readonly IConfiguration configuration;
        private readonly IMapper _mapper;
        private readonly string connectionString;

        public DeviceTokenDirectRepository(IConfiguration config, IMapper mapper)
        {
            configuration = config;
            _mapper = mapper;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<DeviceToken>> GetDeviceTokenListAsync()
        {
            var result = new List<DeviceToken>();

            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = "SELECT * FROM DeviceTokens";

            var command = new SqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var token = new DeviceToken()
                {
                    Id = (int)reader["Id"],
                    DeviceId = (int)reader["DeviceId"],
                    Name = reader["Name"].ToString(),
                    Value = reader["Value"].ToString(),
                    Expired = Convert.ToBoolean(reader["Expired"])
                };

                result.Add(token);
            }

            return result.AsEnumerable();
        }

        public async Task<DeviceToken> GetDeviceTokenByIdAsync(int id)
        {
            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = $"SELECT * FROM DeviceTokens WHERE Id = {id}";

            var command = new SqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var device = new DeviceToken()
                {
                    Id = (int)reader["Id"],
                    DeviceId = (int)reader["DeviceId"],
                    Name = reader["Name"].ToString(),
                    Value = reader["Value"].ToString(),
                    Expired = Convert.ToBoolean(reader["Expired"])
                };

                return device;
            }

            return null;
        }

        public async Task<List<DeviceToken>> GetDeviceTokenByDeviceIdAsync(int deviceId)
        {
            var result = new List<DeviceToken>();   
            
            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = $"SELECT * FROM DeviceTokens WHERE DeviceId = {deviceId}";

            var command = new SqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var token = new DeviceToken()
                {
                    Id = (int)reader["Id"],
                    DeviceId = (int)reader["DeviceId"],
                    Name = reader["Name"].ToString(),
                    Value = reader["Value"].ToString(),
                    Expired = Convert.ToBoolean(reader["Expired"])
                };

                result.Add(token);
            }

            return result;
        }

        public async Task<DeviceToken> GetDeviceTokenByRequestAsync(DeviceTokenRequest request)
        {
            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = $"SELECT * FROM DeviceTokens WHERE DeviceId = {request.DeviceId} AND Name = '{request.Name}' AND Value = '{request.Value}'";

            var command = new SqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var device = new DeviceToken()
                {
                    Id = (int)reader["Id"],
                    DeviceId = (int)reader["DeviceId"],
                    Name = reader["Name"].ToString(),
                    Value = reader["Value"].ToString(),
                    Expired = Convert.ToBoolean(reader["Expired"])
                };

                return device;
            }

            return null;
        }

        public async Task<bool> Create(DeviceToken token)
        {
            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = $"INSERT INTO DeviceTokens (DeviceId, Name, Value, Expired) " +
                              $"SELECT @DeviceId, @Name, @Value, @Expired  " +
                              $"WHERE NOT EXISTS ( SELECT * FROM DeviceTokens WHERE DeviceId = @DeviceId AND Name = @Name AND Value = @Value AND Expired = @Expired)";

            var command = new SqlCommand(query, connection);

            var DeviceId = command.CreateParameter();

            DeviceId.ParameterName = "@DeviceId";
            DeviceId.Direction = ParameterDirection.Input;
            DeviceId.Value = token.DeviceId;

            var Name = command.CreateParameter();

            Name.ParameterName = "@Name";
            Name.Direction = ParameterDirection.Input;
            Name.Value = token.Name;

            var Value = command.CreateParameter();

            Value.ParameterName = "@Value";
            Value.Direction = ParameterDirection.Input;
            Value.Value = token.Value;

            var Expired = command.CreateParameter();

            Expired.ParameterName = "@Expired";
            Expired.Direction = ParameterDirection.Input;
            Expired.Value = token.Expired;

            command.Parameters.Add(DeviceId);
            command.Parameters.Add(Name);
            command.Parameters.Add(Value);
            command.Parameters.Add(Expired);

            command.ExecuteNonQuery();

            var result = await GetDeviceTokenByRequestAsync(_mapper.Map<DeviceTokenRequest>(token));

            var mappedResult = _mapper.Map<DeviceTokenRequest>(result);

            var mappedToken = _mapper.Map<DeviceTokenRequest>(token);

            if (mappedResult.DeviceId == mappedToken.DeviceId && mappedResult.Name == mappedToken.Name && mappedResult.Value == mappedToken.Value && mappedResult.Expired == mappedToken.Expired)
            {
                return true;
            }

            return false;
        }

        public async Task UpdateAsync(DeviceToken token)
        {
            var original = await GetDeviceTokenByIdAsync(token.Id);

            if (original != null)
            {
                var connection = new SqlConnection(connectionString);

                connection.Open();

                var query = $"UPDATE DeviceTokens " +
                            $"SET DeviceId = {token.DeviceId}," +
                            $"Name = '{token.Name}'," +
                            $"Value = '{token.Value}'," +
                            $"Expired = {Convert.ToInt32(token.Expired)} " +
                            $"WHERE Id={original.Id}";

                var command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var original = await GetDeviceTokenByIdAsync(id);

            if (original != null)
            {
                var connection = new SqlConnection(connectionString);

                connection.Open();

                var query = $"DELETE FROM DeviceTokens WHERE Id={id}'";

                var command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }
    }
}
