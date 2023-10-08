using ABP.AppCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABP.AppCore.Entities;

namespace ABP.Tests.Settings
{
    public static class DBStore
    {
        public static async Task<ABPContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ABPContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ABPContext(options);
            databaseContext.Database.EnsureCreated();

            var deviceList = GetDeviceEntityList();

            foreach (var device in deviceList)
            {
                databaseContext.Devices.Add(device);
            }

            var tokenList = GetDeviceTokenEntityList();

            foreach (var token in tokenList)
            {
                databaseContext.DeviceTokens.Add(token);
            }

            var experimentList = GetExperimentEntityList();

            foreach (var experiment in experimentList)
            {
                databaseContext.Experiments.Add(experiment);
            }

            await databaseContext.SaveChangesAsync();

            return databaseContext;
        }

        public static List<DeviceEntity> GetDeviceEntityList()
        {
            var result = new List<DeviceEntity>
            {
                new DeviceEntity { Id = 1, DeviceName = "Device1" },
                new DeviceEntity { Id = 2, DeviceName = "Device2" },
                new DeviceEntity { Id = 3, DeviceName = "Device3" },
            };

            return result;
        }

        public static List<DeviceTokenEntity> GetDeviceTokenEntityList()
        {
            var result = new List<DeviceTokenEntity>
            {
                new DeviceTokenEntity { Id = 1, DeviceId = 1, Name = "ButtonExperiment", Value = "ACbinsFVMi3tZUwV1h+CO0i8nnmGJlXWbhEST1XLhEdrf2ggwkocFYkKvtXVB+xSMw==", Expired = false },
                new DeviceTokenEntity { Id = 2, DeviceId = 1, Name = "PriceExperiment", Value = "AFa0SxgQ7Iidu7cwCPm/Wr7VKBpAqYZsaCOb+R3UcyCXABDMcOnwbJi4QTR1uMOGDg==", Expired = false },
                new DeviceTokenEntity { Id = 3, DeviceId = 2, Name = "ButtonExperiment", Value = "ALTUEGvIhNKJFus3U39RyqAbjN8K8Mm2PIQmGu75iNTjFrA5y3q9g6s7wIf/mKxbeg==", Expired = false },
                new DeviceTokenEntity { Id = 4, DeviceId = 2, Name = "PriceExperiment", Value = "AOkEie/jNFzV6p6Uc0F+BLbCzWWzebP6J3+wQrvLIT5RhOjd+aBHlzJuOHJW50KNVw==", Expired = false },
                new DeviceTokenEntity { Id = 5, DeviceId = 3, Name = "ButtonExperiment", Value = "AMZxdTDJqxYyS505V9WRKPzCtITMZoW8YNUuEv4/LkuhdZLo1p/RD4wTKaA7odZrmw==", Expired = false },
                new DeviceTokenEntity { Id = 6, DeviceId = 3, Name = "PriceExperiment", Value = "ADntQxwtEFUL3Qm74gEsSyuxHnuY7PAvhfIA94l3uwbSNEDHaN91Mzc++6H960vBJA==", Expired = false }

            };
            return result;
        }

        public static List<ExperimentEntity> GetExperimentEntityList()
        {
            var result = new List<ExperimentEntity>
            {
                new ExperimentEntity { Id = 1, DeviceId = 1, KeyValue = "ButtonExperiment", Value = "#00FF00", ForRegistered = true },
                new ExperimentEntity { Id = 2, DeviceId = 1, KeyValue = "PriceExperiment", Value = "10", ForRegistered = true },
                new ExperimentEntity { Id = 3, DeviceId = 2, KeyValue = "ButtonExperiment", Value = "#00FF00", ForRegistered = true },
                new ExperimentEntity { Id = 4, DeviceId = 2, KeyValue = "PriceExperiment", Value = "10", ForRegistered = true },
                new ExperimentEntity { Id = 5, DeviceId = 3, KeyValue = "ButtonExperiment", Value = "#0000FF", ForRegistered = true },
                new ExperimentEntity { Id = 6, DeviceId = 3, KeyValue = "PriceExperiment", Value = "20", ForRegistered = true }

            };

            return result;
        }
    }
}


