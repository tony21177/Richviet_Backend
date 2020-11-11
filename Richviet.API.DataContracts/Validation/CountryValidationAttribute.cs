using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.OpenApi.Extensions;
using Richviet.API.Common.Constants;

namespace Richviet.API.DataContracts.Validation
{
    public class CountryValidationAttribute : ValidationAttribute
    {
        private readonly string[] _allowedValues;

        public CountryValidationAttribute(params string[] allowedValues)
        {
            _allowedValues = allowedValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var country = value;
            if (_allowedValues.Any(value=>value.Equals(country)))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"{country} is not a valid country");
        }
    }
}
