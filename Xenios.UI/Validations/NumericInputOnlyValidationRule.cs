using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Xenios.UI.Validations
{
    public class NumericInputOnlyValidationRule : ValidationRule
    {
        private static Regex numericExpression = new Regex("^[0-9]*$");
        public override ValidationResult Validate(object enteredValue, System.Globalization.CultureInfo cultureInfo)
        {
            var value = enteredValue as String;
            var isNumeric = numericExpression.IsMatch(value);

            return new ValidationResult(isNumeric, "Only numeric values are allowed.");
        }
    }
}
