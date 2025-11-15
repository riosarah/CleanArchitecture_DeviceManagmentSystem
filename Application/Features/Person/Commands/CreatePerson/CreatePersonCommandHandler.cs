using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Person.Commands.CreatePerson
{
    public sealed class CreatePersonCommandHandler(IUnitOfWork uow)
        : IRequestHandler<CreatePersonCommand, Result<GetPersonDto>>
    {
        public async Task<Result<GetPersonDto>> Handle(CreatePersonCommand request, CancellationToken ct)
        {
            var entity = request.Adapt<Domain.Entities.Person>();
            await uow.Persons.AddAsync(entity, ct);
            await uow.SaveChangesAsync(ct);
            var dto = entity.Adapt<GetPersonDto>();
            return Result<GetPersonDto>.Created(dto);
        }
    }
}
