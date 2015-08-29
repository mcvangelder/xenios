using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Xenios.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Xenios.Business.Test
{
    [TestClass]
    public class InsurancePolicyDataServiceTest
    {
        private const int defaultPolicyCount = 5;
        private const string defaultFileName = @"c:\temp\insurance_information_retrievalTest.txt";
        private DataAccess.InsurancePolicyRepository _repository;

        private void CreateRepository(List<InsurancePolicy> policies, String fileName = defaultFileName)
        {
            DeleteRepository(fileName);
            _repository = new DataAccess.InsurancePolicyRepository(fileName);
            _repository.SaveAll(policies);
        }

        private void CreateRepository(int initialRecordCount = defaultPolicyCount, String fileName = defaultFileName)
        {
            CreateRepository(Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(initialRecordCount), fileName);
        }

        public void DeleteRepository(String fileName = defaultFileName)
        {
            File.Delete(fileName);
        }

        [TestMethod]
        public void Should_get_all_insurance_informations()
        {
            CreateRepository();
            InsurancePolicyDataService service =
                            new InsurancePolicyDataService(defaultFileName);

            var allInfos = service.GetAllInsurancePolicies();
            Assert.IsTrue(allInfos.Count == defaultPolicyCount);
        }

        [TestMethod]
        public void Should_notify_insurance_policies_updated()
        {
            CreateRepository(initialRecordCount: 0);

            var isNotifiedEvent = new AutoResetEvent(false);
            var newInfosCount = 0;

            using (var service = new InsurancePolicyDataService(defaultFileName))
            {
                service.NotifyInsurancePolicyUpdated += (infos) =>
                {
                    newInfosCount = infos.Count;
                    isNotifiedEvent.Set();
                };

                // Add a new policy record directly through an unmonitored repository to simulate an external process
                // updating the repository
                var newInformation = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(1);
                var repo = new DataAccess.InsurancePolicyRepository(defaultFileName);
                repo.SaveAll(newInformation);

                isNotifiedEvent.WaitOne(TimeSpan.FromSeconds(1));
                Assert.AreEqual(1, newInfosCount);
            }
        }

        [TestMethod]
        public void Should_save_and_read_insurance_information()
        {
            CreateRepository(initialRecordCount: 0);

            var expectedPolicy = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(1);
            using (var service = new InsurancePolicyDataService(defaultFileName))
            {
                service.Save(expectedPolicy);

                var savedPolicy = service.GetAllInsurancePolicies();
                Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(expectedPolicy, savedPolicy);
            }
        }

        [TestMethod]
        public void Should_find_policy_having_customer_with_first_name()
        {
            var expectedCount = 1;
            CreateRepository(initialRecordCount: expectedCount);

            var expectedInfo = _repository.GetAll().First();
            var expectedCustomer = expectedInfo.Customer;
            var expectedFirstName = expectedCustomer.FirstName;

            Xenios.Test.Helpers.InsurancePolicyHelper.
                AssertPolicyCanBeFoundByCustomerName(
                    new List<InsurancePolicy> {expectedInfo}, expectedFirstName, defaultFileName);
        }

        [TestMethod]
        public void Should_find_policy_having_customer_with_last_name()
        {
            var expectedCount = 1;
            CreateRepository(initialRecordCount: expectedCount);

            var expectedInfo = _repository.GetAll().First();
            var expectedCustomer = expectedInfo.Customer;
            var expectedLastName = expectedCustomer.LastName;

            Xenios.Test.Helpers.InsurancePolicyHelper.
                AssertPolicyCanBeFoundByCustomerName(
                    new List<InsurancePolicy>() { expectedInfo }, expectedLastName, defaultFileName);
        }

        [TestMethod]
        public void Should_find_policy_having_customer_containing_first_or_last_name()
        {
            var expectedName = "expected_name";
            var expectedPartial = "expected";

            var randomPolicies = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(2);
            var expectedInfoByFirstName = randomPolicies.First();
            var expectedInfoByLastName = randomPolicies.Last();

            expectedInfoByFirstName.Customer.FirstName = expectedName;
            expectedInfoByLastName.Customer.LastName = expectedName;

            CreateRepository(policies: randomPolicies);

            Xenios.Test.Helpers.InsurancePolicyHelper.
                AssertPolicyCanBeFoundByCustomerName(
                    randomPolicies, expectedPartial, defaultFileName);
        }
    }
}
