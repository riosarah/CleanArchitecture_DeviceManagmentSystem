using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Domain.Contracts;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Person.Commands.UpdatePerson
{
    public sealed class UpdatePersonCommandHandler(IUnitOfWork uow, IPersonUniquenessChecker uniquenessChecker) 
        : IRequestHandler<UpdatePersonCommand, Result<GetPersonDto>>
    {
        public async Task<Result<GetPersonDto>> Handle(UpdatePersonCommand request, CancellationToken ct)
        {
            var person = await uow.Persons.GetByIdAsync(request.Id, ct);
            await person.UpdateAsync(request.FirstName, request.LastName, request.Email, uniquenessChecker, ct);
            uow.Persons.Update(person);
            await uow.SaveChangesAsync(ct);
            return person.Adapt<Result<GetPersonDto>>();
        }
    }
}
