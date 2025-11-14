using Application.Contracts;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PersonUniquenessChecker(IUnitOfWork uow) : IPersonUniquenessChecker
    {
        public async Task<bool> IsUniqueAsync(int id, string firstName, string lastName, string mailAddress, CancellationToken ct = default)
        {
            var exists = await uow.Persons.GetByNameAndMailAsync(firstName, lastName, mailAddress, ct);
            return exists is null || exists.Id == id;
        }
    }
}
