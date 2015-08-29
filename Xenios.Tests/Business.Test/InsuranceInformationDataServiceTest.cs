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
    public class InsuranceInformationDataServiceTest
    {
        private const int informationsCount = 5;
        private const string fileName = @"c:\temp\insurance_information_retrievalTest.txt";
        private DataAccess.InsuranceInformationRepository _repository;

        private void CreateRepositoryWithDefaultInformation()
        {
            _repository = new DataAccess.InsuranceInformationRepository(fileName);
            var informations = Xenios.Test.Helpers.InsuranceInformationHelper.CreateInsuranceInformations(informationsCount);
            foreach(var info in informations)
            {
                _repository.Save(info);
            }
        }

        [TestInitialize]

        public void DeleteRepository()
        {
            File.Delete(fileName);
        }

        [TestMethod]
        public void Should_get_all_insurance_informations()
        {
            CreateRepositoryWithDefaultInformation();
            InsuranceInformationDataService service = 
                            new InsuranceInformationDataService(fileName);

            var allInfos = service.GetAllInsurancePolicies();
            Assert.IsTrue(allInfos.Count == informationsCount);
        }

        [TestMethod]
        public void Should_notify_insurance_policies_updated()
        {
            var isNotifiedEvent = new AutoResetEvent(false);
            var newInfosCount = 0;

            using (var service = new InsuranceInformationDataService(fileName))
            {
                service.NotifyInsuranceInformationUpdated += (infos) =>
                {
                    newInfosCount = infos.Count;
                    isNotifiedEvent.Set();
                };

                var newInformation = Xenios.Test.Helpers.InsuranceInformationHelper.CreateInsuranceInformation();
                var repo = new DataAccess.InsuranceInformationRepository(fileName);
                repo.Save(newInformation);

                isNotifiedEvent.WaitOne(TimeSpan.FromSeconds(1));
                Assert.AreEqual(1, newInfosCount);
            }
        }

        [TestMethod]
        public void Should_save_and_read_insurance_information()
        {
            DeleteRepository();

            var insuranceInformation = Xenios.Test.Helpers.InsuranceInformationHelper.CreateInsuranceInformation();
            using (var insuranceInformationRetrievalService = new InsuranceInformationDataService(fileName))
            {
                insuranceInformationRetrievalService.Save(insuranceInformation);

                var savedInformation = insuranceInformationRetrievalService.GetAllInsurancePolicies().Single();
                Xenios.Test.Helpers.InsuranceInformationHelper.AssertAreEqual(insuranceInformation, savedInformation);
            }
        }

        [TestMethod]
        public void Should_find_customer_by_first_name()
        {
            DeleteRepository();

            var expectedInfo = Xenios.Test.Helpers.InsuranceInformationHelper.CreateInsuranceInformation();
            _repository = new DataAccess.InsuranceInformationRepository(fileName);
            _repository.Save(expectedInfo);

            var expectedCustomer = expectedInfo.Customer;
            var expectedFirstName = expectedCustomer.FirstName;
            var expectedCount = 1;

            using(var service = new InsuranceInformationDataService(fileName))
            {
                var searchResults = service.FindInsurancePoliciesByCustomerName(expectedFirstName);
                var searchResultsCount = searchResults.Count;
                var searchResult = searchResults.First();

                Assert.AreEqual(expectedCount, searchResultsCount);
                Xenios.Test.Helpers.InsuranceInformationHelper.AssertAreEqual(expectedInfo, searchResult);
            }
        }
    }
}
