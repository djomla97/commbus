using System.Collections.Generic;

namespace SharedResources.Interfaces
{
    public interface IPayload
    {
        List<IResource> Resource { get; set; }
        string ErrorMessage { get; set; }
    }
}
