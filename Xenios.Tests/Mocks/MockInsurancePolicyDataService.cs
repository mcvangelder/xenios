using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Business;

namespace Xenios.Mocks
{
    public delegate void OnCalledEvent();

    public class MockInsurancePolicyDataService : AbsractInsurancePolicyDataService, IDisposable
    {
        public MockInsurancePolicyDataService(String fileName)
            :base(fileName)
        { }

        public event OnSaveEvent OnSave;
        public event OnCalledEvent OnGetAllInsurancePolicies;
        public event OnCalledEvent OnFindInsurancePoliciesByCustomerName;
        public event OnCalledEvent OnDispose;

        public override void Save(List<Domain.Models.InsurancePolicy> policies)
        {
            if (OnSave != null)
                OnSave(policies);
        }

        public override List<Domain.Models.InsurancePolicy> GetAllInsurancePolicies()
        {
            if (OnGetAllInsurancePolicies != null)
                OnGetAllInsurancePolicies();

            return new List<Domain.Models.InsurancePolicy>();
        }

        public override List<Domain.Models.InsurancePolicy> FindInsurancePoliciesByCustomerName(string searchValue)
        {
            if (OnFindInsurancePoliciesByCustomerName != null)
                OnFindInsurancePoliciesByCustomerName();

            return new List<Domain.Models.InsurancePolicy>();
        }

        public void Dispose()
        {
            if(OnDispose != null)
            {
                OnDispose();
            }
        }

        internal void RaisePoliciesChanged()
        {
            RaiseNotifyInsurancePoliciesUpdated(new List<Domain.Models.InsurancePolicy>());
        }
    }
}
