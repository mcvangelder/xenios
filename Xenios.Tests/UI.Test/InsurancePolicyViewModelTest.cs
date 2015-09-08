﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Windows.Media.Imaging;
using Xenios.UI.Services;
using System.Threading;
using Xenios.UI.Utilities;
using System.Collections.Generic;
using System.Windows;

namespace Xenios.UI.Test
{
    [TestClass]
    public class InsurancePolicyViewModelTest
    {
        private Mocks.MockDataService _dataService;
        private ViewModel.InsurancePolicyViewModel _viewModel;
        private Mocks.MockApplicationService _applicationService;
        private Mocks.MockCountriesService _mockCountriesService;

        private const string mockFilePath = @"\mockpath\mockfile";

        [TestInitialize]
        public void CreateViewModel()
        {
            _dataService = new Xenios.Mocks.MockDataService();
            _mockCountriesService = new Mocks.MockCountriesService();

            _viewModel = new ViewModel.InsurancePolicyViewModel(_dataService, _mockCountriesService);

            _applicationService = new Mocks.MockApplicationService();
            _viewModel.ApplicationService = _applicationService;
        }

        private void SetPathToFile(String path = null)
        {
            path = path ?? mockFilePath;

            var readyEvent = new ManualResetEvent(false);

            _viewModel.PropertyChanged += (sender, arg) =>
            {
                if (arg.PropertyName == "IsEnabled")
                    readyEvent.Set();
            };
            _viewModel.PathToFile = path;

            bool isReady = readyEvent.WaitOne(Constants.WaitTimeOut);
            Assert.IsTrue(isReady);
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

            SetPathToFile();

            var isNotified = notifiyEvent.WaitOne(Constants.WaitTimeOut);

            Assert.AreEqual(_viewModel.PathToFile, _dataService.SourceFile);
            Assert.IsTrue(isNotified && isUpdated);
        }

        [TestMethod]
        public void Should_request_clear_and_load_policies_run_on_application_service_RunOnUI_method()
        {
            // this test over simplifies the actual calls made and only counts the number of times called

            var expectedCount = 2;

            using (var expectedCountReceivedEvent = new ManualResetEvent(false))
            {
                var runOnUICount = 0;
                _applicationService.OnRunOnUI += () =>
                {
                    runOnUICount++;
                    if (runOnUICount == expectedCount)
                        expectedCountReceivedEvent.Set();
                };

                SetPathToFile();

                var isNotified = expectedCountReceivedEvent.WaitOne(Constants.WaitTimeOut);
                Assert.IsTrue(isNotified);
            }
        }

        [TestMethod]
        public void Should_find_all_policies_when_search_string_is_empty()
        {
            var expectedPolicies = _dataService.GetAllInsurancePolicies();

            SetPathToFile();

            // set to non empty string to simulate a pre-existing search
            _viewModel.SearchText = "make a non-empty search first";
            // set to empty string to simulate clearing a search
            _viewModel.SearchText = String.Empty;

            var searchResults = _viewModel.InsurancePolicies.Select(s => s.InsurancePolicy).ToList();

            Xenios.Test.Helpers.InsurancePolicyHelper.
                AssertAreEqual(expectedPolicies, searchResults.ToList());
        }

        [TestMethod]
        public void Should_update_last_read_datetime_after_loading_policies()
        {
            using (var lastReadUpdatedEvent = new ManualResetEvent(false))
            {
                var lastReadDateTime = _viewModel.LastReadDateTime;
                _viewModel.PropertyChanged += (sender, arg) =>
                {
                    if (arg.PropertyName == "LastReadDateTime")
                        lastReadUpdatedEvent.Set();
                };

                SetPathToFile();

                lastReadUpdatedEvent.WaitOne(Constants.WaitTimeOut);
                Assert.AreNotEqual(lastReadDateTime, _viewModel.LastReadDateTime);
            }
        }

