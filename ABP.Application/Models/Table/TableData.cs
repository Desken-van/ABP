using System.Collections.Generic;

namespace ABP.Application.Models.Table
{
    public class TableData
    {
        public int TotalDeviceCount { get; set; }
        public List<OptionData> Options { get; set; }
    }
}
