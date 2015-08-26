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
            var insuranceInformation = new DomainModels.InsuranceInformation
            {
                Id = Guid.NewGuid(),
                Customer = new DomainModels.CustomerInformation
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
                PaymentInformation = new DomainModels.PaymentInformation
                {
                    Id = Guid.NewGuid(),
                    CreditCardType = 0,
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

            Assert.IsNotNull(savedInformation);
        }
    }
}
