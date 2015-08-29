﻿using System;
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
        private const int defaultInformationsCount = 5;
        private const string defaultFileName = @"c:\temp\insurance_information_retrievalTest.txt";
        private DataAccess.InsurancePolicyRepository _repository;

        private void CreateRepository(String fileName = defaultFileName, int initialRecordCount = defaultInformationsCount)
        {
            DeleteRepository(fileName);
            _repository = new DataAccess.InsurancePolicyRepository(fileName);
            var informations = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(initialRecordCount);
            foreach(var info in informations)
            {
                _repository.Save(info);
            }
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
            Assert.IsTrue(allInfos.Count == defaultInformationsCount);
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
                var newInformation = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy();
                var repo = new DataAccess.InsurancePolicyRepository(defaultFileName);
                repo.Save(newInformation);

                isNotifiedEvent.WaitOne(TimeSpan.FromSeconds(1));
                Assert.AreEqual(1, newInfosCount);
            }
        }

        [TestMethod]
        public void Should_save_and_read_insurance_information()
        {
            CreateRepository(initialRecordCount: 0);

            var expectedPolicy = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy();
            using (var insuranceInformationRetrievalService = new InsurancePolicyDataService(defaultFileName))
            {
                insuranceInformationRetrievalService.Save(expectedPolicy);

                var savedPolicy = insuranceInformationRetrievalService.GetAllInsurancePolicies().Single();
                Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(expectedPolicy, savedPolicy);
            }
        }

        [TestMethod]
        public void Should_find_customer_by_first_name()
        {
            var expectedCount = 1;
            CreateRepository(initialRecordCount: expectedCount);

            var expectedInfo = _repository.GetAll().First();
            var expectedCustomer = expectedInfo.Customer;
            var expectedFirstName = expectedCustomer.FirstName;
            

            using(var service = new InsurancePolicyDataService(defaultFileName))
            {
                var searchResults = service.FindInsurancePoliciesByCustomerName(expectedFirstName);
                var searchResultsCount = searchResults.Count;
                var searchResult = searchResults.First();

                Assert.AreEqual(expectedCount, searchResultsCount);
                Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(expectedInfo, searchResult);
            }
        }
    }
}
