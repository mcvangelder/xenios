using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xenios.UI.Validations;

namespace Xenios.UI.Test
{
    [TestClass]
    public class ValidatorTests
    {
        [TestMethod]
        public void Should_validate_value_of_length_16()
        {
            var value = "1234567890123456";
            var validationRule = new CreditCardLengthValidationRule();

            var result = validationRule.Validate(value, null);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Should_validate_only_numeric_values_permitted()
        {
            var value = "1234567890abcdef";

            var validationRule = new NumericInputOnlyValidationRule();

            var result = validationRule.Validate(value, null);

            Assert.IsFalse(result.IsValid);
        }
    }
}
