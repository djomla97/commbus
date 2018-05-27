using SharedResources;
using SharedResources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLib
{
    public class Repository
    {
        public Repository()
        {

        }

        public IResponse DoQuery(string sqlZahtev)
        {
            return new Response();
        }
    }
}
