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

namespace Application.Features.Usage.Queries.GetAllUsages
{
    public sealed class GetAllUsagesQueryHandler(IUnitOfWork uow): IRequestHandler<GetAllUsagesQuery, Result<IReadOnlyCollection<GetUsageDto>>>
    {
        public async Task<Result<IReadOnlyCollection<GetUsageDto>>> Handle(GetAllUsagesQuery request, CancellationToken ct)
        {
            var usages = await uow.Usages.GetAllAsync();
            var dtos = usages.Adapt<IReadOnlyCollection<GetUsageDto>>();
            return Result<IReadOnlyCollection<GetUsageDto>>.Success(dtos);
        }
    }
}
