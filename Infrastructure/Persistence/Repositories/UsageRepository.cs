using Application.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UsageRepository(AppDbContext ctx) : GenericRepository<Usage>(ctx), IUsageRepository
    {
        public async Task<Usage?> GetByPersonDeviceDateAsync(int id, int personId, int deviceId, DateTime from, DateTime to, CancellationToken ct)
        {
            return await ctx.Usages.FirstAsync(e => e.Id == id && e.PersonId == personId && e.DeviceId == deviceId && e.From >= from && e.To <= to, ct);
        }
    }
}
