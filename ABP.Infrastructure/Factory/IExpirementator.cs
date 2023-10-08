using System.Collections.Generic;
using ABP.Application.Models.Table;

namespace ABP.Infrastructure.Factory
{
    public interface IExperirementator
    {
        string Calculate(string value);

        TableData GetOptionData(List<DeviceData> list);
    }
}
