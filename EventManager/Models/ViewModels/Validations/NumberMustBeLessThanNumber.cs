using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels.Validations
{
    public class NumberMustBeLessThanNumber : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public NumberMustBeLessThanNumber(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            ErrorMessage = ErrorMessageString;
            var currentValue = (int)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (int)property.GetValue(validationContext.ObjectInstance);

            if (currentValue > comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
