namespace Owt.Services.Contacts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using EmailValidation;

    using Owt.Common.Contacts;

    using PhoneNumbers;

    public class ContactValidator : IContactValidator
    {
        private readonly HashSet<string> supportedValidationRegions;

        public ContactValidator(HashSet<string> supportedValidationRegions)
        {
            this.supportedValidationRegions = supportedValidationRegions ?? throw new ArgumentNullException(nameof(supportedValidationRegions));

            if (this.supportedValidationRegions.Count == 0)
            {
                throw new ArgumentException($"At least 1 supported region must be defined.");
            }
        }

        public void ValidateContact(IValidableContactDto validableContactDto)
        {
            if (validableContactDto == null)
            {
                throw new ArgumentNullException(nameof(validableContactDto));
            }

            ValidateNotEmpty(nameof(validableContactDto.Firstname), validableContactDto.Firstname);
            ValidateNotEmpty(nameof(validableContactDto.Lastname), validableContactDto.Lastname);
            ValidateNotEmpty(nameof(validableContactDto.Address), validableContactDto.Address);
            this.ValidateMobilePhoneNumber(validableContactDto.MobilePhoneNumber);
            this.ValidateEmailAddress(validableContactDto.Email);
        }

        private void ValidateEmailAddress(string email)
        {
            if (!EmailValidator.Validate((email ?? string.Empty).Trim()))
            {
                throw new ApplicationException("Email is not valid.");
            }
        }

        private void ValidateMobilePhoneNumber(string mobilePhoneNumber)
        {
            PhoneNumberUtil instance = PhoneNumberUtil.GetInstance();
            HashSet<string> validSupportedRegions = this.supportedValidationRegions
                .Join(
                    instance.GetSupportedRegions(),
                    s => s,
                    r => r,
                    (s, r) => s)
                .OrderBy(code => code)
                .ToHashSet();

            bool validPhoneNumber = false;
            foreach (string region in validSupportedRegions)
            {
                try
                {
                    PhoneNumber phoneNumber = instance.Parse(mobilePhoneNumber, region);
                    if (instance.GetNumberType(phoneNumber) != PhoneNumberType.MOBILE)
                    {
                        continue;
                    }

                    validPhoneNumber = true;
                    break;
                }
                catch (NumberParseException)
                {
                }
            }

            if (validPhoneNumber)
            {
                return;
            }

            StringBuilder errorMessage = new StringBuilder();
            errorMessage.AppendLine("Invalid mobile phone number.");
            errorMessage.AppendLine("Examples :");
            foreach (string region in validSupportedRegions)
            {
                PhoneNumber example = instance.GetExampleNumberForType(region, PhoneNumberType.MOBILE);

                StringBuilder formattedExample = new StringBuilder();

                if (example.NumberOfLeadingZeros > 0)
                {
                    formattedExample.Append("(");
                    for (int i = 0; i < example.NumberOfLeadingZeros; i++)
                    {
                        formattedExample.Append("0");
                    }

                    formattedExample.Append(")");
                }

                formattedExample.Append(example.NationalNumber);

                errorMessage.AppendLine($"\t{region} : {formattedExample}");
            }

            throw new ApplicationException(errorMessage.ToString());
        }

        private static void ValidateNotEmpty(string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value.Trim()))
            {
                throw new ApplicationException($"Property '{propertyName}' cannot be null");
            }
        }
    }
}