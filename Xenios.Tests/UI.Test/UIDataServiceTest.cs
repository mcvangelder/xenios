using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xenios.UI.Test
{
    [TestClass]
    public class UIDataServiceTest
    {
        private Mocks.MockInsurancePolicyDataService _mockBusinessService;
        private UI.Services.PolicyDataServiceWrapper<Mocks.MockInsurancePolicyDataService> _dataServiceWrapper;

        private const string mockFilePath = @"mock/file/path";

        [TestInitialize]
        public void CreateUIDataService()
        {
            _dataServiceWrapper = new UI.Services.PolicyDataServiceWrapper<Mocks.MockInsurancePolicyDataService>();
            _dataServiceWrapper.SourceFile = mockFilePath;
            _mockBusinessService = _dataServiceWrapper.InsurancePolicyDataService;
        }

        // TODO MVG: Test clean up to dispose data service

        [TestMethod]
        public void Should_call_save_on_business_data_service()
        {
            bool isNotified = false;
            _mockBusinessService.OnSave += (policies) => { isNotified = true; };

            _dataServiceWrapper.Save(Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(1));
            Assert.IsTrue(isNotified);
        }

        [TestMethod]
        public void Should_call_get_all_insurance_policies_on_buisness_data_service()
        {
            bool isNotified = false;
            _mockBusinessService.OnGetAllInsurancePolicies += () => { isNotified = true; };

            _dataServiceWrapper.GetAllInsurancePolicies();
            Assert.IsTrue(isNotified);
        }

        [TestMethod]
        public void Should_call_find_insurance_policies_by_customer_name_on_business_data_service()
        {
            bool isNotified = false;
            _mockBusinessService.OnFindInsurancePoliciesByCustomerName += () => { isNotified = true; };

            _dataServiceWrapper.FindInsurancePoliciesByCustomerName("ignored");
            Assert.IsTrue(isNotified);
        }

        [TestMethod]
        public void Should_create_new_business_data_service_when_source_file_changes()
        {
            var previousBusinessService = _dataServiceWrapper.InsurancePolicyDataService;
            _dataServiceWrapper.SourceFile = "another/mock/path";
            var currentBusinessService = _dataServiceWrapper.InsurancePolicyDataService;

            Assert.AreNotSame(previousBusinessService, currentBusinessService);
        }

        [TestMethod]
        public void Should_dispose_previous_business_data_service_when_source_file_changes()
        {
            var isNotified = false;
            _mockBusinessService.OnDispose += () => { isNotified = true; };

            _dataServiceWrapper.SourceFile = "another/mock/path";
            Assert.IsTrue(isNotified);
        }

        [TestMethod]
        public void Should_notify_when_changes_to_policy_repository_are_made()
        {
            var isNotified = false;

            _dataServiceWrapper.PoliciesChanged += () => { isNotified = true; };
            _mockBusinessService.RaisePoliciesChanged();

            Assert.IsTrue(isNotified);
        }

        [TestMethod]
        public void Should_dispose_and_remove_business_data_service_when_source_file_is_null_or_empty()
        {
            var isNotified = false;

            _dataServiceWrapper.InsurancePolicyDataService.OnDispose += () => { isNotified = true; };
            _dataServiceWrapper.SourceFile = String.Empty;

            Assert.IsTrue(isNotified);
            Assert.IsNull(_dataServiceWrapper.InsurancePolicyDataService);
        }

        [TestMethod]
        public void Should_call_refresh_policies_on_business_data_service()
        {
            var isNotified = false;

            _dataServiceWrapper.InsurancePolicyDataService.OnRefreshInsurancePolicies += () => { isNotified = true; };
            _dataServiceWrapper.RefreshPolicies(Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(5));

            Assert.IsTrue(isNotified);
        }
    }
}
