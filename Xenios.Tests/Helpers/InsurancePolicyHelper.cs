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
                    CreditCardNumber = "1234-5678-9012-3456",
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

        public static void AssertPolicyCanBeFoundByCustomerName(int expectedCount, InsurancePolicy expectedPolicy, string customerName, string repositoryFile)
        {
            using (var service = new InsurancePolicyDataService(repositoryFile))
            {
                var searchResults = service.FindInsurancePoliciesByCustomerName(customerName);
                var searchResultsCount = searchResults.Count;

                Assert.AreEqual(expectedCount, searchResultsCount);

                var searchResult = searchResults.First();
                Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(expectedPolicy, searchResult);
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
    }
}
