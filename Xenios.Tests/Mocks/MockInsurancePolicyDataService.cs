using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Business;

namespace Xenios.Mocks
{
    public class MockInsurancePolicyDataService : IInsurancePolicyDataService
    {
        public event OnSaveEvent OnSave;

        public void Save(List<Domain.Models.InsurancePolicy> policies)
        {
            if (OnSave != null)
                OnSave(policies);
        }
    }
}
