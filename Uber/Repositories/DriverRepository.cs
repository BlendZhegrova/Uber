using Uber.Data;
using Uber.Domain;
using Uber.Repositories.Interface;

namespace Uber.Repositories;

public class DriverRepository : Repository<Driver>, IDriverRepository
{
    public DriverRepository(DataContext context) : base(context)
    {
    }
    
}