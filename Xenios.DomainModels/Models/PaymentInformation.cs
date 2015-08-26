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

        public string CreditCardNumber { get; set; }
    }
}
