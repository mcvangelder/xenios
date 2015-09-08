using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Xenios.Business;
using Xenios.Domain.Models;
using Xenios.UI.Services;
using Xenios.UI.Utilities;

namespace Xenios.UI.ViewModel
{
    public partial class InsurancePolicyViewModel
    {
        private void InitializeViewModel(IPolicyDataService policyDataService, ICountriesService countriesService)
        {
            PolicyDataService = policyDataService;
            CountriesService = countriesService;

            SetInsuranceTypesList();
            SetCreditCardTypesList();

            RefreshPolicyListCommand = new RelayCommand(LoadInsurancePolicies, () => IsPathToFileSpecified);
            SavePoliciesCommand = new RelayCommand(SavePolicies, () => IsPathToFileSpecified);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
            CloseFileCommand = new RelayCommand(CloseFile, () => IsPathToFileSpecified);
            OpenFileDialogCommand = new RelayCommand(OpenFileDialog);

            PropertyChanged += InsurancePolicyViewModel_PropertyChanged;
            PolicyDataService.PoliciesChanged += _dataService_PoliciesChanged;

            try
            {
                // This block throws exception with in unit test. Unable to find required Reference to
                // eliminate the exception. The issue is scheme: "pack://" is not recognized outside of the
                // WPF project.
                UpToDateStatusImage = LoadBitmapImage(upToDateStatusImagePath);
                OutOfDateStatusImage = LoadBitmapImage(outOfDateStatusImagePath);
            }
            catch (UriFormatException) { }
        }

        private void SetInsuranceTypesList()
        {
            var allTypes = EnumHelper.GetAllAsCollection<List<Domain.Enums.InsuranceTypes>, Domain.Enums.InsuranceTypes>();
            allTypes.Remove(Domain.Enums.InsuranceTypes.Unspecified);
            InsuranceTypesList = allTypes;
        }

        private void SetCreditCardTypesList()
        {
            var allTypes = EnumHelper.GetAllAsCollection<List<Domain.Enums.CreditCardTypes>, Domain.Enums.CreditCardTypes>();
            allTypes.Remove(Domain.Enums.CreditCardTypes.Unspecified);
            CreditCardTypesList = allTypes;
        }

        void InsurancePolicyViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case PathToFilePropertyName:
                    ProcessPathToFileChange();
                    break;
                case SearchTextPropertyName:
                    FilterPolicies();
                    break;
                case IsDataUpToDatePropertyName:
                    ProcessIsDataUpToDateChange();
                    break;
                default:
                    break;
            }
        }

        private void FilterPolicies()
        {
            Task t = Task.Factory.StartNew(() =>
                {
                    ApplicationService.IsBusy(true);
                    foreach (var policy in _insurancePolicies)
                    {
                        if (String.IsNullOrEmpty(_searchText))
                        {
                            policy.IsIncludedInFilter = true;
                            continue;
                        }

                        var firstName = policy.CustomerFirstName ?? String.Empty;
                        var lastName = policy.CustomerLastName ?? String.Empty;

                        policy.IsIncludedInFilter =
                            firstName.ToLower().Contains(_searchText.ToLower()) ||
                            lastName.ToLower().Contains(_searchText.ToLower());
                    }
                    ApplicationService.IsBusy(false);
                });
        }

        private void ProcessIsDataUpToDateChange()
        {
            SetStatusImage();
        }

        private void SetStatusImage()
        {
            StatusImage = _isDataUpToDate.GetValueOrDefault(false) ? UpToDateStatusImage : OutOfDateStatusImage;
        }

        private static BitmapImage LoadBitmapImage(string imagePath)
        {
            var bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            // Freezing bitmap allows this object to be shared accross threads regardless of which thread created the object
            bitmapImage.Freeze();

            return bitmapImage;
        }

        private void ProcessPathToFileChange()
        {
            PolicyDataService.SourceFile = _pathToFile;
            Task.Factory.StartNew(() =>
            {
                LoadInsurancePolicies();
                NotifyPathToFileDependentCommands();
                SetIsEnabled();
                SetStatusBarVisibility();
            });
        }

        private void SetStatusBarVisibility()
        {
            ApplicationService.RunOnUI(() =>
            {
                StatusBarItemVisibility = IsEnabled ? Visibility.Visible : Visibility.Collapsed;
            });
        }

        private void SetIsEnabled()
        {
            ApplicationService.RunOnUI(() =>
            {
                IsEnabled = !String.IsNullOrWhiteSpace(_pathToFile);
            });
        }

        void _dataService_PoliciesChanged()
        {
            ApplicationService.RunOnUI(() => IsDataUpToDate = false);
        }

        private void NotifyPathToFileDependentCommands()
        {
            ApplicationService.RunOnUI(() =>
            {
                SavePoliciesCommand.RaiseCanExecuteChanged();
                CloseFileCommand.RaiseCanExecuteChanged();
                RefreshPolicyListCommand.RaiseCanExecuteChanged();
            });
        }

        private void LoadInsurancePolicies()
        {
            List<InsurancePolicy> policies = null;
            ApplicationService.IsBusy(true);
            if (!String.IsNullOrEmpty(_pathToFile))
                policies = GetPolicies();

            ApplicationService.RunOnUI(() =>
            {
                InsurancePolicies.Clear();
                if (policies != null)
                {
                    policies.ForEach(item => InsurancePolicies.Add(new PolicyDataGridViewModel(item)));
                    FilterPolicies();
                }
            });
            LastReadDateTime = DateTime.Now;
            IsDataUpToDate = true;

            ApplicationService.IsBusy(false);
        }

        private List<InsurancePolicy> GetPolicies()
        {
            // Always do a refresh, as this will merge new entries not saved yet with the entries that
            // are loaded
            return PolicyDataService.RefreshPolicies(InsurancePolicies.Select(s => s.InsurancePolicy).ToList());
        }

        private void ExitApplication()
        {
            if (ApplicationService == null)
                return;

            ApplicationService.ExitApplication();
        }

        private void SavePolicies()
        {
            TrySavePolicies();
        }

        private bool TrySavePolicies()
        {
            if (_isDataUpToDate.GetValueOrDefault(false))
            {
                PolicyDataService.Save(_insurancePolicies.Select(s => s.InsurancePolicy).ToList());
                return true;
            }
            else
            {
                ApplicationService.Alert("You must refresh before you can save.");
                return false;
            }
        }

        private void CloseFile()
        {
            if (TrySavePolicies())
            {
                PathToFile = String.Empty;
            }
        }

        private void OpenFileDialog()
        {
            var chosenFile = ApplicationService.ChooseFile();
            if (String.IsNullOrEmpty(chosenFile))
                return;

            PathToFile = chosenFile;
        }

        private List<Country> GetAllCountries()
        {
            return CountriesService.GetAllCountries();
        }
    }
}
