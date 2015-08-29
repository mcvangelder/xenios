using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Business;

namespace Xenios.Mocks
{
    public delegate void OnLoadEvent();

    public class MockInsurancePolicyDataService : AbsractInsurancePolicyDataService
    {
        public MockInsurancePolicyDataService(String fileName)
            :base(fileName)
        { }

        public event OnSaveEvent OnSave;
        public event OnLoadEvent OnGetAllInsurancePolicies;
        public event OnLoadEvent OnFindInsurancePoliciesByCustomerName;

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
    }
}
