using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Models;

namespace Xenios.DataAccess
{
    public class CountriesRepository : IRepository<Country>
    {
        private static List<Country> _countries = new List<Country>()
        {
            new Country { Name = "Canada" },
            new Country { Name = "Mexico" },
            new Country { Name = "United States" }
        };

        public List<Country> GetAll()
        {
            return _countries;
        }
    }
}
