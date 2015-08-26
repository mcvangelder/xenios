using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.Domain.Models
{
    public class InsuranceInformation
    {
        public Guid Id { get; set; }

        public int InsuranceType { get; set; }
        public DateTime CoverageBeginDateTime { get; set; }

        public decimal Price { get; set; }

        public int TermLength { get; set; }

        public int TermUnit { get; set; }

        public CustomerInformation Customer { get; set; }

        public PaymentInformation PaymentInformation { get; set; }
    }
}
