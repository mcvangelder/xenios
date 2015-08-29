using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Business;

namespace Xenios.Mocks
{
    public delegate void OnLoadEvent();

    public class MockInsurancePolicyDataService : IInsurancePolicyDataService
    {
        public event OnSaveEvent OnSave;
        public event OnLoadEvent OnGetAllInsurancePolicies;

        public void Save(List<Domain.Models.InsurancePolicy> policies)
        {
            if (OnSave != null)
                OnSave(policies);
        }

        public List<Domain.Models.InsurancePolicy> GetAllInsurancePolicies()
        {
            if (OnGetAllInsurancePolicies != null)
                OnGetAllInsurancePolicies();

            return new List<Domain.Models.InsurancePolicy>();
        }
    }
}
