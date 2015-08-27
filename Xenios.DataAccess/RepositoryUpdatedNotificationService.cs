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

    public class RepositoryUpdatedNotificationService : IDisposable
    {
        private InsuranceInformationRepository _repository;
        private FileSystemWatcher _fileSystemWatcher;

        public RepositoryUpdatedNotificationService(InsuranceInformationRepository repository)
        {
            _repository = repository;
            ConfigureFileWatcher(repository);
        }

        private void ConfigureFileWatcher(InsuranceInformationRepository repository)
        {
            var fileName = repository.FileName;
            var directory = Path.GetDirectoryName(fileName);
            var file = Path.GetFileName(fileName);

            _fileSystemWatcher = new FileSystemWatcher(directory, file);
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

        public void Dispose(bool dispose)
        {
            if(dispose)
            {
                _fileSystemWatcher.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
