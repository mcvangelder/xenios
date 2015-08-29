using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.UI.Services;

namespace Xenios.Mocks
{
    internal class MockDataService : IDataService
    {
        private List<Domain.Models.InsurancePolicy> _insuranceInfos;
        public String SourceFile { get; set; }
        
        public MockDataService()
        {
            _insuranceInfos = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(5);
        }

        public List<Domain.Models.InsurancePolicy> GetAllInsuranceInformations()
        {
            return _insuranceInfos;
        }

        public List<Domain.Models.InsurancePolicy> FindInsuranceInformationsByCustomerName(string searchValue)
        {
            return new List<Domain.Models.InsurancePolicy>() { _insuranceInfos.First() };
        }
    }
}
