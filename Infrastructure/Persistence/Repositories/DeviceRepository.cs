using Application.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class DeviceRepository(AppDbContext ctx) : GenericRepository<Device>(ctx), IDeviceRepository
    {
        public async Task<Device?> GetByNameAndSerialNrAsync(string name, string serialNr, CancellationToken ct)
        {
            return await ctx.Devices.FirstAsync(e=>e.Name == name && e.SerialNumber == serialNr);
            
        }
    }
}
