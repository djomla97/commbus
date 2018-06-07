namespace SharedResources.Interfaces
{
    public interface IResource
    {  
        int ID { get; set; }
        string Title { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        IType Type { get; set; }
    }
}
