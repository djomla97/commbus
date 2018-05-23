using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedResources
{    
    public enum Status
    {
        BAD_FORMAT,
        REJECTED,
        SUCCESS
    }

    public enum StatusCodes
    {
        BAD_FORMAT_CODE = 5000,
        SUCCESS_CODE = 2000,
        REJECTED_CODE = 3000
    }
}
