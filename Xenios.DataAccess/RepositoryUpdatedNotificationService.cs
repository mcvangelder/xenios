using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.DataAccess
{
    public delegate void RepositoryUpdated(InsuranceInformationRepository repository);

    public class RepositoryUpdatedNotificationService
    {
        private InsuranceInformationRepository _repository;

        public RepositoryUpdatedNotificationService(InsuranceInformationRepository repository)
        {
            _repository = repository;
        }

        public event RepositoryUpdated NotifyRepositoryUpdated; 
    }
}
