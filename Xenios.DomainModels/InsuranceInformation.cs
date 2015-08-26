using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.DomainModels
{
    public class InsuranceInformation
    {
        public Guid Id { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public string AddressLine1 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public int InsuranceType { get; set; }

        public DateTime CoverageBeginDateTime { get; set; }

        public int CreditCardType { get; set; }

        public string CreditCardNumber { get; set; }

        public string CountryOfBirth { get; set; }

        public decimal Price { get; set; }

        public int TermLength { get; set; }

        public int TermUnit { get; set; }
    }
}
