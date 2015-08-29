using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Business;

namespace Xenios.UI.Services
{
    public class DataService
    {
        public IInsurancePolicyDataService InsurancePolicyDataService { get; set; }

        public void Save(List<Domain.Models.InsurancePolicy> policies)
        {
            InsurancePolicyDataService.Save(policies);
        }

        public void GetAllInsurancePolicies()
        {
            InsurancePolicyDataService.GetAllInsurancePolicies();
        }
    }
}
