using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Enums;

namespace Xenios.Domain.Models
{
    public class PaymentInformation
    {
        public Guid Id { get; set; }

        public CreditCardTypes CreditCardType { get; set; }

        public String CreditCardNumber { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string CreditCardVerificationNumber { get; set; }
    }
}
