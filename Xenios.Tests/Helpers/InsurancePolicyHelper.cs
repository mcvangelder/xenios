using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Business;
using Xenios.Domain.Models;

namespace Xenios.Test.Helpers
{
    public class InsurancePolicyHelper
    {
        public static List<Xenios.Domain.Models.InsurancePolicy> CreateInsurancePolicies(int count)
        {
            var result = new List<Xenios.Domain.Models.InsurancePolicy>(count);
            for (var i = 0; i < count; i++)
                result.Add(CreateInsurancePolicy());

            return result;
        }

        public static Domain.Models.InsurancePolicy CreateInsurancePolicy()
        {
            var guidString = Guid.NewGuid().ToString();

            var insuranceInformation = new Domain.Models.InsurancePolicy
            {
                Id = Guid.NewGuid(),
                Customer = new Domain.Models.CustomerInformation
                {
                    Id = Guid.NewGuid(),
                    FirstName = guidString.Substring(0, 8),
                    LastName = guidString.Substring(20),
                    AddressLine1 = "123 Some Street",
                    City = guidString.Substring(9, 4),
                    State = guidString.Substring(14, 4),
                    PostalCode = "12345",
                    Country = "United States",
                },
                PaymentInformation = new Domain.Models.PaymentInformation
                {
                    Id = Guid.NewGuid(),
                    CreditCardType = Domain.Enums.CreditCardTypes.Amex,
                    CreditCardNumber = "1234567890123456",
                    ExpirationDate = DateTime.Now,
                    CreditCardVerificationNumber = "001"
                },
                InsuranceType = Domain.Enums.InsuranceTypes.Comprehensive,
                CoverageBeginDateTime = DateTime.Now,
                Price = (decimal)274.00,
                TermLength = 6,
                TermUnit = Domain.Enums.TermUnits.Months
            };
            return insuranceInformation;
        }

        // TODO MVG - This does belong in helper method as it is testing the DataService explicitly. Move this to the appropriate test class
        public static void AssertPolicyCanBeFoundByCustomerName(List<InsurancePolicy> expectedPolicies, string customerName, string repositoryFile)
        {
            using (var service = new InsurancePolicyDataService(repositoryFile))
            {
                var actualPolicies = service.FindInsurancePoliciesByCustomerName(customerName);

                AssertAreEqual(expectedPolicies, actualPolicies);
            }
        }

        public static void AssertAreEqual(InsurancePolicy expected, InsurancePolicy actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.InsuranceType, actual.InsuranceType);
            Assert.AreEqual(expected.Price, actual.Price);
            Assert.AreEqual(expected.TermLength, actual.TermLength);
            Assert.AreEqual(expected.TermUnit, expected.TermUnit);
            Assert.AreEqual(expected.CoverageBeginDateTime, actual.CoverageBeginDateTime);

            Assert.AreEqual(expected.Customer.Id, actual.Customer.Id);
            Assert.AreEqual(expected.Customer.AddressLine1, actual.Customer.AddressLine1);
            Assert.AreEqual(expected.Customer.City, actual.Customer.City);
            Assert.AreEqual(expected.Customer.FirstName, actual.Customer.FirstName);
            Assert.AreEqual(expected.Customer.LastName, actual.Customer.LastName);
            Assert.AreEqual(expected.Customer.PostalCode, actual.Customer.PostalCode);
            Assert.AreEqual(expected.Customer.State, actual.Customer.State);
            Assert.AreEqual(expected.Customer.Country, actual.Customer.Country);

            Assert.AreEqual(expected.PaymentInformation.Id, actual.PaymentInformation.Id);
            Assert.AreEqual(expected.PaymentInformation.CreditCardNumber, actual.PaymentInformation.CreditCardNumber);
            Assert.AreEqual(expected.PaymentInformation.CreditCardType, actual.PaymentInformation.CreditCardType);
            Assert.AreEqual(expected.PaymentInformation.CreditCardVerificationNumber, actual.PaymentInformation.CreditCardVerificationNumber);
            Assert.AreEqual(expected.PaymentInformation.ExpirationDate, actual.PaymentInformation.ExpirationDate);
        }

        public static void AssertAreEqual(List<Domain.Models.InsurancePolicy> expectedPolicies,List<Domain.Models.InsurancePolicy> actualPolicies)
        {
            int expectedPoliciesCount = expectedPolicies.Count, actualPoliciesCount = actualPolicies.Count;

            Assert.AreEqual(expectedPoliciesCount, actualPoliciesCount);
            for (int i = 0; i < expectedPoliciesCount; i++)
            {
                var expectedPolicy = expectedPolicies[0];
                var actualPolicy = actualPolicies[0];
                Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(expectedPolicy, actualPolicy);
            }
        }
    }
}
