using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.Business
{
    public abstract class AbsractInsurancePolicyDataService
    {
        public event InsurancePoliciesUpdated NotifyInsurancePoliciesUpdated;

        protected String FileName { get; set; }

        protected AbsractInsurancePolicyDataService(String fileName)
        {
            FileName = fileName;
        }

        public abstract void Save(List<Domain.Models.InsurancePolicy> policies);

        public abstract List<Domain.Models.InsurancePolicy> GetAllInsurancePolicies();

        public abstract List<Domain.Models.InsurancePolicy> FindInsurancePoliciesByCustomerName(string searchValue);

        protected void RaiseNotifyInsurancePoliciesUpdated(List<Domain.Models.InsurancePolicy> policies)
        {
            if (NotifyInsurancePoliciesUpdated != null)
            {
                NotifyInsurancePoliciesUpdated(policies);
            }
        }
    }
}
