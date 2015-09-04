﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Windows.Media.Imaging;
using Xenios.UI.Services;
using System.Threading;

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
            _viewModel.ApplicationService = _applicationService;

        }

        [TestMethod]
        public void Should_open_file_and_load_insurance_informations_and_run_on_ui()
        {
            var notifiyEvent = new ManualResetEvent(false);
            var isUpdated = false;

            _viewModel.InsurancePolicies.CollectionChanged += (sender, args) =>
            {
                notifiyEvent.Set();
                isUpdated = _viewModel.InsurancePolicies.Count > 0;
            };

            _viewModel.PathToFile = mockFilePath;

            var isNotified = notifiyEvent.WaitOne(Constants.WaitTimeOut);

            Assert.AreEqual(_viewModel.PathToFile, _dataService.SourceFile);
            Assert.IsTrue(isNotified && isUpdated);
        }

        [TestMethod]
        public void Should_request_clear_and_load_policies_run_on_application_service_RunOnUI_method()
        {
            // this test over simplifies the actual calls made and only counts the number of times called

            // Expected calls to UI:
            //      LoadPolicies
            //      Once for the group:
            //          SavePoliciesCommand.RaiseCanExecuteChanged();
            //          CloseFileCommand.RaiseCanExecuteChanged();
            //          RefreshPolicyListCommand.RaiseCanExecuteChanged();
            var expectedCount = 2;

            var readyEvent = new ManualResetEvent(false);
            var runOnUICount = 0;
            _applicationService.OnRunOnUI += () =>
            {
                runOnUICount++;
                if (runOnUICount == expectedCount)
                    readyEvent.Set();
            };
            _viewModel.PathToFile = mockFilePath;

            var isNotified = readyEvent.WaitOne(Constants.WaitTimeOut);
            Assert.IsTrue(isNotified);
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
            
            _viewModel.PathToFile = mockFilePath;

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
            var readyEvent = new ManualResetEvent(false);

            var lastReadDateTime = _viewModel.LastReadDateTime;
            _viewModel.PathToFile = mockFilePath;
            _viewModel.PropertyChanged += (sender, arg) =>
            {
                if (arg.PropertyName == "LastReadDateTime")
                    readyEvent.Set();
            };
            readyEvent.WaitOne(Constants.WaitTimeOut);
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
            var isNotified = false;

            _viewModel.PathToFile = mockFilePath;

            // register event handler here to prevent false positive from setting the PathToFile
            // which does an initial load of the InsurancePolicies
            _dataService.OnRefreshPolicies += () => { isNotified = true; };

            var previousLastReadDateTime = _viewModel.LastReadDateTime;
            System.Threading.Thread.Sleep(50); // prevent false negatives

            _viewModel.RefreshPolicyListCommand.Execute(null);
            var currentLastReadDateTime = _viewModel.LastReadDateTime;

            Assert.IsTrue(isNotified);
            Assert.AreNotEqual(previousLastReadDateTime, currentLastReadDateTime);
        }

        [TestMethod]
        public void Should_refersh_data_and_apply_current_search_text()
        {
            var readyEvent = new ManualResetEvent(false);

            _viewModel.PropertyChanged += (sender, arg) =>
            {
                if (arg.PropertyName == "LastReadDateTime")
                    readyEvent.Set();
            };

            _viewModel.PathToFile = mockFilePath;
            var previousLastReadDateTime = _viewModel.LastReadDateTime;

            var isReady = readyEvent.WaitOne(Constants.WaitTimeOut);
            Assert.IsTrue(isReady);

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
        public void Should_close_application_when_exit_command_executed()
        {
            var isNotifed = false;

            _applicationService.OnExit += () => { isNotifed = true; };
            _viewModel.ExitApplicationCommand.Execute(null);

            Assert.IsTrue(isNotifed);
        }

        [TestMethod]
        public void Should_set_path_to_file_empty_when_close_file_command_executed()
        {
            _viewModel.CloseFileCommand.Execute(null);

            Assert.IsTrue(String.IsNullOrEmpty(_viewModel.PathToFile));
        }

        [TestMethod]
        public void Should_call_open_file_dialog_on_application_service()
        {
            var isNotified = false;
            _applicationService.OnChooseFile += () => { isNotified = true; };
            _viewModel.OpenFileDialogCommand.Execute(null);

            Assert.IsTrue(isNotified);
        }

        [TestMethod]
        public void Should_not_attempt_to_load_insurance_policies_when_path_to_file_is_empty()
        {
            var isNotified = false;
            _dataService.OnGetAllInsurancePolicies += () => { isNotified = true; };
            _viewModel.PathToFile = String.Empty;

            Assert.IsFalse(isNotified);
        }

        [TestMethod]
        public void Should_clear_InsurancePolicies_when_PathToFile_is_set_to_empty()
        {
            _viewModel.PathToFile = mockFilePath;
            _viewModel.PathToFile = String.Empty;

            Assert.AreEqual(0, _viewModel.InsurancePolicies.Count);
        }

        [TestMethod]
        public void Should_enable_when_PathToFile_is_set_non_empty_and_policies_have_been_loaded()
        {
            var readyEvent = new ManualResetEvent(false);
            _viewModel.PropertyChanged += (sender, arg) => { if (arg.PropertyName == "IsEnabled") readyEvent.Set(); };
            _viewModel.PathToFile = mockFilePath;

            readyEvent.WaitOne(Constants.WaitTimeOut);
            Assert.IsTrue(_viewModel.IsEnabled);
        }

        [TestMethod]
        public void Should_not_enable_when_PathToFile_is_set_empty()
        {
            _viewModel.PathToFile = mockFilePath;
            _viewModel.PathToFile = String.Empty;
            Assert.IsFalse(_viewModel.IsEnabled);
        }

        [TestMethod]
        public void Should_make_status_bar_item_visible_when_PathToFile_is_set_non_empty()
        {
            _viewModel.PathToFile = mockFilePath;
            Assert.AreEqual(_viewModel.StatusBarItemVisibility, System.Windows.Visibility.Visible);
        }

        [TestMethod]
        public void Should_make_status_bar_item_collapsed_when_PathToFile_is_set_emtpy()
        {
            _viewModel.PathToFile = mockFilePath;
            _viewModel.PathToFile = String.Empty;
            Assert.AreEqual(_viewModel.StatusBarItemVisibility, System.Windows.Visibility.Collapsed);
        }

        [TestMethod]
        public void Should_alert_user_to_refresh_when_out_of_date_and_trying_to_save()
        {
            var isAlerted = false;
            var readyEvent = new ManualResetEvent(false);

            _viewModel.SavePoliciesCommand.CanExecuteChanged += (sender, arg) => { readyEvent.Set(); };
            _applicationService.OnAlert += () => { isAlerted = true; };
            _dataService.OnSave += (policies) => { Assert.Fail("Save was called!."); };

            _viewModel.PathToFile = mockFilePath;

            var updateEventSet = readyEvent.WaitOne(Constants.WaitTimeOut);
            Assert.IsTrue(updateEventSet);

            _viewModel.IsDataUpToDate = false;
            _viewModel.SavePoliciesCommand.Execute(null);

            Assert.IsTrue(isAlerted);
        }

        [TestMethod]
        public void Should_create_list_of_TermTypes()
        {
            var termTypes = _viewModel.TermTypesList;
            Assert.IsTrue(termTypes.Count > 0);
        }

        [TestMethod]
        public void Should_create_list_of_InsuranceTypes()
        {
            var insuranceTypes = _viewModel.InsuranceTypesList;
            Assert.IsTrue(insuranceTypes.Count > 0);
        }

        [TestMethod]
        public void Should_notify_application_service_is_busy_when_PathToFile_set()
        {
            var readyEvent = new ManualResetEvent(false);

            var isNotifiedCount = 0;
            _applicationService.OnIsBusy += () =>
            {
                isNotifiedCount++;
                if (isNotifiedCount == 2) readyEvent.Set();
            };
            _viewModel.PathToFile = mockFilePath;

            var isNotified = readyEvent.WaitOne(Constants.WaitTimeOut);
        }

        [TestMethod]
        public void Should_call_get_all_countries_on_countries_service()
        {
            var isNotified = false;
            var mockCountriesService = new Mocks.MockCountriesService();
            mockCountriesService.OnGetAllCountries += () => { isNotified = true; };
            _viewModel.CountriesService = mockCountriesService;

            _viewModel.GetAllCountries();
            Assert.IsTrue(isNotified);
        }
    }
}
