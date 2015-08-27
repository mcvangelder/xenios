using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Models;

namespace Xenios.Business
{
    public delegate void InsuranceInformationUpdated(List<InsuranceInformation> reloadedInformations);

    public class InsuranceInformationRetrievalService
    {
        private DataAccess.InsuranceInformationRepository _informationRepository;

        public InsuranceInformationRetrievalService(String sourceFile)
        {
            _informationRepository = new DataAccess.InsuranceInformationRepository(sourceFile);
        }

        public List<InsuranceInformation> GetAllInsurancePolicies()
        {
            return _informationRepository.GetAll();
        }

        public event InsuranceInformationUpdated NotifyInsuranceInformationUpdated;
    }
}
