﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Xenios.UI.Test
{
    [TestClass]
    public class InsurancePolicyViewModelTest
    {
        private Mocks.MockDataService _dataService;
        private ViewModel.InsurancePolicyViewModel _viewModel;

        private const string mockFilePath = @"\mockpath\mockfile";

        [TestInitialize]
        public void CreateViewModel()
        {
            _dataService = new Xenios.Mocks.MockDataService();
            _viewModel = new ViewModel.InsurancePolicyViewModel(_dataService);
        }

        [TestMethod]
        public void Should_open_file_and_load_insurance_informations()
        {
            var isNotified = false;

            _viewModel.InsurancePolicies.CollectionChanged += (sender, args) => {
                isNotified = true;
            };

            _viewModel.PathToFile = mockFilePath;
            
            var isUpdated = _viewModel.InsurancePolicies.Count > 0;

            Assert.AreEqual(_viewModel.PathToFile, _dataService.SourceFile);
            Assert.IsTrue(isNotified && isUpdated);
        }

        [TestMethod]
        public void Should_search_insurance_informations_by_customer_name()
        {
            _viewModel.PathToFile = mockFilePath;

            var expectedInfos = _dataService.FindInsurancePoliciesByCustomerName("ignored");
            var expectedInfosCount = 1;
            var expectedInfo = expectedInfos.First();

            _viewModel.SearchText = "ignored";

            var searchResultCount = _viewModel.InsurancePolicies.Count;
            var searchResultInfo = _viewModel.InsurancePolicies.First();

            Assert.AreEqual(expectedInfosCount, searchResultCount);
            Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(expectedInfo, searchResultInfo);
        }

        [TestMethod]
        public void Should_find_all_policies_when_search_string_is_empty()
        {
            var expectedPolicies = _dataService.GetAllInsurancePolicies();

            // set to non empty string to simulate a pre-existing search
            _viewModel.SearchText = "make a non-empty search first";
            // set to empty string to simulate clearing a search
            _viewModel.SearchText = String.Empty;

            var searchResults = _viewModel.InsurancePolicies;

            Xenios.Test.Helpers.InsurancePolicyHelper.
                AssertAreEqual(expectedPolicies, searchResults.ToList());
        }

        [TestMethod]
        public void Should_update_last_read_datetime_after_loading_policies()
        {
            var lastReadDateTime = _viewModel.LastReadDateTime;
            _viewModel.PathToFile = mockFilePath;

            Assert.AreNotEqual(lastReadDateTime, _viewModel.LastReadDateTime);
        }

        [TestMethod]
        public void Should_indicate_data_is_up_to_date_upon_loading()
        {
            _viewModel.PathToFile = mockFilePath;

            Assert.IsTrue(_viewModel.IsDataUpToDate.GetValueOrDefault(false));
        }

        [TestMethod]
        public void Should_indicate_data_is_not_up_to_date()
        {
            _dataService.RaiseNewPoliciesAvailable();

            Assert.IsFalse(_viewModel.IsDataUpToDate.GetValueOrDefault(true));
        }
    }
}
