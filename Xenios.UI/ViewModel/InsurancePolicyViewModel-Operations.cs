using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Xenios.Domain.Models;
using Xenios.UI.Services;

namespace Xenios.UI.ViewModel
{
    public partial class InsurancePolicyViewModel
    {
        private void InitializeViewModel()
        {
            RefreshPolicyListCommand = new RelayCommand(LoadInsurancePolicies, () => IsPathToFileSpecified);
            SavePoliciesCommand = new RelayCommand(SavePolicies, () => IsPathToFileSpecified);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
            CloseFileCommand = new RelayCommand(CloseFile, () => IsPathToFileSpecified);
            OpenFileDialogCommand = new RelayCommand(OpenFileDialog);

            PropertyChanged += InsurancePolicyViewModel_PropertyChanged;

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

        void InsurancePolicyViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case PathToFilePropertyName:
                    ProcessPathToFileChange();
                    break;
                case SearchTextPropertyName:
                    LoadInsurancePolicies();
                    break;
                case IsDataUpToDatePropertyName:
                    SetStatusImage();
                    break;
                default:
                    break;
            }
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
            _dataService.SourceFile = _pathToFile;
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

        public void SetDataService(IDataService service)
        {
            if (_dataService == service)
                return;

            _dataService = service;
            _dataService.PoliciesChanged += _dataService_PoliciesChanged;
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
            policies = GetPolicies();

            ApplicationService.RunOnUI(() =>
            {
                InsurancePolicies.Clear();
                if (policies != null)
                {
                    policies.ForEach(item => InsurancePolicies.Add(item));
                }
            });
            LastReadDateTime = DateTime.Now;
            IsDataUpToDate = true;

            ApplicationService.IsBusy(false);
        }

        private List<InsurancePolicy> GetPolicies()
        {
            List<InsurancePolicy> policies = null;

            if (String.IsNullOrEmpty(_searchText))
            {
                policies = _dataService.RefreshPolicies(InsurancePolicies.ToList());
            }
            else
            {
                policies = _dataService.FindInsurancePoliciesByCustomerName(_searchText);
            }

            return policies;
        }

        private void ExitApplication()
        {
            if (ApplicationService == null)
                return;

            ApplicationService.ExitApplication();
        }

        private void SavePolicies()
        {
            if (_isDataUpToDate.GetValueOrDefault(false))
            {
                _dataService.Save(_insurancePolicies.ToList());
            }
            else
            {
                ApplicationService.Alert("You must refresh before you can save.");
            }
        }

        private void CloseFile()
        {
            PathToFile = String.Empty;
        }

        private void OpenFileDialog()
        {
            var chosenFile = ApplicationService.ChooseFile();
            if (String.IsNullOrEmpty(chosenFile))
                return;

            PathToFile = chosenFile;
        }
    }
}
