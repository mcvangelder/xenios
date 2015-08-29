using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
