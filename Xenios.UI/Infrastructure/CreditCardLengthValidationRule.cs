using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Xenios.UI.Infrastructure
{
    public class CreditCardLengthValidationRule : ValidationRule
    {
        private const int acceptedLength = 16;
        public override ValidationResult Validate(object enteredValue, System.Globalization.CultureInfo cultureInfo)
        {
            var value = enteredValue as String;

            return new ValidationResult(value.Length == acceptedLength, "Invalid credit card number length");
        }
    }
}
