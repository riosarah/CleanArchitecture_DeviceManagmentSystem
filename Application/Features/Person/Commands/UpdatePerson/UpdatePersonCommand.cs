using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Person.Commands.UpdatePerson
{
    public readonly record struct UpdatePersonCommand(int Id, string FirstName, string LastName, string Email) : IRequest<Result<GetPersonDto>>;
}
