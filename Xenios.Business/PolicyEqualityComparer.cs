using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.Business
{
    public class PolicyEqualityComparer : IEqualityComparer<Domain.Models.InsurancePolicy>
    {

        public bool Equals(Domain.Models.InsurancePolicy x, Domain.Models.InsurancePolicy y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Domain.Models.InsurancePolicy obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
