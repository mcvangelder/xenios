using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.Business.Util
{
    public class CollectionHelper
    {
        public static List<Domain.Models.InsurancePolicy> Merge(
            List<Domain.Models.InsurancePolicy> currentPolicies,
            List<Domain.Models.InsurancePolicy> refreshedPolicies)
        {
            refreshedPolicies.RemoveAll( (match) => 
            {
                var currentPolicy = currentPolicies.FirstOrDefault(p => p.Id == match.Id);
                if (currentPolicy == null)
                    return false;

                return currentPolicy.LastUpdateDate >= match.LastUpdateDate;
            });
            var newList = refreshedPolicies.Union(currentPolicies, new PolicyEqualityComparer()).ToList();

            return newList;
        }
    }
}
