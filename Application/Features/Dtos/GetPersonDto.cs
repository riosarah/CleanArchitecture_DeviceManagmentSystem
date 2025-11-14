using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dtos
{
    public sealed record GetPersonDto(int Id, string FirstName, string LastName, string EmailAddress)
    {
    }
}
