using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xenios.UI.ViewModel;

namespace Xenios.UI.Test
{
    /// <summary>
    /// Summary description for PoliciesDataGridViewModelTests
    /// </summary>
    [TestClass]
    public class PoliciesDataGridViewModelTests
    {
        [TestMethod]
        public void Should_flatten_InsurancePolicy_domain_object_for_datagrid()
        {
            var policy = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy();
            var dataGridViewModel = new PolicyDataGridViewModel(policy);
            Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(policy, dataGridViewModel);
        }

        [TestMethod]
        public void Should_create_InsurancePolicy_domain_object_graph_when_creating_PolicyDataGridViewModel_with_default_constructor()
        {
            var dataGridViewModel = new PolicyDataGridViewModel();
            var policy = dataGridViewModel.InsurancePolicy;

            Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(policy, dataGridViewModel);
        }
    }
}
