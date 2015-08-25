using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.DomainModels;

namespace Xenios.DataAccess
{
    public class InsuranceInformationService
    {
        private string _fileName;

        public InsuranceInformationService(string fileName)
        {
            _fileName = fileName;
        }

        public void Save(InsuranceInformation insuranceInformation)
        {
            throw new NotImplementedException();
        }

        public List<InsuranceInformation> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
