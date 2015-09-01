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
            var currentPolicies = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicies(5);
            var refreshedPolicies = new List<Domain.Models.InsurancePolicy>(currentPolicies);

            // simulate user has modified the collection
            currentPolicies.Add(Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy());
            // simulate source collection has been modified by another user
            refreshedPolicies.Add(Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy());

            var merged = Utilities.CollectionHelper.Merge(currentPolicies, refreshedPolicies);
            Assert.AreEqual(7, merged.Count);
        }
    }
}
