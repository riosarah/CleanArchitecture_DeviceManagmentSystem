using Application.Contracts.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UsageRepository(AppDbContext ctx) : GenericRepository<Usage>(ctx), IUsageRepository
    {
    }
}
