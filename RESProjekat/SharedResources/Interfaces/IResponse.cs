namespace SharedResources.Interfaces
{
    public interface IResponse
    {
        IPayload Payload { get; set; }
        string Status { get; set; }
        StatusCode StatusCode { get; set; }
    }
}
