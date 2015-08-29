using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Xenios.UI.Test
{
    [TestClass]
    public class InsuranceInformationViewModelTest
    {
        [TestMethod]
        public void Should_open_file_and_load_insurance_informations()
        {
            var dataService =  new Xenios.Mocks.MockDataService();
            
            ViewModel.InsuranceInformationViewModel viewModel =
               new ViewModel.InsuranceInformationViewModel(dataService);
            
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
        public void Should_search_insurance_informations_by_customer_first_name()
        {
            var dataService = new Xenios.Mocks.MockDataService();

            var viewModel = new ViewModel.InsuranceInformationViewModel(dataService);
            viewModel.PathToFile = "mockfilepath";

            var expectedInfos = dataService.FindInsuranceInformationsByCustomerName("ignored");
            var expectedInfosCount = 1;
            var expectedInfo = expectedInfos.First();

            viewModel.SearchText = "ignored";

            var searchResultCount = viewModel.InsuranceInformations.Count;
            var searchResultInfo = viewModel.InsuranceInformations.First();

            Assert.AreEqual(expectedInfosCount, searchResultCount);
            Xenios.Test.Helpers.InsuranceInformationHelper.AssertAreEqual(expectedInfo, searchResultInfo);
        }
    }
}
