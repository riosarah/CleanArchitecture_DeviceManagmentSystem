using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Device.Commands.CreateDevice
{
    public class CreateDeviceCommandHandler(IUnitOfWork uow)
        : IRequestHandler<CreateDeviceCommand, Result<GetDeviceDto>
    {
        public async Task<Result<GetDeviceDto>> Handle(CreateDeviceCommand request, CancellationToken ct)
        {
            var device = Domain.Entities.CreateAsync(request.Name, request.SerialNumber, request.type);
        }
    }
}
