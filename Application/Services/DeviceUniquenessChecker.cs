using Application.Contracts;
using Domain.Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DeviceUniquenessChecker(IUnitOfWork uow) : IDeviceUniquenessChecker
    {
        public async Task<bool> IsUniqueAsync(int id, string name, string serialNr, CancellationToken ct = default)
        {
            var existing = await uow.Devices.GetByNameAndSerialNrAsync(name, serialNr, ct);
            return existing is null || existing.Id == id;
        }
    }
}
