using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.Domain.Enums
{
   public enum CreditCardTypes : byte
    {
        Unspecified = 0,
        MasterCard = 1,
        Visa = 2,
        DiscoverCard = 3,
        Amex = 4,
    }
}
