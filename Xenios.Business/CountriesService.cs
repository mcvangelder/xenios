using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.DataAccess;
using Xenios.Domain.Models;

namespace Xenios.Business
{
    public class CountriesService : ICountriesService
    {
        private IRepository<Country> _countryRepository;

        public CountriesService(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public List<Country> GetAllCountries()
        {
            return _countryRepository.GetAll();
        }
    }
}
