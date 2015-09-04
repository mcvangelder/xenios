using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenios.Domain.Models;

namespace Xenios.Business
{
    public interface ICountriesService
    {
        List<Country> GetAllCountries();
    }
}
