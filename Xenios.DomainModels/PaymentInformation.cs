using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.DomainModels
{
    public class PaymentInformation
    {
        public Guid Id { get; set; }

        public int CreditCardType { get; set; }

        public string CreditCardNumber { get; set; }
    }
}
