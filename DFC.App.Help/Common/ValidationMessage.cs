using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Help.Common
{
    public class ValidationMessage
    {
        public const string FieldEmptyGuid = "The field {0} cannot be an empty guid.";
        public const string FieldNotLowercase = "The field {0} is not in lowercase.";
        public const string FieldNotUrlPath = "The field {0} does not contains valid characters for a url path. Valid characters are {1}";
    }
}
