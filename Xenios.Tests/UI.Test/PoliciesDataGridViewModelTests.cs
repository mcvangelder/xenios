using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xenios.UI.ViewModel;
using Xenios.Domain.Models;

namespace Xenios.UI.Test
{
    /// <summary>
    /// Summary description for PoliciesDataGridViewModelTests
    /// </summary>
    [TestClass]
    public class PoliciesDataGridViewModelTests
    {
        private PolicyDataGridViewModel _dataGridViewModel;
        private InsurancePolicy _policy;

        [TestInitialize]
        public void InitializeTest()
        {
            _policy = Xenios.Test.Helpers.InsurancePolicyHelper.CreateInsurancePolicy();
            _dataGridViewModel = new PolicyDataGridViewModel(_policy);
        }

        [TestMethod]
        public void Should_flatten_InsurancePolicy_domain_object_for_datagrid()
        {
            Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(_policy, _dataGridViewModel);
        }

        [TestMethod]
        public void Should_create_InsurancePolicy_domain_object_graph_when_creating_PolicyDataGridViewModel_with_default_constructor()
        {
            var dataGridViewModel = new PolicyDataGridViewModel();
            var defaultEmptyPolicy = dataGridViewModel.InsurancePolicy;

            Xenios.Test.Helpers.InsurancePolicyHelper.AssertAreEqual(defaultEmptyPolicy, dataGridViewModel);
        }

        [TestMethod]
        public void Should_split_InsuranceTypes_into_list_InsuranceType_enums()
        {
            _policy.InsuranceType = Domain.Enums.InsuranceTypes.Collision |
                                    Domain.Enums.InsuranceTypes.Comprehensive |
                                    Domain.Enums.InsuranceTypes.Glass |
                                    Domain.Enums.InsuranceTypes.Liability |
                                    Domain.Enums.InsuranceTypes.Umbrella;

            var selectedInsuranceTypes = _dataGridViewModel.SelectedInsuranceTypes;

            AssertAllInsuranceTypesArePresent(_policy.InsuranceType,selectedInsuranceTypes);
        }

        private static void AssertAllInsuranceTypesArePresent(Domain.Enums.InsuranceTypes expected, List<Domain.Enums.InsuranceTypes> selectedInsuranceTypes)
        {
            foreach (var val in selectedInsuranceTypes)
            {
                Assert.IsTrue((expected & val) != 0);
            }
        }

        [TestMethod]
        public void Should_combine_SelectedInsuranceTypes_into_single_enum()
        {
            var selectedInsuranceTypes = new List<Domain.Enums.InsuranceTypes>
            {
                Domain.Enums.InsuranceTypes.Collision, 
                Domain.Enums.InsuranceTypes.Comprehensive,
                Domain.Enums.InsuranceTypes.Glass,
                Domain.Enums.InsuranceTypes.Liability,
                Domain.Enums.InsuranceTypes.Umbrella
            };

            _dataGridViewModel.SelectedInsuranceTypes = selectedInsuranceTypes;

            AssertAllInsuranceTypesArePresent(_policy.InsuranceType, selectedInsuranceTypes);
        }
    }
}
