using System.Windows;
using System.Windows.Controls;
using Xenios.UI.Services;
using Xenios.UI.ViewModel;

namespace Xenios.UI.Views
{
    /// <summary>
    /// Description for InsuranceInformationView.
    /// </summary>
    public partial class InsuranceInformationView : UserControl, IApplicationService
    {
        private InsurancePolicyViewModel _viewModel;
        /// <summary>
        /// Initializes a new instance of the InsuranceInformationView class.
        /// </summary>
        public InsuranceInformationView()
        {
            InitializeComponent();
            _viewModel = ((InsurancePolicyViewModel)DataContext);
            _viewModel.ApplicationService = this;
        }

        public void ExitApplication()
        {
            Window.GetWindow(this).Close();
        }

        public string ChooseFile()
        {
            var fileDiaglog = new Microsoft.Win32.OpenFileDialog();
            var result = fileDiaglog.ShowDialog();
            fileDiaglog.Multiselect = false;

            var chosenFile = string.Empty;

            if(result == true)
            {
                chosenFile = fileDiaglog.FileName;
            }

            return chosenFile;
        }


        public void Alert(string alertText)
        {
            MessageBox.Show(alertText, "Alert!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}