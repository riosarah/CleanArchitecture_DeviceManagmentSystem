using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Device.Queries.GetDeviceById
{
    public readonly record struct GetDeviceByIdQuery(int Id):IRequest<Result<GetDeviceDto>>
    {
    }
}
