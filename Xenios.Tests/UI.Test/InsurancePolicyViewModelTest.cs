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

            viewModel.InsuranceInformations.CollectionChanged += (sender, args) => {
                isNotified = true;
            };

            viewModel.PathToFile = @"\mockpath\mockfile";
            
            var isUpdated = viewModel.InsuranceInformations.Count > 0;

            Assert.AreEqual(viewModel.PathToFile, dataService.SourceFile);
            Assert.IsTrue(isNotified && isUpdated);
        }

        [TestMethod]
        public void Should_search_insurance_informations_by_customer_name()
        {
            var dataService = new Xenios.Mocks.MockDataService();

            var viewModel = new ViewModel.InsurancePolicyViewModel(dataService);
            viewModel.PathToFile = "mockfilepath";

            var expectedInfos = dataService.FindInsuranceInformationsByCustomerName("ignored");
            var expectedInfosCount = 1;
            var expectedInfo = expectedInfos.First();

            viewModel.SearchText = "ignored";

            var searchResultCount = viewModel.InsuranceInformations.Count;
            var searchResultInfo = viewModel.InsuranceInformations.First();

            Assert.AreEqual(expectedInfosCount, searchResultCount);
            Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(expectedInfo, searchResultInfo);
        }

        [TestMethod]
        public void Should_find_all_policies_when_search_string_is_empty()
        {
            var dataService = new Xenios.Mocks.MockDataService();

            var viewModel = new ViewModel.InsurancePolicyViewModel(dataService);
            var expectedPolicies = dataService.GetAllInsuranceInformations();
            var expectedCount = expectedPolicies.Count;

            // set to non empty string to simulate a pre-existing search
            viewModel.SearchText = "make a non-empty search first";
            // set to empty string to simulate clearing a search
            viewModel.SearchText = String.Empty;

            var searchResultCount = viewModel.InsuranceInformations.Count;
            var searchResults = viewModel.InsuranceInformations;

            Assert.AreEqual(expectedCount, searchResultCount);
            for (int i = 0; i < expectedPolicies.Count; i++)
            {
                var expectedPolicy = expectedPolicies[0];
                var searchPolicy = searchResults[0];
                Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(expectedPolicy, searchPolicy);
            }
        }
    }
}
