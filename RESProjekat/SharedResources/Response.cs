using SharedResources.Interfaces;

namespace SharedResources
{
    public class Response : IResponse
    {
        public IPayload Payload { get; set; }
        public string Status { get; set; }
        public StatusCode StatusCode { get; set; }
    }
}
