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
        private DataAccess.InsurancePolicyRepository _repository;

        [TestInitialize]
        public void CreateRepository()
        {
            DeleteRepositoryFile();
            _repository = new InsurancePolicyRepository(fileName);
        }

        [TestCleanup]
        public void DeleteRepositoryFile()
        {
            File.Delete(fileName);
        }

        [TestMethod]
        public void Should_persist_insurance_info()
        {
            var insuranceInformation = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(1);

            _repository.SaveAll(insuranceInformation);
            var savedInformation = _repository.GetAll();

            Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(insuranceInformation, savedInformation);
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void Should_throw_exception_writing_to_file_already_opened_for_write()
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                _repository = new InsurancePolicyRepository(fileName);
                var insuranceInformation = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(1);
                _repository.SaveAll(insuranceInformation);
            }
        }

        [TestMethod]
        public void Should_return_last_write_time_on_repository()
        {
            var list = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(1);
            var firstWriteTime = _repository.SaveAll(list);
            // ignore this save all last write time, it simulates another source updating the file
            _repository.SaveAll(list);
            var lastWriteTime = _repository.GetLastWriteTime();

            Assert.AreNotEqual(firstWriteTime, lastWriteTime);
        }
    }
}
