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

namespace Application.Features.Device.Queries.GetAllDevices
{
    public sealed class GetAllDevicesQueryHandler(IUnitOfWork uow) : IRequestHandler<GetAllDevicesQuery, Result<IReadOnlyCollection<GetDeviceDto>>>
    {
        public async Task<Result<IReadOnlyCollection<GetDeviceDto>>> Handle(GetAllDevicesQuery request, CancellationToken cancellationToken)
        {
            var entities = await uow.Devices.GetAllAsync(ct: cancellationToken);
            var dtos = entities.Adapt<IReadOnlyCollection<GetDeviceDto>>();
            return Result<IReadOnlyCollection<GetDeviceDto>>.Success(dtos);
        }
    }
    
}
