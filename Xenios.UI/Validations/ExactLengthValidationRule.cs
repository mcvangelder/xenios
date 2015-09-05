using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Xenios.UI.Validations
{
    public class ExactLengthValidationRule : ValidationRule
    {
        public int Length { get; set; }
        
        public override ValidationResult Validate(object enteredValue, System.Globalization.CultureInfo cultureInfo)
        {
            var value = enteredValue as String;

            return new ValidationResult(value.Length == Length, String.Format("Must be exactly {0} characters long.", Length));
        }
    }
}
