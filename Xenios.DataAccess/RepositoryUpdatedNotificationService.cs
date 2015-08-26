using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.DataAccess
{
    public delegate void RepositoryUpdated(InsuranceInformationRepository repository);

    public class RepositoryUpdatedNotificationService
    {
        private InsuranceInformationRepository _repository;
        private FileSystemWatcher _fileSystemWatcher;

        public RepositoryUpdatedNotificationService(InsuranceInformationRepository repository)
        {
            _repository = repository;
            _fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(repository.FileName), Path.GetFileName(repository.FileName));
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileSystemWatcher.Changed += _fileSystemWatcher_Changed;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        void _fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (NotifyRepositoryUpdated != null)
                NotifyRepositoryUpdated(_repository);
        }

        public event RepositoryUpdated NotifyRepositoryUpdated; 
    }
}
