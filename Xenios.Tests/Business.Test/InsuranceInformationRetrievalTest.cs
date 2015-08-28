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
    public class InsuranceInformationRetrievalTest
    {
        private const int informationsCount = 5;
        private const string fileName = @"c:\temp\insurance_information_retrievalTest.txt";
        

        private void CreateRepositoryWithDefaultInformation()
        {
            var repo = new DataAccess.InsuranceInformationRepository(fileName);
            var informations = Xenios.Test.Helpers.InsuranceInformationHelper.CreateInsuranceInformations(informationsCount);
            foreach(var info in informations)
            {
                repo.Save(info);
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
            InsuranceInformationRetrievalService service = 
                            new InsuranceInformationRetrievalService(fileName);

            var allInfos = service.GetAllInsurancePolicies();
            Assert.IsTrue(allInfos.Count == informationsCount);
        }

        [TestMethod]
        public void Should_notify_insurance_policies_updated()
        {
            var isNotifiedEvent = new AutoResetEvent(false);
            var newInfosCount = 0;

            using (var service = new InsuranceInformationRetrievalService(fileName))
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
                Assert.AreEqual(newInfosCount, 1);
            }
        }

        [TestMethod]
        public void Should_save_and_read_insurance_information()
        {
            DeleteRepository();

            var insuranceInformation = Xenios.Test.Helpers.InsuranceInformationHelper.CreateInsuranceInformation();
            using (var insuranceInformationRetrievalService = new InsuranceInformationRetrievalService(fileName))
            {
                insuranceInformationRetrievalService.Save(insuranceInformation);

                var savedInformation = insuranceInformationRetrievalService.GetAllInsurancePolicies().Single();
                Xenios.Test.Helpers.InsuranceInformationHelper.AssertAreEqual(insuranceInformation, savedInformation);
            }
        }
    }
}
