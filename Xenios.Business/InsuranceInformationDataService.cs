using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Models;

namespace Xenios.Business
{
    public delegate void InsuranceInformationUpdated(List<InsuranceInformation> reloadedInformations);

    public class InsuranceInformationDataService : IDisposable
    {
        private DataAccess.InsuranceInformationRepository _informationRepository;
        private DataAccess.RepositoryUpdatedNotificationService _repositoryUpdatedNotificationService;

        public InsuranceInformationDataService(String sourceFile)
        {
            _informationRepository = new DataAccess.InsuranceInformationRepository(sourceFile);
            CreateInsuranceInformationNotificationService();
        }

        private void CreateInsuranceInformationNotificationService()
        {
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

        public List<InsuranceInformation> FindInsurancePoliciesByCustomerName(String customerName)
        {
            return _informationRepository.GetAll().Where(
                            policy => policy.Customer.FirstName.ToLower().Contains(customerName.ToLower())
                    ).ToList();
        }

        public event InsuranceInformationUpdated NotifyInsuranceInformationUpdated;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        public void Dispose(bool disposing)
        {
            if(disposing)
            {
                _repositoryUpdatedNotificationService.Dispose();
            }
        }

        public void Save(InsuranceInformation insuranceInformation)
        {
            _informationRepository.Save(insuranceInformation);
        }
    }
}
