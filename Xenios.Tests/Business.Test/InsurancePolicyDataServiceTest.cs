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

        [TestMethod]
        public void Should_resolve_edit_conflict_by_taking_record_with_most_recent_update_date()
        {
            int recordCount = 5;
            CreateRepository(initialRecordCount: recordCount);
            
            using(var service = new InsurancePolicyDataService(defaultFileName))
            {
                var originalLoadedRecords = service.GetAllInsurancePolicies();
                var editedRecordKeep = originalLoadedRecords[0];
                var editedRecordDrop = originalLoadedRecords[1];

                DateTime lastEditedDateTime = DateTime.Now;
                editedRecordKeep.LastUpdateDate = lastEditedDateTime.AddMinutes(1);
                editedRecordDrop.LastUpdateDate = lastEditedDateTime.AddMinutes(-1);

                var reloadedRecords = service.GetAllInsurancePolicies();

                var conflictResolvedRecords = service.RefreshPolicies(originalLoadedRecords);
                var conflictResolvedRecordKeep = conflictResolvedRecords.FirstOrDefault(cr => cr.Id == editedRecordKeep.Id);
                var conflictResolvedRecordDrop = conflictResolvedRecords.FirstOrDefault(cr => cr.Id == editedRecordDrop.Id);

                Assert.AreEqual(editedRecordKeep.LastUpdateDate, conflictResolvedRecordKeep.LastUpdateDate);
                Assert.AreNotEqual(editedRecordDrop.LastUpdateDate, conflictResolvedRecordDrop.LastUpdateDate);
            }
        }
    }
}
