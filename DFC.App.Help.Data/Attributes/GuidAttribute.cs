using DFC.App.Help.Data.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.Help.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GuidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!Guid.TryParse(value.ToString(), out var guid) || guid == Guid.Empty)
            {
                return new ValidationResult(string.Format(ValidationMessage.FieldInvalidGuid, validationContext.DisplayName), new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
