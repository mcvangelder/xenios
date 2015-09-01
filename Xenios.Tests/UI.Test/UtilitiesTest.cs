using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xenios.UI.Utilities;
using System.Collections.Generic;

namespace Xenios.UI.Test
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void Should_create_collection_of_enum()
        {
            var collection = EnumHelper.GetAllAsCollection<List<Domain.Enums.TermUnits>,Domain.Enums.TermUnits>();
            var actualValues = Enum.GetValues(typeof(Domain.Enums.TermUnits));
            var actualCount = actualValues.Length;
            var collectionCount = collection.Count;

            Assert.AreEqual(actualCount,collectionCount);
        }

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

            var mergedPolicies = Utilities.CollectionHelper.Merge(currentPolicies, storePolicies);
            Assert.AreEqual(policyCount + 2, mergedPolicies.Count);
        }

        [TestMethod]
        public void Should_resolve_conflicts_during_merge_as_store_wins_entire_record()
        {
            var policyCount = 5;
            var currentPolicies = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(policyCount);
            var storePolicies = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(policyCount);

            for(int i = 0; i < policyCount; i++)
            {
                // make them "equal" in the eyes of the merge
                currentPolicies[i].Id = storePolicies[i].Id;
            }

            var mergedPolicies = Utilities.CollectionHelper.Merge(currentPolicies, storePolicies);
            Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(storePolicies, mergedPolicies);
        }

    }
}
