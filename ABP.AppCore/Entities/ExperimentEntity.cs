using ABP.AppCore.Enums;

namespace ABP.AppCore.Entities
{
    public class ExperimentEntity
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public ExperimentType ExperimentType { get; set; }

        public string KeyValue { get; set; }

        public string Value { get; set; }

        public bool ForRegistered { get; set; }
    }
}
