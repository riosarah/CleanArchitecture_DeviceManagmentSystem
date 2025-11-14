using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUsageUniquenessChecker
    {
        Task<bool> IsUniqueAsync(int Id, Person person, int PersonId, DateTime from, DateTime to, Device device, int DeviceId, CancellationToken ct = default);

    }
}
