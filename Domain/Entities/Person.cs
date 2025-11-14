using Domain.Common;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.ValidationSpecifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class Person : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string MailAddress { get; set; } = string.Empty;

    
        public static async Task<Person> CreateAsync(string firstname, string lastname, string emailA
            IPersonUniquenessChecker uniquenessChecker, CancellationToken ct = default)
        {
            var trimF = firstname.Trim();
            var trimL = lastname.Trim();
            var trimEm = emailA.Trim();
            ValidatePersonProperty(trimF, trimL, trimEm);
            await ValidatePersonUniqueness(0,trimF, trimL, trimEm, uniquenessChecker, ct);
            return new Person {
                FirstName = trimF,
                LastName = trimL,
                MailAddress = trimEm
            };
        }
        /// <summary>
        /// Aktualisiert die Eigenschaften des Sensors.
        /// </summary>
        public async Task UpdateAsync(string firstname, string lastname, string emailA
            IPersonUniquenessChecker uniquenessChecker, CancellationToken ct = default)
        {

            var trimF = firstname.Trim();
            var trimL = lastname.Trim();
            var trimEm = emailA.Trim();
            if (FirstName == trimF &&LastName == trimL && MailAddress == trimEm)
                return; // Keine Änderung
            ValidatePersonProperties(trimF, trimL, trimEm);
            await ValidatePersonUniqueness(Id, trimF, trimL, trimEm, uniquenessChecker, ct);
            FirstName = trimF;
            LastName = trimL;
            MailAddress = trimEm;
        }
        public override string ToString() => $"{FirstName} {LastName} {MailAddress}";
        public static void ValidatePersonProperties(string firstN, string lastN, string mailA)
        {
            var validationResults = new List<DomainValidationResult>
        {
            SensorSpecifications.CheckLocation(location),
            SensorSpecifications.CheckName(name),
            SensorSpecifications.CheckNameNotEqualLocation(name, location)
        };
            foreach (var result in validationResults)
            {
                if (!result.IsValid)
                {
                    throw new DomainValidationException(result.Property, result.ErrorMessage!);
                }
            }
        }
        public static async Task ValidatePersonUniqueness(int id, string firstN, string lastN, string mailA, 
            PersonUniquenessChecker uniquenessChecker, CancellationToken ct = default)
        {
            if (!await uniquenessChecker.IsUniqueAsync(id, firstN, lastN, mailA, ct))
                throw new DomainValidationException("Uniqueness", "Ein Sensor mit der gleichen Location und dem gleichen Namen existiert bereits.");
        }



    }
}
