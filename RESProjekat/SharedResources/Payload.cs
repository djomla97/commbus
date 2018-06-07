using SharedResources.Interfaces;

namespace SharedResources
{
    public class Payload : IPayload
    {
        public IResource Resource { get; set; }
        public string ErrorMessage { get; set; }

        public Payload()
        {

        }

        public Payload(IResource resource, string errorMessage)
        {
            this.Resource = resource;
            this.ErrorMessage = errorMessage;
        }
    }
}
