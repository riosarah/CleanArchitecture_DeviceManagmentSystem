using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Device.Queries.GetAllDevices
{
    public record GetAllDevicesQuery : IRequest<Result<IReadOnlyCollection<GetDeviceDto>>>;
}
