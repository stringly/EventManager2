using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels.Validations
{
    public class RequireIfRelatedFieldTrue : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public RequireIfRelatedFieldTrue(string comparisonProperty)
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

            var comparisonValue = (bool)property.GetValue(validationContext.ObjectInstance);

            if (comparisonValue == true && currentValue <= 0)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
