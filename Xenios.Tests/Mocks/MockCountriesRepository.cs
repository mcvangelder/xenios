using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xenios.DataAccess;
using Xenios.Domain.Models;

namespace Xenios.Mocks
{
    public class MockCountriesRepository : IRepository<Country>
    {
        public event OnCalledEvent OnGetAll;

        public List<Country> GetAll()
        {
            if (OnGetAll != null)
                OnGetAll();

            return new List<Country>();
        }
    }
}
