using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xenios.UI.Services;
using System.Linq;
using Xenios.Domain.Models;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Xenios.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class InsurancePolicyViewModel : ViewModelBase
    {
        private IDataService _dataService;

        public IApplicationService ApplicationService { get; set; }
        public IDataService DataService { get { return _dataService; } }

        private bool IsPathToFileSpecified { get { return !String.IsNullOrEmpty(PathToFile); } }
        public InsurancePolicyViewModel()
        {
            RefreshPolicyListCommand = new RelayCommand(LoadInsurancePolicies, () => IsPathToFileSpecified);
            SavePoliciesCommand = new RelayCommand(SavePolicies, () => IsPathToFileSpecified);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
            CloseFileCommand = new RelayCommand(CloseFile, () => IsPathToFileSpecified);
            OpenFileDialogCommand = new RelayCommand(OpenFileDialog);

            PropertyChanged += InsurancePolicyViewModel_PropertyChanged;
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
            string imagePath = "pack://application:,,,/Xenios.UI;component/Resources/status_Green_Icon_32.png";
            var bitMapImage = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            //bitMapImage.Freeze();
            StatusImage = bitMapImage;
        }

        private void ProcessPathToFileChange()
        {
            _dataService.SourceFile = _pathToFile;
            UpdateInsurancePolicyCollection(_pathToFile);
            NotifyPathToFileDependentCommands();
            IsEnabled = !String.IsNullOrWhiteSpace(_pathToFile);
            StatusBarItemVisibility = IsEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SetDataService(IDataService service)
        {
            if (_dataService == service)
                return;

            _dataService = service;
            _dataService.PoliciesChanged += _dataService_PoliciesChanged;
        }

        void _dataService_PoliciesChanged(List<Domain.Models.InsurancePolicy> newPolicyList)
        {
            IsDataUpToDate = false;
        }

        public RelayCommand RefreshPolicyListCommand { get; set; }

        public RelayCommand SavePoliciesCommand { get; set; }

        public RelayCommand ExitApplicationCommand { get; set; }

        public RelayCommand CloseFileCommand { get; set; }

        public RelayCommand OpenFileDialogCommand { get; set; }

        /// <summary>
        /// The <see cref="PathToFile" /> property's name.
        /// </summary>
        public const String PathToFilePropertyName = "PathToFile";

        private String _pathToFile;

        /// <summary>
        /// Sets and gets the PathToFile property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String PathToFile
        {
            get
            {
                return _pathToFile;
            }

            set
            {
                if (_pathToFile == value)
                {
                    return;
                }
                _pathToFile = value;
                RaisePropertyChanged(PathToFilePropertyName);
            }
        }

        private void NotifyPathToFileDependentCommands()
        {
            SavePoliciesCommand.RaiseCanExecuteChanged();
            CloseFileCommand.RaiseCanExecuteChanged();
            RefreshPolicyListCommand.RaiseCanExecuteChanged();
        }

        private void UpdateInsurancePolicyCollection(string value)
        {
            if (String.IsNullOrEmpty(value))
                ClearInsurancePolicies();
            else
                LoadInsurancePolicies();
        }

        /// <summary>
        /// The <see cref="InsurancePolicies" /> property's name.
        /// </summary>
        public const string InsurancePoliciesPropertyName = "InsurancePolicies";

        private ObservableCollection<Domain.Models.InsurancePolicy> _insurancePolicies = new ObservableCollection<Domain.Models.InsurancePolicy>();

        /// <summary>
        /// Sets and gets the InsurancePolicies property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Domain.Models.InsurancePolicy> InsurancePolicies
        {
            get
            {
                if (IsInDesignModeStatic)
                    PathToFile = @"c:\temp\xenios.txt";

                return _insurancePolicies;
            }

            set
            {
                if (_insurancePolicies == value)
                {
                    return;
                }

                _insurancePolicies = value;
                RaisePropertyChanged(InsurancePoliciesPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SearchText" /> property's name.
        /// </summary>
        public const string SearchTextPropertyName = "SearchText";

        private String _searchText = String.Empty;

        /// <summary>
        /// Sets and gets the SearchText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String SearchText
        {
            get
            {
                return _searchText;
            }

            set
            {
                if (_searchText == value)
                {
                    return;
                }

                _searchText = value;

                RaisePropertyChanged(SearchTextPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="LastReadDateTime" /> property's name.
        /// </summary>
        public const string LastReadDateTimePropertyName = "LastReadDateTime";

        private DateTime? _lastReadDateTime = null;

        /// <summary>
        /// Sets and gets the LastReadDateTime property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime? LastReadDateTime
        {
            get
            {
                return _lastReadDateTime;
            }

            set
            {
                if (_lastReadDateTime == value)
                {
                    return;
                }

                _lastReadDateTime = value;
                RaisePropertyChanged(LastReadDateTimePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsDataUpToDate" /> property's name.
        /// </summary>
        public const string IsDataUpToDatePropertyName = "IsDataUpToDate";

        private bool? _isDataUpToDate = null;

        /// <summary>
        /// Sets and gets the IsDataUpToDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool? IsDataUpToDate
        {
            get
            {
                return _isDataUpToDate;
            }

            set
            {
                if (_isDataUpToDate == value)
                {
                    return;
                }

                _isDataUpToDate = value;
                RaisePropertyChanged(IsDataUpToDatePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="StatusImage" /> property's name.
        /// </summary>
        public const string StatusImagePropertyName = "StatusImage";

        private BitmapImage _statusImage = null;

        /// <summary>
        /// Sets and gets the StatusImage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BitmapImage StatusImage
        {
            get
            {
                return _statusImage;
            }

            set
            {
                if (_statusImage == value)
                {
                    return;
                }

                _statusImage = value;
                RaisePropertyChanged(StatusImagePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsEnabled" /> property's name.
        /// </summary>
        public const string IsEnabledPropertyName = "IsEnabled";

        private bool _isEnabled = false;

        /// <summary>
        /// Sets and gets the IsEnabled property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                if (_isEnabled == value)
                {
                    return;
                }

                _isEnabled = value;
                RaisePropertyChanged(IsEnabledPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="StatusBarItemVisibility" /> property's name.
        /// </summary>
        public const string StatusBarItemVisibilityPropertyName = "StatusBarItemVisibility";

        private Visibility _statusBarItemVisbility = Visibility.Collapsed;

        /// <summary>
        /// Sets and gets the StatusBarVisibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility StatusBarItemVisibility
        {
            get
            {
                if (IsInDesignModeStatic)
                    return Visibility.Visible;

                return _statusBarItemVisbility;
            }

            set
            {
                if (_statusBarItemVisbility == value)
                {
                    return;
                }

                _statusBarItemVisbility = value;
                RaisePropertyChanged(StatusBarItemVisibilityPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="TermTypesList" /> property's name.
        /// </summary>
        public const string TermTypesListPropertyName = "TermTypesList";

        ICollection<Domain.Enums.TermUnits> _termTypes;
        public ICollection<Domain.Enums.TermUnits> TermTypesList
        {
            get
            {
                if (_termTypes == null)
                    _termTypes = CreateListFromEnums<Domain.Enums.TermUnits>();

                return _termTypes;
            }
            set
            {
                if (_termTypes == value)
                    return;

                _termTypes = value;
                RaisePropertyChanged(StatusBarItemVisibilityPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="InsuranceTypesList" /> property's name.
        /// </summary>
        public const string InsuranceTypesListPropertyName = "InsuranceTypesList";

        private ICollection<Domain.Enums.InsuranceTypes> _insuranceTypes = null;

        /// <summary>
        /// Sets and gets the InsuranceTypesList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ICollection<Domain.Enums.InsuranceTypes> InsuranceTypesList
        {
            get
            {
                if (_insuranceTypes == null)
                    _insuranceTypes = CreateListFromEnums<Domain.Enums.InsuranceTypes>();

                return _insuranceTypes;
            }

            set
            {
                if (_insuranceTypes == value)
                {
                    return;
                }

                _insuranceTypes = value;
                RaisePropertyChanged(InsuranceTypesListPropertyName);
            }
        }

        private ICollection<T> CreateListFromEnums<T>()
        {
            var allEnums = Enum.GetValues(typeof(T));
            var list = new ObservableCollection<T>();
            for (int i = 0; i < allEnums.Length; i++)
                list.Add((T)allEnums.GetValue(i));

            return list;
        }

        private void ClearInsurancePolicies()
        {
            InsurancePolicies.Clear();
        }

        private void LoadInsurancePolicies()
        {
            ClearInsurancePolicies();
            List<InsurancePolicy> policies = null;

            if (String.IsNullOrEmpty(_searchText))
            {
                policies = _dataService.GetAllInsurancePolicies();
            }
            else
            {
                policies = _dataService.FindInsurancePoliciesByCustomerName(_searchText);
            }

            if (policies != null)
            {
                policies.ForEach(item => InsurancePolicies.Add(item));
            }

            LastReadDateTime = DateTime.Now;
            IsDataUpToDate = true;
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