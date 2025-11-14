using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IPersonUniquenessChecker
    {
        Task<bool> IsUniqueAsync(int id, string firstName, string lastName, string mailAddress, CancellationToken ct = default);

    }
}
