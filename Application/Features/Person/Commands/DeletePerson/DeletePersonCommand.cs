using Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Person.Commands.DeletePerson
{
    public record struct DeletePersonCommand(int Id): IRequest<Result<bool>>;
}
