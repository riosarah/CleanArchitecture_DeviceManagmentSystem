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
    public class UsagesUniquenessChecker(IUnitOfWork uow) : IUsageUniquenessChecker
    {
        public async Task<bool> IsUniqueAsync(int Id, Person person, int PersonId, DateTime from, DateTime to, Device device, int DeviceId, CancellationToken ct = default)
        {
            var exists = await uow.Usages.GetByPersonDeviceDateAsync(Id, PersonId, DeviceId, from, to, ct);
            return exists is null;
        }
    }
}
