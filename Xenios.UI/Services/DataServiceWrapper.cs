using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Business;

namespace Xenios.UI.Services
{
    public class DataServiceWrapper<T> : IDataService where T: AbsractInsurancePolicyDataService
    {
        public event PoliciesChangedEvent PoliciesChanged;

        public T InsurancePolicyDataService { get; private set; }

        public void Save(List<Domain.Models.InsurancePolicy> policies)
        {
            InsurancePolicyDataService.Save(policies);
        }

        public List<Domain.Models.InsurancePolicy> GetAllInsurancePolicies()
        {
            return InsurancePolicyDataService.GetAllInsurancePolicies();
        }

        public List<Domain.Models.InsurancePolicy> FindInsurancePoliciesByCustomerName(string searchValue)
        {
            return InsurancePolicyDataService.FindInsurancePoliciesByCustomerName(searchValue);
        }

        private String _sourceFile;
        public string SourceFile
        {
            set
            {
                if (_sourceFile == value)
                    return;

                _sourceFile = value;
                DisposeInsurancePolicyDataService();
                if(String.IsNullOrEmpty(value))
                    return;
                
                CreateInsurancePolicyDataService(value);
            }
        }

        private void CreateInsurancePolicyDataService(string value)
        {
            InsurancePolicyDataService = (T)Activator.CreateInstance(typeof(T), value);
            InsurancePolicyDataService.NotifyInsurancePoliciesUpdated += RaisePoliciesChanged;
        }

        private void DisposeInsurancePolicyDataService()
        {
            if(InsurancePolicyDataService != null)
                InsurancePolicyDataService.NotifyInsurancePoliciesUpdated -= RaisePoliciesChanged;

            if (InsurancePolicyDataService is IDisposable)
            {
                ((IDisposable)InsurancePolicyDataService).Dispose();
            }

            InsurancePolicyDataService = null;
        }

        void RaisePoliciesChanged(List<Domain.Models.InsurancePolicy> policies)
        {
            if (PoliciesChanged != null)
                PoliciesChanged(policies);
        }
    }
}
