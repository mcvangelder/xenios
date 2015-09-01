using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Models;
using Xenios.UI.Services;

namespace Xenios.Mocks
{
    public delegate void OnSaveEvent(List<InsurancePolicy> policies);

    internal class MockDataService : IDataService
    {
        private List<Domain.Models.InsurancePolicy> _insuranceInfos;
        public String SourceFile { get; set; }

        public event PoliciesChangedEvent PoliciesChanged;
        public event OnSaveEvent OnSave;
        public event OnCalledEvent OnGetAllInsurancePolicies;
        public event OnCalledEvent OnRefreshPolicies;
  
        public MockDataService()
        {
            _insuranceInfos = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(5);
        }

        public List<Domain.Models.InsurancePolicy> GetAllInsurancePolicies()
        {
            if (OnGetAllInsurancePolicies != null)
                OnGetAllInsurancePolicies();

            return _insuranceInfos;
        }

        public List<Domain.Models.InsurancePolicy> FindInsurancePoliciesByCustomerName(string searchValue)
        {
            return new List<Domain.Models.InsurancePolicy>() { _insuranceInfos.First() };
        }

        internal void RaisePoliciesChanged()
        {
            if (PoliciesChanged != null)
                PoliciesChanged();
        }

        public void Save(List<InsurancePolicy> policies)
        {
            if (OnSave != null)
                OnSave(policies);
        }


        public List<InsurancePolicy> RefreshPolicies(List<InsurancePolicy> currentPolicies)
        {
            if (OnRefreshPolicies != null)
                OnRefreshPolicies();

            return currentPolicies.Union(_insuranceInfos).ToList();
        }
    }
}
