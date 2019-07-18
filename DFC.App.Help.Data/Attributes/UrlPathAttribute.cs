using DFC.App.Help.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DFC.App.Help.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UrlPathAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var validChars = "abcdefghijklmnopqrstuvwxyz01234567890_-";
            var result = false;
            switch (value)
            {
                case IEnumerable<string> list:
                    result = list.All(x => x.Length > 0 && x.All(y => validChars.Contains(y)));
                    break;
                default:
                    result = value.ToString().All(x => validChars.Contains(x));
                    break;
            }

            return result ? ValidationResult.Success
                : new ValidationResult(string.Format(ValidationMessage.FieldNotUrlPath, validationContext.DisplayName, validChars), new[] { validationContext.MemberName });
        }
    }
}
