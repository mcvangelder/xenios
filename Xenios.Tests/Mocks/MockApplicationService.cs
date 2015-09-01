using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.UI.Services;

namespace Xenios.Mocks
{

    public class MockApplicationService : IApplicationService
    {
        public event OnCalledEvent OnExit;
        public event OnCalledEvent OnChooseFile;
        public event OnCalledEvent OnAlert;

        public void ExitApplication()
        {
            if(OnExit != null)
            {
                OnExit();
            }
        }

        public string ChooseFile()
        {
            if (OnChooseFile != null)
                OnChooseFile();

            return String.Empty;
        }

        public void Alert(string p)
        {
            if(OnAlert != null)
            {
                OnAlert();
            }
        }
    }
}
