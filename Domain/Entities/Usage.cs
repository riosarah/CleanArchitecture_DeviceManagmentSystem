using Domain.Common;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.ValidationSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class Usage : BaseEntity
    {
        public Device? Device { get; set; }
        public int DeviceId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Person? Person { get; set; }
        public int PersonId { get; set; }


        public static async Task<Usage> CreateAsync(Device device, int deviceId, 
            DateTime from, DateTime to, Person person, int personId, IUsageUniquenessChecker uniquenessChecker, CancellationToken ct = default)
        {
            ValidateUsageProperties(device, deviceId,from, to, person, personId);
            await ValidateUsageUniqueness(0, device, deviceId, from, to, person, personId, uniquenessChecker, ct);
            return new Usage
            {
                Device = device,
                DeviceId = deviceId,
                From = from,
                To = to,
                Person = person,
                PersonId = personId
            };
        }
        /// <summary>
        /// Aktualisiert die Eigenschaften des Sensors.
        /// </summary>
        public async Task UpdateAsync(Device device, int deviceId,
            DateTime from, DateTime to, Person person, int personId, 
            IUsageUniquenessChecker uniquenessChecker, CancellationToken ct = default)       
        {
            ValidateUsageProperties(device, deviceId, from, to, person, personId);
            await ValidateUsageUniqueness(0, device, deviceId, from, to, person, personId, uniquenessChecker, ct);
            Device = device;
            DeviceId = deviceId;
            From = from;
            To = to;
            Person = person;
            PersonId = personId;
        }
        public override string ToString() => $"Usage: Device: {Device?.ToString()} {From} {To} {Person?.ToString()}";
        public static void ValidateUsageProperties(Device device, int deviceId, DateTime from, DateTime to,Person person, int personId)
        {
            var validationResults = new List<DomainValidationResult>
        {
            UsageSpecifications.CheckDevice(device, deviceId),
            UsageSpecifications.CheckPerson(person, personId)
            };
            foreach (var result in validationResults)
            {
                if (!result.IsValid)
                {
                    throw new DomainValidationException(result.Property, result.ErrorMessage!);
                }
            }
        }
        public static async Task ValidateUsageUniqueness(int Id,Device device, int deviceId, DateTime from, 
            DateTime to, Person person, int personId, IUsageUniquenessChecker uniquenessChecker, CancellationToken ct)
        {
            if (!await uniquenessChecker.IsUniqueAsync(Id,person, personId, from ,to, device, deviceId,  ct))
                throw new DomainValidationException("Uniqueness", "Ein Sensor mit der gleichen Location und dem gleichen Namen existiert bereits.");
        }



    }


}
