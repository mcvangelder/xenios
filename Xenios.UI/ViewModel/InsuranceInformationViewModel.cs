using GalaSoft.MvvmLight;
using System;
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
    public class InsuranceInformationViewModel : ViewModelBase
    {
        private IDataService _dataService;

        public InsuranceInformationViewModel(IDataService dataService)
        {
            _dataService = dataService;
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

                _dataService.SourceFile = _pathToFile = value;

                LoadInsuranceInformations();
                RaisePropertyChanged(PathToFilePropertyName);
            }
        }

        private void LoadInsuranceInformations()
        {
            InsuranceInformations.Clear();
            var infos = _dataService.GetAllInsuranceInformations();
            if (infos == null)
                return;

            infos.ForEach(item => InsuranceInformations.Add(item));
        }

        /// <summary>
        /// The <see cref="InsuranceInformations" /> property's name.
        /// </summary>
        public const string InsuranceInformationsPropertyName = "InsuranceInformations";

        private ObservableCollection<Domain.Models.InsuranceInformation> _insuranceInformations = new ObservableCollection<Domain.Models.InsuranceInformation>();

        /// <summary>
        /// Sets and gets the InsuranceInformations property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Domain.Models.InsuranceInformation> InsuranceInformations
        {
            get
            {
                return _insuranceInformations;
            }

            set
            {
                if (_insuranceInformations == value)
                {
                    return;
                }

                _insuranceInformations = value;
                RaisePropertyChanged(InsuranceInformationsPropertyName);
            }
        }

        public string SearchText { get; set; }
    }
}