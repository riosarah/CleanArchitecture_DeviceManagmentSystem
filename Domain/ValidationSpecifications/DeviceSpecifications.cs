using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValidationSpecifications
{
    public static class DeviceSpecifications
    {

        public const int NameMinLength = 2;

        public static DomainValidationResult CheckSerialNumber(string serialNr) =>
            string.IsNullOrWhiteSpace(serialNr)
                ? DomainValidationResult.Failure("SerialNumber", "Serialnumber darf nicht leer sein.")
                : DomainValidationResult.Success("SerialNumber");

        public static DomainValidationResult CheckName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return DomainValidationResult.Failure("Name", "Name darf nicht leer sein.");
            }
            if (name.Trim().Length < NameMinLength)
            {
                return DomainValidationResult.Failure("Name", $"Name muss mindestens {NameMinLength} Zeichen haben.");
            }
            return DomainValidationResult.Success("Name");
        }

        



    }
}
