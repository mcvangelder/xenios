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
        private List<Domain.Models.InsuranceInformation> _insuranceInfos;
        public String SourceFile { get; set; }
        
        public MockDataService()
        {
            _insuranceInfos = Xenios.Test.Helpers.InsuranceInformationHelper.CreateInsuranceInformations(5);
        }

        public List<Domain.Models.InsuranceInformation> GetAllInsuranceInformations()
        {
            return _insuranceInfos;
        }

        public List<Domain.Models.InsuranceInformation> FindInsuranceInformationsByCustomerName(string searchValue)
        {
            return new List<Domain.Models.InsuranceInformation>() { _insuranceInfos.First() };
        }
    }
}
