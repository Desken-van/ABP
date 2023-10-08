namespace ABP.Application.Models.Requests
{
    public class DeviceTokenRequest
    {
        public int DeviceId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool Expired { get; set; }
    }
}
