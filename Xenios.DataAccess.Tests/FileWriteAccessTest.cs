using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xenios.DataAccess.Tests
{
    [TestClass]
    public class FileWriteAccessTest
    {
        [TestMethod]
        public void Should_persist_insurance_info()
        {
            var fileName = "insurance_information_storage.txt";
            var infoService = new DataAccess.InsuranceInformationService(fileName);
            var insuranceInformation = new DomainModels.InsuranceInformation
            {
                Id = Guid.NewGuid(),
                CustomerFirstName = "John",
                CustomerLastName = "Smith",
                AddressLine1 = "123 Some Street",
                City = "City",
                State = "State",
                PostalCode = "12345",
                InsuranceType = 0,
                CoverageBeginDateTime = DateTime.Now,
                CreditCardType = 0,
                CreditCardNumber = "1234-5678-9012-3456",
                CountryOfBirth = "United States",
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
