using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.Domain.Models
{
    public class CustomerInformation
    {
        public CustomerInformation()
        {
            Id = Guid.NewGuid();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AddressLine1 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public Guid Id { get; set; }

        public string Country { get; set; }
    }
}