        [TestMethod]
        public void Should_indicate_data_is_up_to_date_upon_loading()
        {
            SetPathToFile();

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

            SetPathToFile();

            // register event handler here to prevent false positive from setting the PathToFile
            // which does an initial load of the InsurancePolicies
            _dataService.OnRefreshPolicies += () => { isNotified = true; };

            var previousLastReadDateTime = _viewModel.LastReadDateTime;
            // prevent false negatives by allowing enough time to pass so there is a difference between the timestamps
            System.Threading.Thread.Sleep(5);

            _viewModel.RefreshPolicyListCommand.Execute(null);
            var currentLastReadDateTime = _viewModel.LastReadDateTime;

            Assert.IsTrue(isNotified);
            Assert.AreNotEqual(previousLastReadDateTime, currentLastReadDateTime);
        }

        [TestMethod]
        public void Should_refresh_data_and_apply_current_search_text()
        {
            SetPathToFile();
            var searchText = "FilterValue";

            _viewModel.InsurancePolicies[0].CustomerFirstName = searchText;

            using (var searchTextEvent = new AutoResetEvent(false))
            {
                var isBusyCount = 0;
                _applicationService.OnIsBusy += () =>
                {
                    isBusyCount++;
                    if (isBusyCount == 2)
                    {
                        isBusyCount = 0; searchTextEvent.Set();
                    }
                };

                _viewModel.SearchText = searchText;
                searchTextEvent.WaitOne(Constants.WaitTimeOut);


                var previousSearchResults = _viewModel.InsurancePolicies.Where(w => w.IsIncludedInFilter).Select(s => s.InsurancePolicy).ToList();

                _viewModel.RefreshPolicyListCommand.Execute(null);
                var currentSearchResults = _viewModel.InsurancePolicies.Where(w => w.IsIncludedInFilter).Select(s => s.InsurancePolicy).ToList();

                Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(previousSearchResults, currentSearchResults);
            }
        }

        [TestMethod]
        public void Should_save_data_when_save_command_executed_and_IsDataUpToDate_remains_true()
        {
            SetPathToFile();

            bool isSaved = false;

            _dataService.OnSave += (policies) => { isSaved = true; };
            _viewModel.SavePoliciesCommand.Execute(null);

            Assert.IsTrue(isSaved);
            Assert.IsTrue(_viewModel.IsDataUpToDate.GetValueOrDefault(false));
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
            SetPathToFile();

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
            SetPathToFile();
            SetPathToFile(String.Empty);

            Assert.AreEqual(0, _viewModel.InsurancePolicies.Count);
        }

        [TestMethod]
        public void Should_enable_when_PathToFile_is_set_non_empty_and_policies_have_been_loaded()
        {
            SetPathToFile();

            Assert.IsTrue(_viewModel.IsEnabled);
        }

        [TestMethod]
        public void Should_not_enable_when_PathToFile_is_set_empty()
        {
            SetPathToFile();
            SetPathToFile(String.Empty);
            Assert.IsFalse(_viewModel.IsEnabled);
        }

        [TestMethod]
        public void Should_make_status_bar_item_visible_when_PathToFile_is_set_non_empty()
        {
            SetPathToFile();

            Assert.AreEqual(_viewModel.StatusBarItemVisibility, System.Windows.Visibility.Visible);
        }

        [TestMethod]
        public void Should_make_status_bar_item_collapsed_when_PathToFile_is_set_emtpy()
        {
            SetPathToFile();
            SetPathToFile(String.Empty);

            Assert.AreEqual(_viewModel.StatusBarItemVisibility, System.Windows.Visibility.Collapsed);
        }

        [TestMethod]
        public void Should_alert_user_to_refresh_when_out_of_date_and_trying_to_save()
        {
            var isAlerted = false;

            _applicationService.OnAlert += () => { isAlerted = true; };
            _dataService.OnSave += (policies) => { Assert.Fail("Save was called!."); };

            SetPathToFile();

            _viewModel.IsDataUpToDate = false;
            _viewModel.SavePoliciesCommand.Execute(null);

            Assert.IsTrue(isAlerted);
        }

        [TestMethod]
        public void Should_create_list_of_TermTypes()
        {
            var termTypes = _viewModel.TermTypesList;
            var knownTypes = EnumHelper.GetAllAsCollection<List<Domain.Enums.TermUnits>, Domain.Enums.TermUnits>();

            Assert.IsTrue(termTypes.Count == knownTypes.Count);
        }

