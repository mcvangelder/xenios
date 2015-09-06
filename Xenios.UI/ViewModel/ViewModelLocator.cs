/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Xenios.UI"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Xenios.UI.Services;

namespace Xenios.UI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private XeniosServiceLocator _xeniosServiceLocator = new XeniosServiceLocator();
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<InsurancePolicyViewModel>(() => 
                new InsurancePolicyViewModel(
                            _xeniosServiceLocator.InsurancePolicyDataService,
                            _xeniosServiceLocator.CountriesService)
                );
        }

        public InsurancePolicyViewModel InsurancePolicy
        {
            get{
                return ServiceLocator.Current.GetInstance<InsurancePolicyViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}