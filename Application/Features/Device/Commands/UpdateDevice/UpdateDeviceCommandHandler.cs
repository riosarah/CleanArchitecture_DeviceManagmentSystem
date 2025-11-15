using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Application.Features.Sensors.Commands.UpdateSensor;
using Domain.Contracts;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Device.Commands.UpdateDevice
{
    public sealed class UpdateDeviceCommandHandler(IUnitOfWork uow, IDeviceUniquenessChecker uniquenessChecker)
    : IRequestHandler<UpdateDeviceCommand, Result<GetDeviceDto>>
    {
        public async Task<Result<GetDeviceDto>> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
        {

            var entity = await uow.Devices.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null) return Result<GetDeviceDto>.NotFound($"Device with ID {request.Id} not found.");

            await entity.UpdateAsync(request.name, request.serialNumber, request.type, uniquenessChecker, cancellationToken);
            uow.Devices.Update(entity);
            await uow.SaveChangesAsync(cancellationToken);
            return Result<GetDeviceDto>.Success(entity.Adapt<GetDeviceDto>());

        }
    }
}
