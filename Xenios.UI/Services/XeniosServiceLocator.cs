
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Xenios.Business;
using Xenios.DataAccess;

namespace Xenios.UI.Services
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class XeniosServiceLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public XeniosServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<DataServiceWrapper<Business.InsurancePolicyDataService>>();
            SimpleIoc.Default.Register<CountriesService>(() =>
                    new CountriesService(new CountriesRepository())
                );
        }

        public IDataService InsurancePolicyDataService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DataServiceWrapper<Business.InsurancePolicyDataService>>();
            }
        }
        
        public ICountriesService CountriesService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CountriesService>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}