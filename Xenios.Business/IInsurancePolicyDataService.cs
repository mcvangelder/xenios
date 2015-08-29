using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.Business
{
    public interface IInsurancePolicyDataService
    {
        void Save(List<Domain.Models.InsurancePolicy> policies);

        List<Domain.Models.InsurancePolicy> GetAllInsurancePolicies();

        List<Domain.Models.InsurancePolicy> FindInsurancePoliciesByCustomerName(string searchValue);
    }
}
