using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Device : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string NameSerialNumber => $"{Name}-{SerialNumber}";
        public DeviceType type { get; set; }
        public string DeviceName
        {
            get
            {
                return type switch
                {
                    DeviceType.Smartphone => "Smartphone",
                    DeviceType.Tablet => "Tablet",
                    DeviceType.Notebook => "Notebook",
                    _ => "Unknown Device"
                };
            }
        }
        public enum DeviceType
        {
            Smartphone,
            Tablet,
            Notebook
        }
    }
}
