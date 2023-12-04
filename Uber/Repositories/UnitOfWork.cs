using Uber.Data;
using Uber.Repositories.Interface;

namespace Uber.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork(DataContext context, IDriverRepository drivers)
    {
        _context = context;
        Drivers = drivers;
    }

    public IDriverRepository Drivers { get; }
    public ICustomerRepository Customers { get; }


    public void Dispose()
    {
        _context.Dispose();
    }

    public void Save()
    {
        _context.SaveChangesAsync();
    }
}