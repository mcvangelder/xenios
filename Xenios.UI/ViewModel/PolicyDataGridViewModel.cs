using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Enums;
using Xenios.Domain.Models;
using Xenios.UI.Utilities;

namespace Xenios.UI.ViewModel
{
    public class PolicyDataGridViewModel : ViewModelBase
    {
        private Domain.Models.InsurancePolicy _policy;
        public Domain.Models.InsurancePolicy InsurancePolicy { get { return _policy; } }

        public PolicyDataGridViewModel(Domain.Models.InsurancePolicy policy)
        {
            _policy = policy;
        }

        public PolicyDataGridViewModel()
        {
            _policy = InsurancePolicy.NewInsurancePolicy();
        }

        /// <summary>
        /// The <see cref="CoverageBeginDateTime" /> property's name.
        /// </summary>
        public const string CoverageBeginDateTimePropertyName = "CoverageBeginDateTime";

        /// <summary>
        /// Sets and gets the CoverageBeginDateTime property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime CoverageBeginDateTime
        {
            get { return _policy.CoverageBeginDateTime; }
            set
            {
                if (_policy.CoverageBeginDateTime == value) return;

                _policy.CoverageBeginDateTime = value;
                RaisePropertyChanged(CoverageBeginDateTimePropertyName); ;
            }
        }

        /// <summary>
        /// The <see cref="CustomerAddressLine1" /> property's name.
        /// </summary>
        public const string CustomerAddressLine1PropertyName = "CustomerAddressLine1";

