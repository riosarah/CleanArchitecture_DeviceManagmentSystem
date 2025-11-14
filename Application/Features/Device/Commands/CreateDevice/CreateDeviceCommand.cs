using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Device.Commands.CreateDevice
{
    public readonly record struct CreateDeviceCommand(string Name, string SerialNumber, Domain.Entities.Device.DeviceType type):IRequest<Result<GetDeviceDto>>
    {
    }
}
