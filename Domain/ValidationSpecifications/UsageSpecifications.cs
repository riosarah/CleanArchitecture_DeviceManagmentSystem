using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValidationSpecifications
{
    public class UsageSpecifications
    {

        public const int NameMinLength = 2;
        public static DomainValidationResult CheckDate(DateTime to, DateTime from) =>
            to < from
                ? DomainValidationResult.Failure("Date", "Das Enddatum muss nach dem Startdatum liegen.")
                : DomainValidationResult.Success("Date");

        public static DomainValidationResult CheckPerson(Person person, int personid)=>
                        person.Id != personid
                ? DomainValidationResult.Failure("Person", "Die angegebene Person ist nicht korrekt.")
                : DomainValidationResult.Success("Person");

        public static DomainValidationResult CheckDevice( Device device, int deviceid) =>
                        device.Id != deviceid
                ? DomainValidationResult.Failure("Device", "Das angegebene Ger�t ist nicht korrekt.")
                : DomainValidationResult.Success("Device"); 
    }
}
