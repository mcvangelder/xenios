using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Models;

namespace Xenios.UI.Services
{
    public delegate void PoliciesChangedEvent();
    public interface IPolicyDataService
    {
        event PoliciesChangedEvent PoliciesChanged;

        String SourceFile { set; }

        List<InsurancePolicy> GetAllInsurancePolicies();

        void Save(List<InsurancePolicy> policies);

        List<InsurancePolicy> RefreshPolicies(List<InsurancePolicy> currentPolicies);
    }
}
