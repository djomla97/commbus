namespace SharedResources
{
    public interface IResponse
    {
        IPayload Payload { get; }
        Status Status { get; }
        StatusCode StatusCode { get; }
    }
}
