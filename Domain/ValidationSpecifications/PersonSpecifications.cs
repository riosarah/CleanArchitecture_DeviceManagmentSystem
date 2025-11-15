using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValidationSpecifications
{
    public class PersonSpecifications
    {

        public const int NameMinLength = 2;

        public static DomainValidationResult CheckFirstName(string fName) =>
            string.IsNullOrWhiteSpace(fName)
                ? DomainValidationResult.Failure("FirstName", "FirstName darf nicht leer sein.")
                : DomainValidationResult.Success("FirstName");
        public static DomainValidationResult CheckLastName(string lName) =>
            string.IsNullOrWhiteSpace(lName)
                ? DomainValidationResult.Failure("LastName", "LastName darf nicht leer sein.")
                : DomainValidationResult.Success("LastName");

        public static DomainValidationResult CheckMailAddress(string mailA) =>
            string.IsNullOrWhiteSpace(mailA)
                ? DomainValidationResult.Failure("MailAddress", "MailAddress darf nicht leer sein.")
                : DomainValidationResult.Success("MailAddress");
       
    }
}
