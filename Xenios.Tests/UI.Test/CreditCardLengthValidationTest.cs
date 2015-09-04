using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xenios.UI.Infrastructure;

namespace Xenios.UI.Test
{
    [TestClass]
    public class CreditCardLengthValidationTest
    {
        [TestMethod]
        public void Should_validate_value_of_length_16()
        {
            var value = "1234567890123456";
            var validationRule = new CreditCardLengthValidationRule();

            var result = validationRule.Validate(value, null);

            Assert.IsTrue(result.IsValid);
        }

    }
}
