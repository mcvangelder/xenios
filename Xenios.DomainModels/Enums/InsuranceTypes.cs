using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.Domain.Enums
{
    [Flags]
    public enum InsuranceTypes : byte
    {
        Unspecified = 0,
        Comprehensive = 1,
        Liability = 2,
        Collision = 4,
        Glass = 8,
        Umbrella = 16
    }
}
