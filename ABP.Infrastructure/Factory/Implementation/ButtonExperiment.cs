using ABP.Application.Models.Table;
using System;
using System.Collections.Generic;

namespace ABP.Infrastructure.Factory.Implementation
{
    public class ButtonExperiment : IExperirementator
    {
        public string Calculate(string value)
        {
            if (value == null)
            {
                var rand = new Random();
                var button = rand.Next(1, 4);

                if (button == 1)
                {
                    var result = "#FF0000";

                    return result;
                }
                if (button == 2)
                {
                    var result = "#00FF00";

                    return result;
                }
                if (button == 3)
                {
                    var result = "#0000FF";

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


                foreach (var data in list)
                {
                    if (data.Experiment.Value == "#FF0000")
                    {
                        option1.DeviceData.Add(data);
                    }
                    if (data.Experiment.Value == "#00FF00")
                    {
                        option2.DeviceData.Add(data);
                    }
                    if (data.Experiment.Value == "#0000FF")
                    {
                        option3.DeviceData.Add(data);
                    }
                }

                option1.TotalCountIn = option1.DeviceData.Count;
                option2.TotalCountIn = option2.DeviceData.Count;
                option3.TotalCountIn = option3.DeviceData.Count;

                result.Options.Add(option1);
                result.Options.Add(option2);
                result.Options.Add(option3);
            }

            return result;
        }
    }
}
