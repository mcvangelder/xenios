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
            using (InsurancePolicyDataService service =
                            new InsurancePolicyDataService(defaultFileName))
            {

                var allInfos = service.GetAllInsurancePolicies();
                Assert.IsTrue(allInfos.Count == defaultPolicyCount);
            }
        }

        [TestMethod]
        public void Should_notify_insurance_policies_updated()
        {
            CreateRepository(initialRecordCount: 0);

            var isNotifiedEvent = new AutoResetEvent(false);

            using (var service = new InsurancePolicyDataService(defaultFileName))
            {
                service.NotifyInsurancePoliciesUpdated += () =>
                {
                    isNotifiedEvent.Set();
                };

                // Add a new policy record directly through an unmonitored repository to simulate an external process
                // updating the repository
                var newInformation = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(1);
                var repo = new DataAccess.InsurancePolicyRepository(defaultFileName);
                repo.SaveAll(newInformation);

                bool isNotified = isNotifiedEvent.WaitOne(Constants.WaitTimeOut);
                Assert.IsTrue(isNotified);
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

            AssertPolicyCanBeFoundByCustomerName(
                    new List<InsurancePolicy> { expectedInfo }, expectedFirstName);
        }

        [TestMethod]
        public void Should_find_policy_having_customer_with_last_name()
        {
            var expectedCount = 1;
            CreateRepository(initialRecordCount: expectedCount);

            var expectedInfo = _repository.GetAll().First();
            var expectedCustomer = expectedInfo.Customer;
            var expectedLastName = expectedCustomer.LastName;

            AssertPolicyCanBeFoundByCustomerName(
                    new List<InsurancePolicy>() { expectedInfo }, expectedLastName);
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

            AssertPolicyCanBeFoundByCustomerName(
                randomPolicies, expectedPartial);
        }

        private static void AssertPolicyCanBeFoundByCustomerName(List<InsurancePolicy> expectedPolicies, string customerName)
        {
            using (var service = new InsurancePolicyDataService(defaultFileName))
            {
                var actualPolicies = service.FindInsurancePoliciesByCustomerName(customerName);
                Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(expectedPolicies, actualPolicies);
            }
        }

        [TestMethod]
        public void Should_merge_existing_records_and_source_records_during_refresh()
        {
            int recordCount = 5;
            CreateRepository(initialRecordCount: recordCount);
            var existingRecords = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(recordCount);

            using(var service = new InsurancePolicyDataService(defaultFileName))
            {
                var sourceRecords = service.GetAllInsurancePolicies();
                var mergedRecords = service.RefreshPolicies(existingRecords);

                Assert.AreEqual(recordCount * 2, mergedRecords.Count);
            }
        }
    }
}
