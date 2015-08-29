using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Models;

namespace Xenios.Business
{
    public delegate void InsurancePoloicyUpdated(List<InsurancePolicy> reloadedInformations);

    public class InsurancePolicyDataService : IDisposable
    {
        private DataAccess.InsurancePolicyRepository _informationRepository;
        private DataAccess.RepositoryUpdatedNotificationService _repositoryUpdatedNotificationService;

        public InsurancePolicyDataService(String sourceFile)
        {
            _informationRepository = new DataAccess.InsurancePolicyRepository(sourceFile);
            CreateinsurancePolicyNotificationService();
        }

        private void CreateinsurancePolicyNotificationService()
        {
            _repositoryUpdatedNotificationService = new DataAccess.RepositoryUpdatedNotificationService(_informationRepository);
            _repositoryUpdatedNotificationService.NotifyRepositoryUpdated += _repositoryUpdatedNotificationService_NotifyRepositoryUpdated;
        }

        void _repositoryUpdatedNotificationService_NotifyRepositoryUpdated(DataAccess.InsurancePolicyRepository repository)
        {
            if(NotifyInsurancePolicyUpdated != null)
            {
                NotifyInsurancePolicyUpdated(repository.GetAll());
            }
        }

        public List<InsurancePolicy> GetAllInsurancePolicies()
        {
            return _informationRepository.GetAll();
        }

        public List<InsurancePolicy> FindInsurancePoliciesByCustomerName(String customerName)
        {
            return _informationRepository.GetAll().Where(
                            policy => policy.Customer.FirstName.ToLower().Contains(customerName.ToLower())
                    ).ToList();
        }

        public event InsurancePoloicyUpdated NotifyInsurancePolicyUpdated;

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

        public void Save(InsurancePolicy insurancePolicy)
        {
            _informationRepository.Save(insurancePolicy);
        }
    }
}
