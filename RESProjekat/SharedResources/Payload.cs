using SharedResources.Interfaces;
using System.Collections.Generic;

namespace SharedResources
{
    public class Payload : IPayload
    {
        public List<IResource> Resource { get; set; }
        public string ErrorMessage { get; set; }

        public Payload()
        {

        }

        public Payload(List<IResource> resource, string errorMessage)
        {
            this.Resource = resource;
            this.ErrorMessage = errorMessage;
        }
    }
}
