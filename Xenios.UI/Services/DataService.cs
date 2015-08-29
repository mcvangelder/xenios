using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Business;

namespace Xenios.UI.Services
{
    public class DataService : IDataService
    {
        public event PoliciesChangedEvent PoliciesChanged;

        public IInsurancePolicyDataService InsurancePolicyDataService { get; set; }

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

        public string SourceFile
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

    }
}
