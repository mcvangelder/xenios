using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Xenios.UI.Validations
{
    public class RequiredValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var inputStr = value as String;

            return new ValidationResult(!String.IsNullOrEmpty(inputStr), "Field is required.");
        }
    }
}
