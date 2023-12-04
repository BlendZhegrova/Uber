using Uber.Data;
using Uber.Domain;
using Uber.Repositories.Interface;

namespace Uber.Repositories;

public class CustomerRepository : Repository<Customer>,ICustomerRepository
{
    public CustomerRepository(DataContext context) : base(context)
    {
    }
}