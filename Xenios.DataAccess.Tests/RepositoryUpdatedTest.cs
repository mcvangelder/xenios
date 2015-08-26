using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;

namespace Xenios.DataAccess.Tests
{
    [TestClass]
    public class RepositoryUpdatedTest
    {
        private string _fileName = @"temp_repository.txt";
        private InsuranceInformationRepository repository;

        [TestInitialize]
        public void CreateRepository()
        {
            repository = new InsuranceInformationRepository(_fileName);
        }

        [TestCleanup]
        public void DeleteRepository()
        {
            File.Delete(_fileName);
        }

        [TestMethod]
        public void Should_detect_repository_updates()
        {
            DataAccess.RepositoryUpdatedNotificationService notificationService = 
                new RepositoryUpdatedNotificationService(repository);

            var repositoryUpdatedCalled = new ManualResetEvent(false);

            notificationService.NotifyRepositoryUpdated += (repo) =>
                {
                    repositoryUpdatedCalled.Set();
                };

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

            repository.Save(insuranceInformation);
            var isNotified = repositoryUpdatedCalled.WaitOne(TimeSpan.FromSeconds(1));
            Assert.IsTrue(isNotified, "NotifyRepositoryUpdated was not called");

        }
    }
}
