using Uber.Repositories.Interface;
namespace Uber.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    public Task<List<T>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<T> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<T> Add(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T> Update(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T> Delete(int id)
    {
        throw new NotImplementedException();
    }
}