using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Enums;
using Xenios.Domain.Models;

namespace Xenios.UI.ViewModel
{
    public class PolicyDataGridViewModel
    {
        private Domain.Models.InsurancePolicy _policy;
        public Domain.Models.InsurancePolicy InsurancePolicy { get { return _policy; } }

        public PolicyDataGridViewModel(Domain.Models.InsurancePolicy policy)
        {
            _policy = policy;
        }

        public PolicyDataGridViewModel()
        {
            _policy = new Domain.Models.InsurancePolicy();
            _policy.Id = Guid.NewGuid();
            _policy.CoverageBeginDateTime = DateTime.Now;
            _policy.TermUnit = TermUnits.Months;
            _policy.Customer = new CustomerInformation();
            _policy.PaymentInformation = new PaymentInformation();
        }

        public DateTime CoverageBeginDateTime { get { return _policy.CoverageBeginDateTime; } set { _policy.CoverageBeginDateTime = value; } }

        public String CustomerAddressLine1 { get { return _policy.Customer.AddressLine1; } set { _policy.Customer.AddressLine1 = value; } }

        public String CustomerCity { get { return _policy.Customer.City; } set { _policy.Customer.City = value; } }

        public String CustomerCountry { get { return _policy.Customer.Country; } set { _policy.Customer.Country = value; } }

        public String CustomerFirstName { get { return _policy.Customer.FirstName; } set { _policy.Customer.FirstName = value; } }

        public String CustomerLastName { get { return _policy.Customer.LastName; } set { _policy.Customer.LastName = value; } }

        public String CustomerPostalCode { get { return _policy.Customer.PostalCode; } set { _policy.Customer.PostalCode = value; } }

        public String CustomerState { get { return _policy.Customer.State; } set { _policy.Customer.State = value; } }

        public InsuranceTypes InsuranceType { get { return _policy.InsuranceType; } set { _policy.InsuranceType = value; } }

        public String PaymentInformationCreditCardNumber { get { return _policy.PaymentInformation.CreditCardNumber; } set { _policy.PaymentInformation.CreditCardNumber = value; } }

        public CreditCardTypes PaymentInformationCreditCardType { get { return _policy.PaymentInformation.CreditCardType; } set { _policy.PaymentInformation.CreditCardType = value; } }

        public String PaymentInformationCreditCardVerificationNumber { get { return _policy.PaymentInformation.CreditCardVerificationNumber; } set { _policy.PaymentInformation.CreditCardVerificationNumber = value; } }

        public DateTime PaymentInformationExpirationDate { get { return _policy.PaymentInformation.ExpirationDate; } set { _policy.PaymentInformation.ExpirationDate = value; } }

        public decimal Price { get { return _policy.Price; } set { _policy.Price = value; } }

        public int TermLength { get { return _policy.TermLength; } set { _policy.TermLength = value; } }

        public TermUnits TermUnit { get { return _policy.TermUnit; } set { _policy.TermUnit = value; } }
    }
}
