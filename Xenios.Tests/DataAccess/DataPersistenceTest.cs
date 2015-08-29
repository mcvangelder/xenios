using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace Xenios.DataAccess.Tests
{
    [TestClass]
    public class DataPersistenceTest
    {
        private const String fileName = @"c:\temp\insurance_information_storage.txt";

        [TestInitialize]
        [TestCleanup]
        public void DeleteRepositoryFile()
        {
            File.Delete(fileName);
        }

        [TestMethod]
        public void Should_persist_insurance_info()
        {
            var infoRepo = new DataAccess.InsurancePolicyRepository(fileName);
            var insuranceInformation = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy();

            infoRepo.Save(insuranceInformation);
            var savedInformation = infoRepo.GetAll().FirstOrDefault(i => i.Id == insuranceInformation.Id);

            Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(insuranceInformation, savedInformation);
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void Should_throw_exception_writing_to_file_already_opened_for_write()
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                var informationService = new InsurancePolicyRepository(fileName);
                var insuranceInformation = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy();
                informationService.Save(insuranceInformation);
            }
        }
    }
}
