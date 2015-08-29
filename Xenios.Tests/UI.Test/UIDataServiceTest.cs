using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xenios.UI.Test
{
    [TestClass]
    public class UIDataServiceTest
    {
        [TestMethod]
        public void Should_call_save_on_business_data_service()
        {
            var mockBusinessService = new Mocks.MockInsurancePolicyDataService();
            var dataService = new UI.Services.DataService();
            dataService.InsurancePolicyDataService = mockBusinessService;
            
            bool isNotified = false;
            mockBusinessService.OnSave += (policies) => { isNotified = true; };

            dataService.Save(Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(1));
            Assert.IsTrue(isNotified);
        }

        [TestMethod]
        public void Should_call_get_all_insurance_policies_on_buisness_data_service()
        {
            var mockBusinessService = new Mocks.MockInsurancePolicyDataService();
            var dataService = new UI.Services.DataService();
            dataService.InsurancePolicyDataService = mockBusinessService;

            bool isNotified = false;
            mockBusinessService.OnGetAllInsurancePolicies += () => { isNotified = true; };

            dataService.GetAllInsurancePolicies();
            Assert.IsTrue(isNotified);
        }
    }
}
