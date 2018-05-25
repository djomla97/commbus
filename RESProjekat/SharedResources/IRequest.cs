using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedResources
{
    public interface IRequest
    {
        string Verb { get; }
        string Noun { get; }
        string Query { get; }
        string Fields { get; }
    }
}
