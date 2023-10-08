using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ABP.AppCore.Enums;
using ABP.Application.Models;
using ABP.Application.Models.Requests;
using ABP_Server.Models;

namespace ABP_Client
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //await CalculateProcess(600, true);

            await TableProcess(true, ExperimentType.ButtonExperiment);

            Console.ReadKey();
        }

        private static async Task TableProcess(bool ef, ExperimentType type)
        {
            var root = string.Empty;

            if (ef)
            {
                root = "ef";
            }
            else
            {
                root = "direct";
            }

            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("https://localhost:44383");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PostAsJsonAsync<ExperimentType>($"api/{root}/experiment/table/{type}", type);
        }


        private static async Task CalculateProcess(int k, bool ef)
        {
            var root = string.Empty;

            if (ef)
            {
                root = "ef";
            }
            else
            {
                root = "direct";
            }
            
            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("https://localhost:44383");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var count = 0;

            while (count != k)
            {
                var device = new DeviceRequest()
                {
                    DeviceName = $"Device{count + 1}"
                };

                var response = await httpClient.PostAsJsonAsync<DeviceRequest>($"api/{root}/device/add", device);

                //response.EnsureSuccessStatusCode();

                var dataDevice = await httpClient.GetFromJsonAsync<Device>($"api/{root}/device/crud/getbyname/{device.DeviceName}");

                var tokenList = await httpClient.GetFromJsonAsync<List<DeviceToken>>($"api/{root}/device/token/crud/list/{dataDevice.Id}");

                foreach (var token in tokenList)
                {
                    var tokenRequest = new DeviceTokenRequest()
                    {
                        DeviceId = token.DeviceId,
                        Expired = token.Expired,
                        Name = token.Name,
                        Value = token.Value
                    };
                    
                    response = await httpClient.PostAsJsonAsync<DeviceTokenRequest>($"api/{root}/experiment/add", tokenRequest);
                }

                count++;
            }

            httpClient.Dispose();
        }
    }
}
