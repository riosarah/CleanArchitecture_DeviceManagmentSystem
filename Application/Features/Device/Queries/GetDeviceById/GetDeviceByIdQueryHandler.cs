using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Device.Queries.GetDeviceById
{
    public sealed class GetDeviceByIdQueryHandler(IUnitOfWork uow) : IRequestHandler<GetDeviceByIdQuery, Result<GetDeviceDto>>
    {
        public async Task<Result<GetDeviceDto>> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await uow.Devices.GetByIdAsync(request.Id, cancellationToken);

            if (entity is null)
            {
                return Result<GetDeviceDto>.NotFound($"Device with ID {request.Id} not found.");
            }
            return Result<GetDeviceDto>.Success(entity.Adapt<GetDeviceDto>());

        }


    }
}
