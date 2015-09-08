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
using Xenios.UI.Utilities;
using Xenios.Business;


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
    public partial class InsurancePolicyViewModel : ViewModelBase
    {
        public ICountriesService CountriesService { get; private set; }
        public IApplicationService ApplicationService { get; set; }
        public IPolicyDataService PolicyDataService { get; private set; }

        public static BitmapImage UpToDateStatusImage;
        public static BitmapImage OutOfDateStatusImage;

        public const String upToDateStatusImagePath = "pack://application:,,,/Xenios.UI;component/Resources/status_Green_Icon_32.png";
        public const String outOfDateStatusImagePath = "pack://application:,,,/Xenios.UI;component/Resources/status_warning_Icon_32.png";

        private bool IsPathToFileSpecified { get { return !String.IsNullOrEmpty(PathToFile); } }
        public InsurancePolicyViewModel(IPolicyDataService policyDataService, ICountriesService countriesService)
        {
            InitializeViewModel(policyDataService,countriesService);
        }


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


        /// <summary>
        /// The <see cref="InsurancePolicies" /> property's name.
        /// </summary>
        public const string InsurancePoliciesPropertyName = "InsurancePolicies";

        private ObservableCollection<PolicyDataGridViewModel> _insurancePolicies = new ObservableCollection<PolicyDataGridViewModel>();

        /// <summary>
        /// Sets and gets the InsurancePolicies property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<PolicyDataGridViewModel> InsurancePolicies
        {
            get
            {
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
                    _termTypes = EnumHelper.
                        GetAllAsCollection<ObservableCollection<Domain.Enums.TermUnits>,Domain.Enums.TermUnits>();

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
        /// Gets the InsuranceTypesList property.
        /// </summary>
        public ICollection<Domain.Enums.InsuranceTypes> InsuranceTypesList
        {
            get; private set;
        }

        /// <summary>
        /// Gets the CreditCardTypesList property.
        /// </summary>
        public ICollection<Domain.Enums.CreditCardTypes> CreditCardTypesList
        {
            get;
            private set;
        }

        /// <summary>
        /// The <see cref="Countries" /> property's name.
        /// </summary>
        public const string CountriesPropertyName = "Countries";

        private List<CountryViewModel> _countries = null;

        /// <summary>
        /// Sets and gets the Countries property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<CountryViewModel> Countries
        {
            get
            {
                if (_countries == null)
                {
                    _countries = new List<CountryViewModel>();
                    GetAllCountries().ForEach(i => _countries.Add(new CountryViewModel(i)));
                }
                return _countries;
            }

            set
            {
                if (_countries == value)
                {
                    return;
                }

                _countries = value;
                RaisePropertyChanged(CountriesPropertyName);
            }
        }
  
    }
}