using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dtos
{
    public  sealed record GetDeviceDto(int Id, string Name, string SerialNumber, DeviceType type, string DeviceName)
    {
    }
 
}
