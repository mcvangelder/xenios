using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xenios.UI.Test
{
    [TestClass]
    public class UIDataServiceTest
    {
        private Mocks.MockInsurancePolicyDataService _mockBusinessService;
        private UI.Services.DataService _dataService;

        [TestInitialize]
        public void CreateUIDataService()
        {
            _mockBusinessService = new Mocks.MockInsurancePolicyDataService();
            _dataService = new UI.Services.DataService();
            _dataService.InsurancePolicyDataService = _mockBusinessService;
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
    }
}
