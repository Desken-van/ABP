using Microsoft.Extensions.Configuration;
using AutoMapper;
using ABP.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq;
using ABP.Repository.ContractImplementation.DirectImplementations;
using System.Data;

namespace ABP.Repository.Repository.DirectRepositories
{
    public class DeviceDirectRepository : IDeviceDirectRepository
    {
        private readonly IConfiguration configuration;
        private readonly IMapper _mapper;
        private readonly string connectionString;

        public DeviceDirectRepository(IConfiguration config, IMapper mapper)
        {
            configuration = config;
            _mapper = mapper;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Device>> GetDeviceListAsync()
        {
            var result = new List<Device>();

            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = "SELECT * FROM Devices";

            var command = new SqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var device = new Device()
                {
                    Id = (int)reader["Id"],
                    DeviceName = reader["DeviceName"].ToString(),
                };

                result.Add(device);
            }

            return result.AsEnumerable();
        }

        public async Task<Device> GetDeviceByIdAsync(int id)
        {
            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = $"SELECT * FROM Devices WHERE Id = {id}";

            var command = new SqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var device = new Device()
                {
                    Id = (int)reader["Id"],
                    DeviceName = reader["DeviceName"].ToString(),
                };

                return device;
            }

            return null;
        }

        public async Task<Device> GetDeviceByNameAsync(string name)
        {
            var connection = new SqlConnection(connectionString);

            connection.Open();

            var query = $"SELECT * FROM Devices WHERE DeviceName = '{name}'";

            var command = new SqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var device = new Device()
                {
                    Id = (int)reader["Id"],
                    DeviceName = reader["DeviceName"].ToString(),
                };

                return device;
            }

            return null;
        }

        public async Task<bool> Create(Device device)
        {
            var connection = new SqlConnection(connectionString);
            
            connection.Open();

            var query = $"INSERT INTO Devices (DeviceName) SELECT @DeviceName WHERE NOT EXISTS ( SELECT * FROM Devices WHERE DeviceName = @DeviceName)";

            var command = new SqlCommand(query, connection);

            var DeviceName = command.CreateParameter();

            DeviceName.ParameterName = "@DeviceName";
            DeviceName.Direction = ParameterDirection.Input;
            DeviceName.Value = device.DeviceName;

            command.Parameters.Add(DeviceName);

            command.ExecuteNonQuery();

            var result = await GetDeviceByNameAsync(device.DeviceName);

            if (result.DeviceName == device.DeviceName)
            {
                return true;
            }

            return false;
        }

        public async Task UpdateAsync(Device device)
        {
            var original = await GetDeviceByIdAsync(device.Id);

            if (original != null)
            {
                var connection = new SqlConnection(connectionString);
            
                connection.Open();

                var query = $"UPDATE Devices SET DeviceName='{device.DeviceName}' WHERE Id={original.Id}";

                var command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var original = await GetDeviceByIdAsync(id);

            if (original != null)
            {
                var connection = new SqlConnection(connectionString);

                connection.Open();

                var query = $"DELETE FROM Devices WHERE Id={id}'";

                var command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }
    }
}
