using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace Xenios.DataAccess.Tests
{
    [TestClass]
    public class DataPersistenceTest
    {
        private const String fileName = @"\\monticello\home\development\insurance_information_storage.txt";

        private static Domain.Models.InsuranceInformation CreateInsuranceInformation()
        {
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
            var infoService = new DataAccess.InsuranceInformationRepository(fileName);
            var insuranceInformation = CreateInsuranceInformation();

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
            Assert.AreEqual(insuranceInformation.PaymentInformation.CreditCardVerificationNumber, savedInformation.PaymentInformation.CreditCardVerificationNumber);
            Assert.AreEqual(insuranceInformation.PaymentInformation.ExpirationDate, savedInformation.PaymentInformation.ExpirationDate);
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void Should_throw_exception_writing_to_file_already_opened_for_write()
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                var informationService = new InsuranceInformationRepository(fileName);
                var insuranceInformation = CreateInsuranceInformation();
                informationService.Save(insuranceInformation);
            }
        }
    }
}
