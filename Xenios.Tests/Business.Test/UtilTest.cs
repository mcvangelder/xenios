using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Xenios.Business.Test
{
    [TestClass]
    public class UtilTest
    {
        [TestMethod]
        public void Should_merge_unique_by_policy_id_items_into_one_list()
        {
            var policyCount = 5;
            var currentPolicies = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(policyCount);
            var storePolicies = new List<Domain.Models.InsurancePolicy>(currentPolicies);

            // simulate user has modified the collection
            currentPolicies.Add(Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy());
            // simulate source collection has been modified by another user
            storePolicies.Add(Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy());

            var mergedPolicies = Util.CollectionHelper.Merge(currentPolicies, storePolicies);
            Assert.AreEqual(policyCount + 2, mergedPolicies.Count);
        }

        [TestMethod]
        public void Should_resolve_conflicts_during_merge_as_most_recently_updated_wins()
        {
            var policyCount = 5;
            var now = DateTime.Now;

            var currentPolicies = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(policyCount);
            // simulate all loaded policies have been modified 1 min in the future
            currentPolicies.ForEach(p => p.LastUpdateDate = now.AddMinutes(1));

            var storePolicies = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(policyCount);
            // simulate all store policies were lst updated 1 min in the past
            storePolicies.ForEach(p => p.LastUpdateDate = now.AddMinutes(-1));

            for (int i = 0; i < policyCount; i++)
            {
                // make them "equal" in the eyes of the merge
                currentPolicies[i].Id = storePolicies[i].Id;
            }

            var mergedPolicies = Util.CollectionHelper.Merge(currentPolicies, storePolicies);
            Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(currentPolicies, mergedPolicies);
        }
    }
}
