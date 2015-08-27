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
        private DataAccess.RepositoryUpdatedNotificationService _repositoryUpdatedNotificationService;

        public InsuranceInformationRetrievalService(String sourceFile)
        {
            _informationRepository = new DataAccess.InsuranceInformationRepository(sourceFile);
            _repositoryUpdatedNotificationService = new DataAccess.RepositoryUpdatedNotificationService(_informationRepository);
            _repositoryUpdatedNotificationService.NotifyRepositoryUpdated += _repositoryUpdatedNotificationService_NotifyRepositoryUpdated;
        }

        void _repositoryUpdatedNotificationService_NotifyRepositoryUpdated(DataAccess.InsuranceInformationRepository repository)
        {
            if(NotifyInsuranceInformationUpdated != null)
            {
                NotifyInsuranceInformationUpdated(repository.GetAll());
            }
        }

        public List<InsuranceInformation> GetAllInsurancePolicies()
        {
            return _informationRepository.GetAll();
        }

        public event InsuranceInformationUpdated NotifyInsuranceInformationUpdated;
    }
}
