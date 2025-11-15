using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Domain.Contracts;
using Domain.Entities;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Device.Commands.CreateDevice
{
    public class CreateDeviceCommandHandler(IUnitOfWork uow, IDeviceUniquenessChecker uniquenessChecker)
        : IRequestHandler<CreateDeviceCommand, Result<GetDeviceDto>>
    {
        public async Task<Result<GetDeviceDto>> Handle(CreateDeviceCommand request, CancellationToken ct)
        {
            var device = await Domain.Entities.Device.CreateAsync(request.Name, request.SerialNumber, request.type, uniquenessChecker, ct);
            await uow.Devices.AddAsync(device, ct);
            await uow.SaveChangesAsync(ct);
            return Result<GetDeviceDto>.Created(device.Adapt<GetDeviceDto>());

        }
    }
}
