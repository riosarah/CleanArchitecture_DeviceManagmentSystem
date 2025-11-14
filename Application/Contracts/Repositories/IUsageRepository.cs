using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Repositories
{
    public interface IUsageRepository : IGenericRepository<Usage>
    {
        Task<Usage?> GetByPersonDeviceDateAsync(int id, int personId, int deviceId, DateTime from, DateTime to, CancellationToken ct);
    }
}
