using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Business;
using Xenios.Domain.Models;

namespace Xenios.Mocks
{
    class MockCountriesService : ICountriesService
    {
        public event OnCalledEvent OnGetAllCountries;

        public List<Country> GetAllCountries()
        {
            if (OnGetAllCountries != null)
                OnGetAllCountries();

            return new List<Country>();
        }
    }
}
