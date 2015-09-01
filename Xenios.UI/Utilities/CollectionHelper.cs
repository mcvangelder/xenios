using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Business;

namespace Xenios.UI.Utilities
{
    public class CollectionHelper
    {
        public static List<Domain.Models.InsurancePolicy> Merge(
            List<Domain.Models.InsurancePolicy> currentPolicies,
            List<Domain.Models.InsurancePolicy> refreshedPolicies)
        {
            var newList = refreshedPolicies.Union(currentPolicies, new PolicyEqualityComparer()).ToList();

            return newList;
        }
    }
}
