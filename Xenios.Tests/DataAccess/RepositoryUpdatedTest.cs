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
        private InsurancePolicyRepository repository;

        [TestInitialize]
        public void CreateRepository()
        {
            DeleteRepositoryFile();
            repository = new InsurancePolicyRepository(_fileName);
        }

        [TestCleanup]
        public void DeleteRepositoryFile()
        {
            File.Delete(_fileName);
        }

        [TestMethod]
        public void Should_detect_repository_updates()
        {
            using (var notificationService =  new RepositoryUpdatedNotificationService(repository))
            {
                var isNotified = new AutoResetEvent(false);

                notificationService.NotifyRepositoryUpdated += (repo) =>
                    {
                        isNotified.Set();
                    };

                var insurancePolicy = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(1);

                repository.SaveAll(insurancePolicy);
                var isNotifiedSet = isNotified.WaitOne(Constants.WaitTimeOut);

                Assert.IsTrue(isNotifiedSet, "NotifyRepositoryUpdated was not called");
            }
        }
    }
}
