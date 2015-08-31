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
        public InsurancePolicy()
        {
            Id = Guid.NewGuid();
            CoverageBeginDateTime = DateTime.Now;
            TermUnit = TermUnits.Months;
        }

        public Guid Id { get; set; }

        public InsuranceTypes InsuranceType { get; set; }

        public DateTime CoverageBeginDateTime { get; set; }

        public decimal Price { get; set; }

        public int TermLength { get; set; }

        public TermUnits TermUnit { get; set; }

        public CustomerInformation Customer { get; set; }

        public PaymentInformation PaymentInformation { get; set; }
    }
}
