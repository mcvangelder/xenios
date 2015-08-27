using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Xenios.Business.Test
{
    [TestClass]
    public class InsuranceInformationRetrievalTest
    {
        private const string fileName = @"c:\temp\insurance_information_retrievalTest.txt";

        [TestInitialize]
        public void CreateRepositoryWithInformation()
        {
            File.Delete(fileName);
            var information1 = Xenios.Test.Helpers.InsuranceInformationHelper.CreateInsuranceInformation();
            var information2 = Xenios.Test.Helpers.InsuranceInformationHelper.CreateInsuranceInformation();

            var repo = new DataAccess.InsuranceInformationRepository(fileName);
            repo.Save(information1);
            repo.Save(information2);
        }

        [TestMethod]
        public void Should_get_all_insurance_informations()
        {
            Business.InsuranceInformationRetrievalService service = 
                new Business.InsuranceInformationRetrievalService { Source = fileName };

            var allInfos = service.GetAllInsurancePolicies();
            Assert.IsTrue(allInfos.Count == 2);
        }
    }
}
