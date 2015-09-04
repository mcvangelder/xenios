using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.UI.ViewModel
{
    public partial class InsurancePolicyViewModel
    {
        public RelayCommand RefreshPolicyListCommand { get; set; }
        public RelayCommand SavePoliciesCommand { get; set; }
        public RelayCommand ExitApplicationCommand { get; set; }
        public RelayCommand CloseFileCommand { get; set; }
        public RelayCommand OpenFileDialogCommand { get; set; }

    }
}
