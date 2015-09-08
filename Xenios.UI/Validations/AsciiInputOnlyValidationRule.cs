using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Xenios.UI.Validations
{
   public class AsciiInputOnlyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var inputStr = value as String;
            
            // ASCII encoding replaces non-ascii with question marks, so we use UTF8 to see if multi-byte sequences are there
            var isAscii = Encoding.UTF8.GetByteCount(inputStr) == inputStr.Length;

            return new ValidationResult(isAscii, "Only ascii characters are allowed");
        }
    }
}
