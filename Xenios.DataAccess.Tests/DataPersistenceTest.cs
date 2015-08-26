using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Xenios.DataAccess.Tests
{
    [TestClass]
    public class DataPersistenceTest
    {
        private const String fileName = @"c:\temp\insurance_information_storage.txt";

        [TestInitialize]
        [TestCleanup]
        public void DeletePersistenceFile()
        {
            var persistenceFile = new FileInfo(fileName);
            persistenceFile.Delete();
        }

        [TestMethod]
        public void Should_persist_insurance_info()
        {
            var infoService = new DataAccess.InsuranceInformationService(fileName);
            var insuranceInformation = new Domain.Models.InsuranceInformation
            {
                Id = Guid.NewGuid(),
                Customer = new Domain.Models.CustomerInformation
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Smith",
                    AddressLine1 = "123 Some Street",
                    City = "City",
                    State = "State",
                    PostalCode = "12345",
                    CountryOfBirth = "United States",
                },
                PaymentInformation = new Domain.Models.PaymentInformation
                {
                    Id = Guid.NewGuid(),
                    CreditCardType = Domain.Enums.CreditCardTypes.Amex,
                    CreditCardNumber = "1234-5678-9012-3456"
                },
                InsuranceType = 0,
                CoverageBeginDateTime = DateTime.Now,
                Price = (decimal)274.00,
                TermLength = 6,
                TermUnit = 0
            };

            infoService.Save(insuranceInformation);
            var savedInformation = infoService.GetAll().FirstOrDefault(i => i.Id == insuranceInformation.Id);

            Assert.AreEqual(insuranceInformation.Id, savedInformation.Id);
            Assert.AreEqual(insuranceInformation.InsuranceType, savedInformation.InsuranceType);
            Assert.AreEqual(insuranceInformation.Price, savedInformation.Price);
            Assert.AreEqual(insuranceInformation.TermLength, savedInformation.TermLength);
            Assert.AreEqual(insuranceInformation.TermUnit, insuranceInformation.TermUnit);
            Assert.AreEqual(insuranceInformation.CoverageBeginDateTime, savedInformation.CoverageBeginDateTime);

            Assert.AreEqual(insuranceInformation.Customer.Id, savedInformation.Customer.Id);
            Assert.AreEqual(insuranceInformation.Customer.AddressLine1, savedInformation.Customer.AddressLine1);
            Assert.AreEqual(insuranceInformation.Customer.City, savedInformation.Customer.City);
            Assert.AreEqual(insuranceInformation.Customer.FirstName, savedInformation.Customer.FirstName);
            Assert.AreEqual(insuranceInformation.Customer.LastName, savedInformation.Customer.LastName);
            Assert.AreEqual(insuranceInformation.Customer.PostalCode, savedInformation.Customer.PostalCode);
            Assert.AreEqual(insuranceInformation.Customer.State, savedInformation.Customer.State);
            Assert.AreEqual(insuranceInformation.Customer.CountryOfBirth, savedInformation.Customer.CountryOfBirth);

            Assert.AreEqual(insuranceInformation.PaymentInformation.Id, savedInformation.PaymentInformation.Id);
            Assert.AreEqual(insuranceInformation.PaymentInformation.CreditCardNumber, savedInformation.PaymentInformation.CreditCardNumber);
            Assert.AreEqual(insuranceInformation.PaymentInformation.CreditCardType, savedInformation.PaymentInformation.CreditCardType);
        }
    }
}
