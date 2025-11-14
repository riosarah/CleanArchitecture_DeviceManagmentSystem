using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Repositories
{
    public interface IDeviceRepository : IGenericRepository<Device>
    {
        public Task<Device?> GetByNameAndSerialNrAsync(string name, string serialNr, CancellationToken ct);
    }
}
