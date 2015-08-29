using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.UI.Services;

namespace Xenios.Mocks
{
    public delegate void OnExitEvent();

    public class MockApplicationService : IApplicationService
    {
        public event OnExitEvent OnExit;

        public void Exit()
        {
            if(OnExit != null)
            {
                OnExit();
            }
        }
    }
}
