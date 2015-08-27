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
                Assert.AreEqual(newInfosCount, informationsCount + 1);
            }
        }

        [TestMethod]
        public void Should_save_and_read_insurance_information()
        {
            File.Delete(fileName);

            var insuranceInformation = Xenios.Test.Helpers.InsuranceInformationHelper.CreateInsuranceInformation();
            using (var insuranceInformationRetrievalService = new InsuranceInformationRetrievalService(fileName))
            {
                insuranceInformationRetrievalService.Save(insuranceInformation);
                var savedInformation = insuranceInformationRetrievalService.GetAllInsurancePolicies().Single();
                Assert.AreEqual(insuranceInformation.Id, savedInformation.Id);
                Assert.AreEqual(insuranceInformation.InsuranceType, savedInformation.InsuranceType);
                Assert.AreEqual(insuranceInformation.Price, savedInformation.Price);
                Assert.AreEqual(insuranceInformation.TermLength, savedInformation.TermLength);
                Assert.AreEqual(insuranceInformation.TermUnit, insuranceInformation.TermUnit);
                Assert.AreEqual(insuranceInformation.CoverageBeginDateTime, savedInformation.CoverageBeginDateTime);

                Assert.AreEqual(insuranceInformation.Customer.Id, savedInformation.Customer.Id);
                Assert.AreEqual(insuranceInformation.Customer.AddressLine1, savedInformation.Customer.AddressLine1);
                Assert.AreEqual(insuranceInformation.Customer.City, savedInformation.Customer.City);
                Assert.AreEqual(insuranceInformation.Customer.FirstName, savedInformation.Customer.FirstName);
                Assert.AreEqual(insuranceInformation.Customer.LastName, savedInformation.Customer.LastName);
                Assert.AreEqual(insuranceInformation.Customer.PostalCode, savedInformation.Customer.PostalCode);
                Assert.AreEqual(insuranceInformation.Customer.State, savedInformation.Customer.State);
                Assert.AreEqual(insuranceInformation.Customer.Country, savedInformation.Customer.Country);

                Assert.AreEqual(insuranceInformation.PaymentInformation.Id, savedInformation.PaymentInformation.Id);
                Assert.AreEqual(insuranceInformation.PaymentInformation.CreditCardNumber, savedInformation.PaymentInformation.CreditCardNumber);
                Assert.AreEqual(insuranceInformation.PaymentInformation.CreditCardType, savedInformation.PaymentInformation.CreditCardType);
                Assert.AreEqual(insuranceInformation.PaymentInformation.CreditCardVerificationNumber, savedInformation.PaymentInformation.CreditCardVerificationNumber);
                Assert.AreEqual(insuranceInformation.PaymentInformation.ExpirationDate, savedInformation.PaymentInformation.ExpirationDate);
            }
        }
    }
}
