using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.UI.Services
{
    public interface IApplicationService
    {
        void ExitApplication();

        String ChooseFile();

        void Alert(string p);
    }
}
