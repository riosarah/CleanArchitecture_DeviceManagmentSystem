using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Repositories
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        Task<Person?> GetByNameAndMailAsync(string firstName, string lastName, string mailAddress, CancellationToken ct);
    }
}
