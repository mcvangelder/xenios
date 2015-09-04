using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Xenios.Domain.Models;

namespace Xenios.DataAccess
{
    [TestClass]
    public class CountriesRepositoryTest
    {
        [TestMethod]
        public void Should_return_supported_countries()
        {
            var listOfSupportCountries = new List<Country>() {
                new Country { Name = "Canada" }, new Country { Name = "Mexico"}, new Country { Name = "United States" }
            };

            var listOfCountries = new CountriesRepository().GetAll();

            foreach(var cntry in listOfSupportCountries)
            {
                var exists = listOfCountries.Exists( c => c.Name == cntry.Name);
                Assert.IsTrue(exists);
            }
        }
    }
}
