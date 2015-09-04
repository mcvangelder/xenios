using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xenios.Business.Test
{
    [TestClass]
    public class CountriesServiceTest
    {
        [TestMethod]
        public void Should_return_list_of_supported_countries()
        {
            var isNotfied = false;

            var mockCountriesRepository = new Mocks.MockCountriesRepository();
            mockCountriesRepository.OnGetAll += () => { isNotfied = true; };

            var listOfCountries = new CountriesService(mockCountriesRepository).GetAllCountries();

            Assert.IsTrue(isNotfied);
        }
    }
}
