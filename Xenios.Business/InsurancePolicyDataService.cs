using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Models;

namespace Xenios.Business
{
    public delegate void InsurancePoliciesUpdated();

    public class InsurancePolicyDataService : AbsractInsurancePolicyDataService, IDisposable
    {
        private DataAccess.InsurancePolicyRepository _policiesRepository;
        private DataAccess.RepositoryUpdatedNotificationService _repositoryUpdatedNotificationService;

        public InsurancePolicyDataService(String sourceFile) : base(sourceFile)
        {
            _policiesRepository = new DataAccess.InsurancePolicyRepository(sourceFile);
            CreateinsurancePolicyNotificationService();
        }

        private void CreateinsurancePolicyNotificationService()
        {
            _repositoryUpdatedNotificationService = new DataAccess.RepositoryUpdatedNotificationService(_policiesRepository);
            _repositoryUpdatedNotificationService.NotifyRepositoryUpdated += (repository) => { RaiseNotifyInsurancePoliciesUpdated(); };
        }

        public override List<InsurancePolicy> GetAllInsurancePolicies()
        {
            return _policiesRepository.GetAll();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_repositoryUpdatedNotificationService != null)
                    _repositoryUpdatedNotificationService.Dispose();
            }
        }
        public override void Save(List<InsurancePolicy> insurancePolicies)
        {
            _policiesRepository.SaveAll(insurancePolicies);
        }

        ~InsurancePolicyDataService()
        {
            Dispose(true);
        }

        public override List<InsurancePolicy> RefreshPolicies(List<InsurancePolicy> existingRecords)
        {
            return Util.CollectionHelper.Merge(existingRecords, _policiesRepository.GetAll());
        }
    }
}
