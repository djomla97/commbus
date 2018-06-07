using SharedResources.Interfaces;

namespace SharedResources
{
    public class ResourceType : IType
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public ResourceType() { }
    }
}
