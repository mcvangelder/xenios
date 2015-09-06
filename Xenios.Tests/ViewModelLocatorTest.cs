using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xenios.UI.Test
{
    [TestClass]
    public class ViewModelLocatorTest
    {
        [TestMethod]
        public void Should_create_InsurancePolicyViewModel_with_data_service()
        {
            var locator = new ViewModel.ViewModelLocator();
            var viewModel = locator.InsurancePolicy;

            Assert.IsNotNull(viewModel.PolicyDataService);
        }
    }
}
