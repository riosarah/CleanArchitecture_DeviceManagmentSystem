using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Person.Queries.GetAllPersons
{
    public readonly record struct GetAllPersonsQuery() : IRequest<Result<IReadOnlyCollection<GetPersonDto>>>;
    
}
