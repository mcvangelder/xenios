﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xenios.Domain.Test
{
    [TestClass]
    public class ModelDefaultTests
    {
        [TestMethod]
        public void Should_create_insurance_policy_with_defaults()
        {
            var insurancePolicy = new Domain.Models.InsurancePolicy();

            Assert.AreNotEqual(DateTime.MinValue, insurancePolicy.CoverageBeginDateTime);
            Assert.IsNotNull(insurancePolicy.Customer);
            Assert.AreNotEqual(Guid.Empty, insurancePolicy.Id);
            Assert.AreEqual(Domain.Enums.InsuranceTypes.Unspecified, insurancePolicy.InsuranceType);
            Assert.IsNotNull(insurancePolicy.PaymentInformation);
            Assert.AreEqual(0, insurancePolicy.Price);
            Assert.AreEqual(0, insurancePolicy.TermLength);
            Assert.AreEqual(Domain.Enums.TermUnits.Months, insurancePolicy.TermUnit);
        }

        [TestMethod]
        public void Should_create_customer_with_defaults()
        {
            var customer = new Domain.Models.CustomerInformation();

            Assert.AreNotEqual(Guid.Empty, customer.Id);
            Assert.IsNull(customer.AddressLine1);
            Assert.IsNull(customer.City);
            Assert.IsNull(customer.Country);
            Assert.IsNull(customer.FirstName);
            Assert.IsNull(customer.LastName);
            Assert.IsNull(customer.PostalCode);
            Assert.IsNull(customer.State);
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
