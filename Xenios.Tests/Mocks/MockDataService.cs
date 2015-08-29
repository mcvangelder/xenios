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
        public List<Domain.Models.InsuranceInformation> GetAllInsuranceInformations()
        {
            return Xenios.Test.Helpers.InsuranceInformationHelper.CreateInsuranceInformations(5);
        }

        public String SourceFile { get; set; }
    }
}
