using SharedResources.Interfaces;

namespace RepositoryLib
{
    public interface IRepository
    {
        IResponse DoQuery(string sqlQuery);
    }
}
