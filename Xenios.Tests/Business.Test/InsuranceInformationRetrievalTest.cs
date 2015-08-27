using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Xenios.Domain.Models;
using System.Collections.Generic;

namespace Xenios.Business.Test
{
    [TestClass]
    public class InsuranceInformationRetrievalTest
    {
        private const int informationsCount = 5;
        private const string fileName = @"c:\temp\insurance_information_retrievalTest.txt";
        

        [TestInitialize]
        public void CreateRepositoryWithInformation()
        {
            File.Delete(fileName);

            var repo = new DataAccess.InsuranceInformationRepository(fileName);
            var informations = Xenios.Test.Helpers.InsuranceInformationHelper.CreateInsuranceInformations(informationsCount);
            foreach(var info in informations)
            {
                repo.Save(info);
            }
        }

        [TestMethod]
        public void Should_get_all_insurance_informations()
        {
            Business.InsuranceInformationRetrievalService service = 
                            new Business.InsuranceInformationRetrievalService(fileName);

            var allInfos = service.GetAllInsurancePolicies();
            Assert.IsTrue(allInfos.Count == informationsCount);
        }

        [TestMethod]
        public void Should_notify_new_insurance_policies_available()
        {
            throw new NotImplementedException();
        }
    }
}
