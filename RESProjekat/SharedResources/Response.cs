namespace SharedResources
{
    public class Response : IResponse
    {
        public IPayload Payload { get; set; }
        public Status Status { get; set; }
        public StatusCode StatusCode { get; set; }
    }
}
