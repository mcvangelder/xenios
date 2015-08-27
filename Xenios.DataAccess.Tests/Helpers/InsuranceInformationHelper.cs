using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.Test.Helpers
{
    public class InsuranceInformationHelper
    {
        public static List<Xenios.Domain.Models.InsuranceInformation> CreateInsuranceInformations(int count)
        {
            var result = new List<Xenios.Domain.Models.InsuranceInformation>(count);
            for (var i = 0; i < count; i++)
                result.Add(CreateInsuranceInformation());

            return result;
        }

        public static Domain.Models.InsuranceInformation CreateInsuranceInformation()
        {
            var guidString = Guid.NewGuid().ToString();

            var insuranceInformation = new Domain.Models.InsuranceInformation
            {
                Id = Guid.NewGuid(),
                Customer = new Domain.Models.CustomerInformation
                {
                    Id = Guid.NewGuid(),
                    FirstName = guidString.Substring(0,8),
                    LastName = guidString.Substring(20),
                    AddressLine1 = "123 Some Street",
                    City = guidString.Substring(9,4),
                    State = guidString.Substring(14,4),
                    PostalCode = "12345",
                    Country = "United States",
                },
                PaymentInformation = new Domain.Models.PaymentInformation
                {
                    Id = Guid.NewGuid(),
                    CreditCardType = Domain.Enums.CreditCardTypes.Amex,
                    CreditCardNumber = "1234-5678-9012-3456",
                    ExpirationDate = DateTime.Now,
                    CreditCardVerificationNumber = "001"
                },
                InsuranceType = Domain.Enums.InsuranceTypes.Comprehensive,
                CoverageBeginDateTime = DateTime.Now,
                Price = (decimal)274.00,
                TermLength = 6,
                TermUnit = Domain.Enums.TermUnits.Months
            };
            return insuranceInformation;
        }
    }
}
