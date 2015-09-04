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
            new Country { Name = "Canada", ImageUri = "pack://application:,,,/Xenios.UI;component/Resources/canada-flag.png" },
            new Country { Name = "Mexico", ImageUri = "pack://application:,,,/Xenios.UI;component/Resources/mexico-flag.png" },
            new Country { Name = "United States", ImageUri = "pack://application:,,,/Xenios.UI;component/Resources/us-flag.png" }
        };

        public List<Country> GetAll()
        {
            return _countries;
        }
    }
}
