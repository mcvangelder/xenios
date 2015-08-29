using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Xenios.UI.Test
{
    [TestClass]
    public class InsurancePolicyViewModelTest
    {
        [TestMethod]
        public void Should_open_file_and_load_insurance_informations()
        {
            var dataService =  new Xenios.Mocks.MockDataService();
            
            ViewModel.InsurancePolicyViewModel viewModel =
               new ViewModel.InsurancePolicyViewModel(dataService);
            
            var isNotified = false;

            viewModel.InsurancePolicies.CollectionChanged += (sender, args) => {
                isNotified = true;
            };

            viewModel.PathToFile = @"\mockpath\mockfile";
            
            var isUpdated = viewModel.InsurancePolicies.Count > 0;

            Assert.AreEqual(viewModel.PathToFile, dataService.SourceFile);
            Assert.IsTrue(isNotified && isUpdated);
        }

        [TestMethod]
        public void Should_search_insurance_informations_by_customer_name()
        {
            var dataService = new Xenios.Mocks.MockDataService();

            var viewModel = new ViewModel.InsurancePolicyViewModel(dataService);
            viewModel.PathToFile = "mockfilepath";

            var expectedInfos = dataService.FindInsurancePoliciesByCustomerName("ignored");
            var expectedInfosCount = 1;
            var expectedInfo = expectedInfos.First();

            viewModel.SearchText = "ignored";

            var searchResultCount = viewModel.InsurancePolicies.Count;
            var searchResultInfo = viewModel.InsurancePolicies.First();

            Assert.AreEqual(expectedInfosCount, searchResultCount);
            Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(expectedInfo, searchResultInfo);
        }

        [TestMethod]
        public void Should_find_all_policies_when_search_string_is_empty()
        {
            var dataService = new Xenios.Mocks.MockDataService();

            var viewModel = new ViewModel.InsurancePolicyViewModel(dataService);
            var expectedPolicies = dataService.GetAllInsurancePolicies();

            // set to non empty string to simulate a pre-existing search
            viewModel.SearchText = "make a non-empty search first";
            // set to empty string to simulate clearing a search
            viewModel.SearchText = String.Empty;

            var searchResults = viewModel.InsurancePolicies;

            Xenios.Test.Helpers.InsurancePolicyHelper.
                AssertAreEqual(expectedPolicies, searchResults.ToList());
        }
    }
}
