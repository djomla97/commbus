using SharedResources.Interfaces;

namespace SharedResources
{
    public class Response : IResponse
    {
        public IPayload Payload { get; set; }
        public string Status { get; set; }
        public StatusCode StatusCode { get; set; }

        public Response() { }

        public Response(IPayload payload, Status status, StatusCode statusCode)
        {
            this.Payload = payload;
            this.Status = status.ToString();
            this.StatusCode = statusCode;
        }
    }
}
