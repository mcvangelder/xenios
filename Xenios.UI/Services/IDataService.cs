using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Models;

namespace Xenios.UI.Services
{
    public delegate void PoliciesChangedEvent(List<InsurancePolicy> newPolicyList);
    public interface IDataService
    {
        event PoliciesChangedEvent PoliciesChanged;

        String SourceFile { get; set; }

        List<InsurancePolicy> GetAllInsurancePolicies();

        List<InsurancePolicy> FindInsurancePoliciesByCustomerName(string searchValue);

        void Save(List<InsurancePolicy> policies);
    }
}
