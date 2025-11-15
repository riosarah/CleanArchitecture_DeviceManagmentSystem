using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Device.Commands.UpdateDevice
{
    public readonly record struct UpdateDeviceCommand(int Id, string name, string serialNumber, Domain.Entities.Device.DeviceType type) : IRequest<Result<GetDeviceDto>>
    {
    }
}
