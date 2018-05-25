namespace SharedResources.Interfaces
{
    public interface IRequest
    {
        string Verb { get; }
        string Noun { get; }
        string Query { get; }
        string Fields { get; }
    }
}
