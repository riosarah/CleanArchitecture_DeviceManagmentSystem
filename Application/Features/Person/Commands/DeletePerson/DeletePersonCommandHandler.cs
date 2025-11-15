using Application.Common.Results;
using Application.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Person.Commands.DeletePerson
{
    public sealed class DeletePersonCommandHandler(IUnitOfWork uow) 
        : IRequestHandler<DeletePersonCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeletePersonCommand request, CancellationToken ct)
        {
            var person = await uow.Persons.GetByIdAsync(request.Id, ct);
            if(person == null)
            {
                return Result<bool>.NotFound($"Person with Id {request.Id} not found.");
            }
            uow.Persons.Remove(person);
            await uow.SaveChangesAsync(ct);
            return Result<bool>.NoContent();

        }
    }
}
