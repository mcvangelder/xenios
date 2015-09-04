using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.DataAccess
{
    public interface IRepository<T>
    {
        List<T> GetAll();
    }
}
