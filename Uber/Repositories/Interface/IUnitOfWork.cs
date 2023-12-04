using Uber.Data;
using Uber.Domain;

namespace Uber.Repositories.Interface;

public interface IUnitOfWork : IDisposable
{
    IDriverRepository Drivers { get; }
    ICustomerRepository Customers { get; }
    void Save();
}