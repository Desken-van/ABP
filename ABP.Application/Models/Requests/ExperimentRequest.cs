using ABP.AppCore.Enums;

namespace ABP.Application.Models.Requests
{
    public class ExperimentRequest
    {
        public int DeviceId { get; set; }

        public ExperimentType ExperimentType { get; set; }

        public string KeyValue { get; set; }

        public string Value { get; set; }
    }
}
