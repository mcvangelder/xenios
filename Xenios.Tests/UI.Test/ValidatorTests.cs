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

            var validationRule = new ExactLengthValidationRule();
            validationRule.Length = 16;
            var result = validationRule.Validate(value, null);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Should_not_validate_value_of_less_than_16()
        {
            var value = "1234567890";

            var validationRule = new ExactLengthValidationRule();
            validationRule.Length = 16;
            var result = validationRule.Validate(value, null);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.ErrorContent);
        }

        [TestMethod]
        public void Should_not_validate_value_of_more_than_16()
        {
            var value = "12345678901234567";

            var validationRule = new ExactLengthValidationRule();
            validationRule.Length = 16;
            var result = validationRule.Validate(value, null);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.ErrorContent);

        }

        [TestMethod]
        public void Should_validate_only_numeric_values_present()
        {
            var value = "1234567890";

            var validationRule = new NumericInputOnlyValidationRule();

            var result = validationRule.Validate(value, null);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Should_not_validate_when_non_numeric_characters_are_present()
        {
            var value = "12345abd";

            var validationRule = new NumericInputOnlyValidationRule();
            var result = validationRule.Validate(value, null);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.ErrorContent);
        }

        [TestMethod]
        public void Should_validate_when_input_only_has_ascii_characters()
        {
            var value = "1234MaE*";

            var validationRule = new AsciiInputOnlyValidationRule();
            var result = validationRule.Validate(value, null);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Should_not_validate_when_input_contains_non_ascii_characters()
        {
            var value = "\u03a0 = Pi";

            var validationRule = new AsciiInputOnlyValidationRule();
            var result = validationRule.Validate(value, null);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.ErrorContent);
        }

        [TestMethod]
        public void Should_validate_only_when_input_has_been_specified()
        {
            var value = "entered input";

            var validationRule = new RequiredValidationRule();
            var result = validationRule.Validate(value, null);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Should_not_validate_when_input_is_empty()
        {
            var value = "";

            var validationRule = new RequiredValidationRule();
            var result = validationRule.Validate(value, null);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.ErrorContent);

        }

        [TestMethod]
        public void Should_not_validate_when_input_is_null()
        {
            String value = null;
            var validationRule = new RequiredValidationRule();
            var result = validationRule.Validate(value, null);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.ErrorContent);
        }
    }
}