        [TestMethod]
        public void Should_create_list_of_InsuranceTypes_excluding_Unspecified()
        {
            var insuranceTypes = _viewModel.InsuranceTypesList;
            var knownTypes = EnumHelper.GetAllAsCollection<List<Domain.Enums.InsuranceTypes>, Domain.Enums.InsuranceTypes>();
            knownTypes.Remove(Domain.Enums.InsuranceTypes.Unspecified);

            Assert.IsTrue(insuranceTypes.Count == knownTypes.Count);
        }

        [TestMethod]
        public void Should_create_list_of_CreditCardTypes_excluding_unspecified()
        {
            var creditCardTypes = _viewModel.CreditCardTypesList;
            var knownTypes = EnumHelper.GetAllAsCollection<List<Domain.Enums.CreditCardTypes>, Domain.Enums.CreditCardTypes>();
            knownTypes.Remove(Domain.Enums.CreditCardTypes.Unspecified);

            Assert.IsTrue(creditCardTypes.Count == knownTypes.Count);
        }

        [TestMethod]
        public void Should_notify_application_service_is_busy_when_PathToFile_set()
        {
            using (var expectedNotificationsEvent = new ManualResetEvent(false))
            {
                var isNotifiedCount = 0;
                _applicationService.OnIsBusy += () =>
                {
                    isNotifiedCount++;
                    if (isNotifiedCount == 2) expectedNotificationsEvent.Set();
                };

                SetPathToFile();

                var isNotified = expectedNotificationsEvent.WaitOne(Constants.WaitTimeOut);
                Assert.IsTrue(isNotified);
            }
        }

        [TestMethod]
        public void Should_call_get_all_countries_on_countries_service()
        {
            var isNotified = false;
            _mockCountriesService.OnGetAllCountries += () => { isNotified = true; };

            var countries = _viewModel.Countries;
            Assert.IsTrue(isNotified);
        }

        [TestMethod]
        public void Should_filter_by_customer_first_name_case_insensitive()
        {
            var searchText = "FilterValue";

            using (var finishedEvent = new ManualResetEvent(false))
            {
                var isBusyCount=0;
               
                SetPathToFile();
                _applicationService.OnIsBusy += () => { isBusyCount++; if (isBusyCount == 2) finishedEvent.Set(); };

                _viewModel.InsurancePolicies[0].CustomerFirstName = searchText;
                _viewModel.SearchText = searchText.ToLower();
                
                finishedEvent.WaitOne();

                var includedInFilterCount = _viewModel.InsurancePolicies.Count(c => c.IsIncludedInFilter);
                Assert.AreEqual(1, includedInFilterCount);
            }
        }

        [TestMethod]
        public void Should_filter_by_customer_last_name_case_insensitive()
        {
            var searchText = "FilterValue";
            using (var finishedEvent = new ManualResetEvent(false))
            {
                var isBusyCount = 0;

                SetPathToFile();
                _viewModel.InsurancePolicies[0].CustomerLastName = searchText;
                _applicationService.OnIsBusy += () => { isBusyCount++; if (isBusyCount == 2) finishedEvent.Set(); };
                _viewModel.SearchText = searchText.ToLower();

                finishedEvent.WaitOne(Constants.WaitTimeOut);

                var includedInFilterCount = _viewModel.InsurancePolicies.Count(c => c.IsIncludedInFilter);
                Assert.AreEqual(1, includedInFilterCount);
            }
        }

        [TestMethod]
        public void Should_filter_by_customer_first_or_last_name_case_insensitive()
        {
            var searchText = "FilterValue";
            using (var finishedEvent = new ManualResetEvent(false))
            {
                var isBusyCount = 0;

                SetPathToFile();
                _viewModel.InsurancePolicies[0].CustomerFirstName = searchText;
                _viewModel.InsurancePolicies[1].CustomerLastName = searchText;
                _applicationService.OnIsBusy += () => { isBusyCount++; if (isBusyCount == 2) finishedEvent.Set(); };
                _viewModel.SearchText = searchText.ToLower();

                finishedEvent.WaitOne(Constants.WaitTimeOut);

                var includedInFilterCount = _viewModel.InsurancePolicies.Count(c => c.IsIncludedInFilter);
                Assert.AreEqual(2, includedInFilterCount);
            }
        }
    }
}
