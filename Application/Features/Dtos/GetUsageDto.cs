using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dtos
{
    public sealed record GetUsageDto(int Id, int DeviceId, Device Device, DateTime From, DateTime To, Person Person, int PersonId)
    {
    }
    }
