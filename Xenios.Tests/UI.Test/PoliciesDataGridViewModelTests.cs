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

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_customer_first_name()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.CustomerFirstName = "Updated";
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_customer_last_name()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.CustomerLastName = "Updated";
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_coverage_date()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.CoverageBeginDateTime = DateTime.Now;
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_customer_address1()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.CustomerAddressLine1 = "updated";
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_customer_city()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.CustomerCity = "updated";
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_customer_country()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.CustomerCountry = "updated";
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_customer_postalcode()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.CustomerPostalCode = "updated";
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_customer_state()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.CustomerState = "updated";
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_insurance_type()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.InsuranceType = Domain.Enums.InsuranceTypes.Collision | Domain.Enums.InsuranceTypes.Comprehensive;
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_credit_card_number()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.PaymentInformationCreditCardNumber = "updated";
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_credit_card_type()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.PaymentInformationCreditCardType = Domain.Enums.CreditCardTypes.Discover;
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_credit_card_verification_number()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.PaymentInformationCreditCardVerificationNumber = "updated";
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_credit_card_expiration_date()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.PaymentInformationExpirationDate = DateTime.Now;
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_price()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.Price = decimal.MaxValue;
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_term_length()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.TermLength = int.MaxValue;
            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        [TestMethod]
        public void Should_update_last_update_date_when_modifying_term_unit()
        {
            var previousLastUpdateDate = GetLastUpdateDate();

            _dataGridViewModel.TermUnit = Domain.Enums.TermUnits.Days;

            var newLastUpdateDate = _policy.LastUpdateDate;

            Assert.AreNotEqual(previousLastUpdateDate, newLastUpdateDate);
        }

        private DateTime GetLastUpdateDate()
        {
            var previousLastUpdateDate = _policy.LastUpdateDate;
            System.Threading.Thread.Sleep(50); // build in delay to better detect last update date change
            return previousLastUpdateDate;
        }
    }
}
