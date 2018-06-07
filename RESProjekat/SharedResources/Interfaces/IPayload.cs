namespace SharedResources.Interfaces
{
    public interface IPayload
    {
        IResource Resource { get; set; }
        string ErrorMessage { get; set; }
    }
}
