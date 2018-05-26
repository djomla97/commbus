using SharedResources.Interfaces;

namespace SharedResources
{
    public class Type : IType
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public Type() { }
    }
}