        /// <summary>
        /// Sets and gets the CustomerAddressLine1 property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String CustomerAddressLine1
        {
            get { return _policy.Customer.AddressLine1; }
            set
            {
                if (_policy.Customer.AddressLine1 == value)
                    return;

                _policy.Customer.AddressLine1 = value;
                RaisePropertyChanged(CustomerAddressLine1PropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CustomerCity" /> property's name.
        /// </summary>
        public const string CustomerCityPropertyName = "CustomerCity";

        /// <summary>
        /// Sets and gets the CustomerCity property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String CustomerCity
        {
            get { return _policy.Customer.City; }
            set
            {
                if (_policy.Customer.City == value)
                    return;

                _policy.Customer.City = value;
                RaisePropertyChanged(CustomerCityPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CustomerCountry" /> property's name.
        /// </summary>
        public const string CustomerCountryPropertyName = "CustomerCountry";

        /// <summary>
        /// Sets and gets the CustomerCountry property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String CustomerCountry
        {
            get { return _policy.Customer.Country; }
            set
            {
                if (_policy.Customer.Country == value)
                    return;

                _policy.Customer.Country = value;
                RaisePropertyChanged(CustomerCountryPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CustomerFirstName" /> property's name.
        /// </summary>
        public const string CustomerFirstNamePropertyName = "CustomerFirstName";

        /// <summary>
        /// Sets and gets the CustomerFirstName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String CustomerFirstName
        {
            get { return _policy.Customer.FirstName; }
            set
            {
                if (_policy.Customer.FirstName == value)
                    return;

                _policy.Customer.FirstName = value;
                RaisePropertyChanged(CustomerFirstNamePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CustomerLastName" /> property's name.
        /// </summary>
        public const string CustomerLastNamePropertyName = "CustomerLastName";

        /// <summary>
        /// Sets and gets the CustomerLastName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String CustomerLastName
        {
            get { return _policy.Customer.LastName; }
            set
            {
                if (_policy.Customer.LastName == value)
                    return;

                _policy.Customer.LastName = value;
                RaisePropertyChanged(CustomerLastNamePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CustomerPostalCode" /> property's name.
        /// </summary>
        public const string CustomerPostalCodePropertyName = "CustomerPostalCode";

        /// <summary>
        /// Sets and gets the CustomerPostalCode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String CustomerPostalCode
        {
            get { return _policy.Customer.PostalCode; }

            set
            {
                if (_policy.Customer.PostalCode == value)
                    return;

                _policy.Customer.PostalCode = value;
                RaisePropertyChanged(CustomerPostalCodePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CustomerState" /> property's name.
        /// </summary>
        public const string CustomerStatePropertyName = "CustomerState";

        /// <summary>
        /// Sets and gets the CustomerState property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String CustomerState
        {
            get { return _policy.Customer.State; }
            set
            {
                if (_policy.Customer.State == value)
                    return;

                _policy.Customer.State = value;
                RaisePropertyChanged(CustomerStatePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="InsuranceType" /> property's name.
        /// </summary>
        public const string InsuranceTypePropertyName = "InsuranceType";

        /// <summary>
        /// Sets and gets the InsuranceType property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public InsuranceTypes InsuranceType
        {
            get
            {
                return _policy.InsuranceType;
            }

            set
            {
                if (_policy.InsuranceType == value)
                {
                    return;
                }

                _policy.InsuranceType = value;
                RaisePropertyChanged(InsuranceTypePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedInsuranceTypes" /> property's name.
        /// </summary>
        public const string SelectedInsuranceTypesPropertyName = "SelectedInsuranceTypes";

        /// <summary>
        /// Sets and gets the SelectedInsuranceTypes property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<InsuranceTypes> SelectedInsuranceTypes
        {
            get
            {
                return (List<InsuranceTypes>)SplitIntoList(InsuranceType);
            }
            set
            {
                InsuranceTypes temp = CombineIntoSingle(value);

                if (InsuranceType == temp)
                    return;

                InsuranceType = temp;
                RaisePropertyChanged(SelectedInsuranceTypesPropertyName);
            }
        }

        private static InsuranceTypes CombineIntoSingle(List<InsuranceTypes> values)
        {
            return  InsuranceTypesListToFlagsConverter.CombineIntoSingle(values);
        }

        private static ICollection<InsuranceTypes> SplitIntoList(Domain.Enums.InsuranceTypes insuranceTypesFlags)
        {
            var split = InsuranceTypesListToFlagsConverter.SplitIntoList(insuranceTypesFlags);
            split.Remove(Domain.Enums.InsuranceTypes.Unspecified);

            return split;
        }

        /// <summary>
        /// The <see cref="PaymentInformationCreditCardNumber" /> property's name.
        /// </summary>
        public const string PaymentInformationCreditCardNumberPropertyName = "PaymentInformationCreditCardNumber";

        /// <summary>
        /// Sets and gets the PaymentInformationCreditCardNumber property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String PaymentInformationCreditCardNumber
        {
            get { return _policy.PaymentInformation.CreditCardNumber; }
            set
            {
                if (_policy.PaymentInformation.CreditCardNumber == value)
                    return;

                _policy.PaymentInformation.CreditCardNumber = value;
                RaisePropertyChanged(PaymentInformationCreditCardNumber);
            }
        }

        /// <summary>
        /// The <see cref="PaymentInformationCreditCardType" /> property's name.
        /// </summary>
        public const string PaymentInformationCreditCardTypePropertyName = "PaymentInformationCreditCardType";

        /// <summary>
        /// Sets and gets the PaymentInformationCreditCardType property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public CreditCardTypes PaymentInformationCreditCardType
        {
            get { return _policy.PaymentInformation.CreditCardType; }
            set
            {
                if (_policy.PaymentInformation.CreditCardType == value)
                    return;

                _policy.PaymentInformation.CreditCardType = value;
                RaisePropertyChanged(PaymentInformationCreditCardTypePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="PaymentInformationCreditCardVerificationNumber" /> property's name.
        /// </summary>
        public const string PaymentInformationCreditCardVerificationNumberPropertyName = "PaymentInformationCreditCardVerificationNumber";

        /// <summary>
        /// Sets and gets the PaymentInformationCreditCardVerificationNumber property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String PaymentInformationCreditCardVerificationNumber
        {
            get { return _policy.PaymentInformation.CreditCardVerificationNumber; }
            set
            {
                if (_policy.PaymentInformation.CreditCardVerificationNumber == value)
                    return;

                _policy.PaymentInformation.CreditCardVerificationNumber = value;
                RaisePropertyChanged(PaymentInformationCreditCardVerificationNumberPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="PaymentInformationExpirationDate" /> property's name.
        /// </summary>
        public const string PaymentInformationExpirationDatePropertyName = "PaymentInformationExpirationDate";

        /// <summary>
        /// Sets and gets the PaymentInformationExpirationDate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime PaymentInformationExpirationDate
        {
            get { return _policy.PaymentInformation.ExpirationDate; }
            set
            {
                if (_policy.PaymentInformation.ExpirationDate == value)
                    return;

                _policy.PaymentInformation.ExpirationDate = value;
                RaisePropertyChanged(PaymentInformationExpirationDatePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Price" /> property's name.
        /// </summary>
        public const string PricePropertyName = "Price";

        /// <summary>
        /// Sets and gets the Price property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public decimal Price
        {
            get { return _policy.Price; }
            set
            {
                if (_policy.Price == value)
                    return;
                _policy.Price = value;
                RaisePropertyChanged(PricePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="TermLength" /> property's name.
        /// </summary>
        public const string TermLengthPropertyName = "TermLength";

        /// <summary>
        /// Sets and gets the TermLength property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TermLength
        {
            get { return _policy.TermLength; }
            set
            {
                if (_policy.TermLength == value)
                    return;

                _policy.TermLength = value;
                RaisePropertyChanged(TermLengthPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="TermUnit" /> property's name.
        /// </summary>
        public const string TermUnitPropertyName = "TermUnit";

        /// <summary>
        /// Sets and gets the TermUnit property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public TermUnits TermUnit
        {
            get { return _policy.TermUnit; }
            set
            {
                if (_policy.TermUnit == value)
                    return;

                _policy.TermUnit = value;
                RaisePropertyChanged(TermUnitPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsIncludedInFilter" /> property's name.
        /// </summary>
        public const string IsIncludedInFilterPropertyName = "IsIncludedInFilter";

        private bool _isIncludedInFilter = true;

        /// <summary>
        /// Sets and gets the IsIncludedInFilter property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsIncludedInFilter
        {
            get
            {
                return _isIncludedInFilter;
            }

            set
            {
                if (_isIncludedInFilter == value)
                {
                    return;
                }

                _isIncludedInFilter = value;
                RaisePropertyChanged(IsIncludedInFilterPropertyName);
            }
        }
    }
}
