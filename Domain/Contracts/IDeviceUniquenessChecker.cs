using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IDeviceUniquenessChecker
    {
        Task<bool> IsUniqueAsync(int id, string name, string serialNumber, CancellationToken ct = default);

    }
}
