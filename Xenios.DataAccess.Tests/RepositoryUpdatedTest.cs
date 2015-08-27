using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;

namespace Xenios.DataAccess.Tests
{
    [TestClass]
    public class RepositoryUpdatedTest
    {
        private string _fileName = @"c:\temp\temp_repository.txt";
        private InsuranceInformationRepository repository;

        [TestInitialize]
        public void CreateRepository()
        {
            DeleteRepositoryFile();
            repository = new InsuranceInformationRepository(_fileName);
        }

        [TestCleanup]
        public void DeleteRepositoryFile()
        {
            File.Delete(_fileName);
        }

        [TestMethod]
        public void Should_detect_repository_updates()
        {
            DataAccess.RepositoryUpdatedNotificationService notificationService = 
                new RepositoryUpdatedNotificationService(repository);

            var isNotified = new AutoResetEvent(false);

            notificationService.NotifyRepositoryUpdated += (repo) =>
                {
                    isNotified.Set();
                };

            var insuranceInformation = Helpers.InsuranceInformationHelper.CreateInsuranceInformation();

            repository.Save(insuranceInformation);
            var isNotifiedSet = isNotified.WaitOne(TimeSpan.FromSeconds(1));

            Assert.IsTrue(isNotifiedSet, "NotifyRepositoryUpdated was not called");
        }
    }
}
