using Domain.Common;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.ValidationSpecifications;
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
        public DeviceType Type { get; set; }
        public string DeviceName
        {
            get
            {
                return Type switch
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

        public static async Task<Device> CreateAsync(string name, string serialNumber, DeviceType type)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new DomainValidationException( nameof(name),"Device name cannot be null or empty.");
            }
            if (String.IsNullOrWhiteSpace(serialNumber))
            {
                throw new DomainValidationException("Device", "Device Serialnumber must not be empty");

            }
            return new Device
            {
                Name = name,
                SerialNumber = serialNumber,
                Type = type
            };
        }

        public async Task UpdateAsync(string name, string serialnumber, DeviceType type
       /*,IDeviceUniquenessChecker uniquenessChecker*/, CancellationToken ct = default)
        {
            var trimmedName = name.Trim();
            var trimmedSN = serialnumber.Trim();
           if(Name == trimmedName && SerialNumber == serialnumber)
            {
                return;
            }
            ValidateDeviceProperties(trimmedName, trimmedSN, type);
            //await ValidateDeviceUniqueness(Id,trimmedName, trimmedSN, uniquenessChecker, ct);
            Name = name;
            SerialNumber = serialnumber;
            Type = type;
        }
        public override string ToString() => $"{Name} {SerialNumber} {Type.ToString()}";
        public static void ValidateDeviceProperties(string name, string serialNr, DeviceType type)
        {
            var validationResults = new List<DomainValidationResult>
        {
            DeviceSpecifications.CheckName(name),
            DeviceSpecifications.CheckSerialNumber(serialNr)
        };
            foreach (var result in validationResults)
            {
                if (!result.IsValid)
                {
                    throw new DomainValidationException(result.Property, result.ErrorMessage!);
                }
            }
        }
        public static async Task ValidateDeviceUniqueness(int id, string name, string serialNr, DeviceType type
,                IDeviceUniquenessChecker uniquenessChecker, CancellationToken ct = default)
        {
            if (!await uniquenessChecker.IsUniqueAsync(id, name, serialNr, ct))
                throw new DomainValidationException("Uniqueness", "Ein Device mit den gleichen Eigenschaften existiert bereits.");
        }
    }
}
