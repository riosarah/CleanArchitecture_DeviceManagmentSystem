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

namespace Application.Features.Person.Queries.GetAllPersons
{
    public class GetAllPersonsQueryHandler(IUnitOfWork uow) : 
        IRequestHandler<GetAllPersonsQuery,Result<IReadOnlyCollection<GetPersonDto>>>
    {
        public async Task<Result<IReadOnlyCollection<GetPersonDto>>> Handle(GetAllPersonsQuery request, CancellationToken ct)
        {
            var persons = await uow.Persons.GetAllAsync();
            var dtos = persons.Adapt<IReadOnlyCollection<GetPersonDto>>();
            return Result<IReadOnlyCollection<GetPersonDto>>.Success(dtos);

        }
    }
}
