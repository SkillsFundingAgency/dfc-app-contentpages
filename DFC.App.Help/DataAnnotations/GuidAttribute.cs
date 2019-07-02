using System;
using System.ComponentModel.DataAnnotations;
using DFC.App.Help.Common;

namespace DFC.App.Help.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GuidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = Guid.Parse(value.ToString());
            if (result == Guid.Empty)
            {
                return new ValidationResult(string.Format(ValidationMessage.FieldEmptyGuid, validationContext.DisplayName), new string[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
