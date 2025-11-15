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

namespace Application.Features.Person.Queries.GetPersonById
{
    public sealed class GetPersonByIdQueryHandler(IUnitOfWork uow) : IRequestHandler<GetPersonByIdQuery, Result<GetPersonDto>>
    {
        public async Task<Result<GetPersonDto>> Handle(GetPersonByIdQuery request, CancellationToken ct)
        {
            var person = await uow.Persons.GetByIdAsync(request.Id, ct);
            var dto = person.Adapt<GetPersonDto>();
            return Result<GetPersonDto>.Success(dto);
        }
    }
}
