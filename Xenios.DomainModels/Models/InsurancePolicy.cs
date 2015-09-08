using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Enums;

namespace Xenios.Domain.Models
{
    public class InsurancePolicy
    {
        public Guid Id { get; set; }

        public InsuranceTypes InsuranceType { get; set; }

        public DateTime CoverageBeginDateTime { get; set; }

        public decimal Price { get; set; }

        public int TermLength { get; set; }

        public TermUnits TermUnit { get; set; }

        public CustomerInformation Customer { get; set; }

        public PaymentInformation PaymentInformation { get; set; }

        public static InsurancePolicy NewInsurancePolicy()
        {
           return new Domain.Models.InsurancePolicy
            {
                Id = Guid.NewGuid(),
                CoverageBeginDateTime = DateTime.Now,
                Customer = new CustomerInformation
                {
                    AddressLine1 = String.Empty,
                    City = String.Empty,
                    Country = String.Empty,
                    FirstName = String.Empty,
                    LastName = String.Empty,
                    PostalCode = String.Empty,
                    State = String.Empty
                },
                InsuranceType = InsuranceTypes.Unspecified,
                PaymentInformation = new PaymentInformation
                {
                    CreditCardNumber = String.Empty,
                    CreditCardType = CreditCardTypes.Unspecified,
                    ExpirationDate = DateTime.Now,
                    CreditCardVerificationNumber = String.Empty
                },
                Price = 0,
                TermLength = 0,
                TermUnit = TermUnits.Months,
            };
        }
    }
}
