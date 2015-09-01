using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xenios.Business.Test
{
    [TestClass]
    public class EqualityComparerTest
    {
        private PolicyEqualityComparer _policyEqualityComparer = new PolicyEqualityComparer();

        [TestMethod]
        public void Should_evalute_two_policies_are_the_same_when_they_have_the_same_Id()
        {
            var policy1 = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy();
            var policy2 = policy1;

            var isEqual = _policyEqualityComparer.Equals(policy1, policy2);
            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        public void Should_evaluate_two_policies_are_different_when_they_have_different_Id()
        {
            var policy1 = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy();
            var policy2 = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy();

            var isEqual = _policyEqualityComparer.Equals(policy1, policy2);
            Assert.IsFalse(isEqual);
        }

        [TestMethod]
        public void Should_evaluate_two_policies_are_same_if_both_are_null()
        {
            Domain.Models.InsurancePolicy policy1 = null;
            Domain.Models.InsurancePolicy policy2 = null;

            var isEqual = _policyEqualityComparer.Equals(policy1, policy2);
            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        public void Should_evaluate_two_policies_are_different_if_first_is_null()
        {
            Domain.Models.InsurancePolicy policy1 = null;
            var policy2 = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy();

            var isEqual = _policyEqualityComparer.Equals(policy1, policy2);
            Assert.IsFalse(isEqual);
        }

        [TestMethod]
        public void Should_evaluate_two_policies_are_different_if_second_is_null()
        {
            Domain.Models.InsurancePolicy policy2 = null;
            var policy1 = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy();

            var isEqual = _policyEqualityComparer.Equals(policy1, policy2);
            Assert.IsFalse(isEqual);
        }
    }
}
