using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xenios.UI.Services;

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

        public InsurancePolicyViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.PoliciesChanged += _dataService_PoliciesChanged;
            RefreshPolicyListCommand = new RelayCommand(() => FindInsurancePoliciesByCustomerName(SearchText));
        }

        void _dataService_PoliciesChanged(List<Domain.Models.InsurancePolicy> newPolicyList)
        {
            IsDataUpToDate = false;
        }

        public RelayCommand RefreshPolicyListCommand { get; set; }

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

                _dataService.SourceFile = _pathToFile = value;

                LoadInsurancePolicies();
                RaisePropertyChanged(PathToFilePropertyName);
            }
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
                FindInsurancePoliciesByCustomerName(value);
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

        private void LoadInsurancePolicies()
        {
            InsurancePolicies.Clear();

            var policies = _dataService.GetAllInsurancePolicies();
            if (policies != null)
            {
                policies.ForEach(item => InsurancePolicies.Add(item));
            }

            LastReadDateTime = DateTime.Now;
            IsDataUpToDate = true;
        }

        private void FindInsurancePoliciesByCustomerName(string value)
        {
            InsurancePolicies.Clear();
            List<Domain.Models.InsurancePolicy> policies = null;

            if (String.IsNullOrEmpty(value))
                policies = _dataService.GetAllInsurancePolicies();

            policies = policies ?? _dataService.FindInsurancePoliciesByCustomerName(value);
            if (policies == null)
                return;

            policies.ForEach(info => InsurancePolicies.Add(info));
        }
    }
}