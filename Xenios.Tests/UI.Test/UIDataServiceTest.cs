using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xenios.UI.Test
{
    [TestClass]
    public class UIDataServiceTest
    {
        private Mocks.MockInsurancePolicyDataService _mockBusinessService;
        private UI.Services.DataService<Mocks.MockInsurancePolicyDataService> _dataService;

        private const string mockFilePath = @"mock/file/path";

        [TestInitialize]
        public void CreateUIDataService()
        {
            _dataService = new UI.Services.DataService<Mocks.MockInsurancePolicyDataService>();
            _dataService.SourceFile = mockFilePath;
            _mockBusinessService = _dataService.InsurancePolicyDataService;
        }

        [TestMethod]
        public void Should_call_save_on_business_data_service()
        {
            bool isNotified = false;
            _mockBusinessService.OnSave += (policies) => { isNotified = true; };

            _dataService.Save(Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(1));
            Assert.IsTrue(isNotified);
        }

        [TestMethod]
        public void Should_call_get_all_insurance_policies_on_buisness_data_service()
        {
            bool isNotified = false;
            _mockBusinessService.OnGetAllInsurancePolicies += () => { isNotified = true; };

            _dataService.GetAllInsurancePolicies();
            Assert.IsTrue(isNotified);
        }

        [TestMethod]
        public void Should_call_find_insurance_policies_by_customer_name_on_business_data_service()
        {
            bool isNotified = false;
            _mockBusinessService.OnFindInsurancePoliciesByCustomerName += () => { isNotified = true; };

            _dataService.FindInsurancePoliciesByCustomerName("ignored");
            Assert.IsTrue(isNotified);
        }

        [TestMethod]
        public void Should_create_new_business_data_service_when_source_file_changes()
        {
            var previousBusinessService = _dataService.InsurancePolicyDataService;
            _dataService.SourceFile = "another/mock/path";
            var currentBusinessService = _dataService.InsurancePolicyDataService;

            Assert.AreNotSame(previousBusinessService, currentBusinessService);
        }

        [TestMethod]
        public void Should_dispose_previous_business_data_service_when_source_file_changes()
        {
            var isNotified = false;
            _mockBusinessService.OnDispose += () => { isNotified = true; };

            _dataService.SourceFile = "another/mock/path";
            Assert.IsTrue(isNotified);
        }
    }
}
