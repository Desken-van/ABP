namespace ABP.AppCore.Entities
{
    public class DeviceTokenEntity
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool Expired { get; set; }
    }
}
