using ABP.Application.Models.Table;
using System;
using System.Collections.Generic;

namespace ABP.Infrastructure.Factory.Implementation
{
    public class PriceExperiment : IExperirementator
    {
        public string Calculate(string value)
        {
            if (value == null)
            {
                var rand = new Random();
                var button = rand.Next(1, 101);

                if (button <= 5)
                {
                    var result = "50";

                    return result;
                }
                if (button <= 15)
                {
                    var result = "20";

                    return result;
                }
                if (button <= 25)
                {
                    var result = "5";

                    return result;
                }

                if (button <= 101)
                {
                    var result = "10";

                    return result;
                }
            }
            else
            {
                return value;
            }

            return null;
        }

        public TableData GetOptionData(List<DeviceData> list)
        {
            var result = new TableData();

            result.Options = new List<OptionData>();

            if (list != null)
            {
                result.TotalDeviceCount = list.Count;

                var option1 = new OptionData();
                option1.DeviceData = new List<DeviceData>();

                var option2 = new OptionData();
                option2.DeviceData = new List<DeviceData>();

                var option3 = new OptionData();
                option3.DeviceData = new List<DeviceData>();

                var option4 = new OptionData();
                option4.DeviceData = new List<DeviceData>();


                foreach (var data in list)
                {
                    if (data.Experiment.Value == "50")
                    {
                        option1.DeviceData.Add(data);
                    }
                    if (data.Experiment.Value == "20")
                    {
                        option2.DeviceData.Add(data);
                    }
                    if (data.Experiment.Value == "5")
                    {
                        option3.DeviceData.Add(data);
                    }
                    if (data.Experiment.Value == "10")
                    {
                        option4.DeviceData.Add(data);
                    }
                }

                option1.TotalCountIn = option1.DeviceData.Count;
                option2.TotalCountIn = option2.DeviceData.Count;
                option3.TotalCountIn = option3.DeviceData.Count;
                option4.TotalCountIn = option4.DeviceData.Count;

                result.Options.Add(option1);
                result.Options.Add(option2);
                result.Options.Add(option3);
                result.Options.Add(option4);
            }

            return result;
        }
    }
}
