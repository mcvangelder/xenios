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
        private Mocks.MockApplicationService _applicationService;

        private const string mockFilePath = @"\mockpath\mockfile";

        [TestInitialize]
        public void CreateViewModel()
        {
            _dataService = new Xenios.Mocks.MockDataService();
            _viewModel = new ViewModel.InsurancePolicyViewModel();
            _viewModel.SetDataService(_dataService);

            _applicationService = new Mocks.MockApplicationService();
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
            _dataService.RaisePoliciesChanged();

            Assert.IsFalse(_viewModel.IsDataUpToDate.GetValueOrDefault(true));
        }

        [TestMethod]
        public void Should_refresh_data_when_refresh_command_executed()
        {
            _viewModel.PathToFile = mockFilePath;

            var previousLastReadDateTime = _viewModel.LastReadDateTime;
            _viewModel.RefreshPolicyListCommand.Execute(null);
            var currentLastReadDateTime = _viewModel.LastReadDateTime;

            Assert.AreNotEqual(previousLastReadDateTime, currentLastReadDateTime);
        }

        [TestMethod]
        public void Should_refersh_data_and_apply_current_search_text()
        {
            _viewModel.PathToFile = mockFilePath;
            var previousLastReadDateTime = _viewModel.LastReadDateTime;
            
            _viewModel.SearchText = "ignored";
            var previousSearchResults = _viewModel.InsurancePolicies.ToList();

            _viewModel.RefreshPolicyListCommand.Execute(null);
            var currentSearchResults = _viewModel.InsurancePolicies.ToList();

            // The mock data service always returns one record, test this assumption holds true
            Assert.AreEqual(1, previousSearchResults.Count);
            Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(previousSearchResults, currentSearchResults);
        }

        [TestMethod]
        public void Should_save_data_when_save_command_executed()
        {
            var isNotified = false;
            _dataService.OnSave += (policies) => { isNotified = true; };

            _viewModel.PathToFile = mockFilePath;
            _viewModel.SavePoliciesCommand.Execute(null);

            Assert.IsTrue(isNotified);
        }

        [TestMethod]
        public void Should_close_application_when_close_command_executed()
        {
            var isNotifed = false;

            _applicationService.OnExit += () => { isNotifed = true; };
            _viewModel.ApplicationService = _applicationService;
            _viewModel.ExitApplicationCommand.Execute(null);
            
            Assert.IsTrue(isNotifed);
        }
    }
}
