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
        public event OnCalledEvent OnOpenFileDialog;

        public void Exit()
        {
            if(OnExit != null)
            {
                OnExit();
            }
        }

        public void OpenFileDialog()
        {
            if (OnOpenFileDialog != null)
                OnOpenFileDialog();
        }
    }
}
