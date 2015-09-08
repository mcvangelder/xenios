using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xenios.Domain.Models;
using Xenios.Domain.Enums;

namespace Xenios.Domain.Test
{
    [TestClass]
    public class ModelFactoryTests
    {

        [TestMethod]
        public void Should_create_policy_with_non_null_values()
        {
            InsurancePolicy policy = InsurancePolicy.NewInsurancePolicy();

            #region Policy
            Assert.AreNotEqual(Guid.Empty, policy.Id);
            Assert.IsNotNull(policy.Customer);
            Assert.AreNotEqual(DateTime.MinValue, policy.CoverageBeginDateTime);
            Assert.AreEqual(InsuranceTypes.Unspecified, policy.InsuranceType);
            Assert.IsNotNull(policy.PaymentInformation);
            Assert.AreEqual(0, policy.Price);
            Assert.AreEqual(0, policy.TermLength);
            Assert.AreEqual(TermUnits.Months, policy.TermUnit);
            Assert.AreNotEqual(DateTime.MinValue, policy.LastUpdateDate);
            #endregion Policy

            #region Customer
            Assert.AreNotEqual(Guid.Empty, policy.Customer.Id);
            Assert.IsNotNull(policy.Customer.AddressLine1);
            Assert.IsNotNull(policy.Customer.City);
            Assert.IsNotNull(policy.Customer.Country);
            Assert.IsNotNull(policy.Customer.FirstName);
            Assert.IsNotNull(policy.Customer.LastName);
            Assert.IsNotNull(policy.Customer.PostalCode);
            Assert.IsNotNull(policy.Customer.State);
            #endregion Customer

            #region PaymentInformation
            Assert.AreNotEqual(Guid.Empty, policy.PaymentInformation.Id);
            Assert.IsNotNull(policy.PaymentInformation.CreditCardNumber);
            Assert.IsNotNull(policy.PaymentInformation.CreditCardVerificationNumber);
            Assert.AreEqual(CreditCardTypes.Unspecified, policy.PaymentInformation.CreditCardType);
            #endregion PaymentInformation
        }

        [TestMethod]
        public void Should_create_payment_information_with_defaults()
        {
            var payment = new Domain.Models.PaymentInformation();

            Assert.IsNull(payment.CreditCardNumber);
            Assert.AreEqual(Domain.Enums.CreditCardTypes.Unspecified, payment.CreditCardType);
            Assert.IsNull(payment.CreditCardVerificationNumber);
            Assert.AreNotEqual(DateTime.MinValue, payment.ExpirationDate);
            Assert.AreNotEqual(Guid.Empty, payment.Id);
        }
    }
}
