using SharedResources.Interfaces;

namespace SharedResources
{
    public class Resource : IResource
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IType Type { get; set; }

        public Resource() { }

        public Resource(IType type)
        {
            Type = type;
        }
    }
}
