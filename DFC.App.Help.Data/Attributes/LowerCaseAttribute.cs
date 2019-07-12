using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DFC.App.Help.Data.Common;

namespace DFC.App.Help.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LowerCaseAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var result = false;

            switch (value)
            {
                case IEnumerable<string> list:
                    result = list.All(s => s.Equals(s.ToLower()));
                    break;
                default:
                    result = value.ToString().Equals(value.ToString().ToLower());
                    break;
            }

            return result ? ValidationResult.Success
                : new ValidationResult(string.Format(ValidationMessage.FieldNotLowercase, validationContext.DisplayName), new[] { validationContext.MemberName });
        }
    }
}
