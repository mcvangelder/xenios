using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.DataAccess
{
    public delegate void RepositoryUpdated(InsurancePolicyRepository repository);

    public class RepositoryUpdatedNotificationService : IDisposable
    {
        private InsurancePolicyRepository _repository;
        private FileSystemWatcher _fileSystemWatcher;

        public RepositoryUpdatedNotificationService(InsurancePolicyRepository repository)
        {
            _repository = repository;
            ConfigureFileWatcher(repository);
        }

        private void ConfigureFileWatcher(InsurancePolicyRepository repository)
        {
            var fileName = repository.FileName;
            var directory = Path.GetDirectoryName(fileName);
            var file = Path.GetFileName(fileName);

            _fileSystemWatcher = new FileSystemWatcher(directory, file);
            // TODO: Consider Deletes and Renames if time
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileSystemWatcher.Changed += fileSystemWatcher_Changed;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (NotifyRepositoryUpdated != null)
                NotifyRepositoryUpdated(_repository);
        }

        public event RepositoryUpdated NotifyRepositoryUpdated;

        private void Dispose(bool dispose)
        {
            if (dispose)
            {
                if (_fileSystemWatcher != null)
                    _fileSystemWatcher.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RepositoryUpdatedNotificationService()
        {
            Dispose(true);
        }
    }
}
