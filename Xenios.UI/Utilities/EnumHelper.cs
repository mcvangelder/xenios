using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.UI.Utilities
{
    public static class EnumHelper
    {
        public static TReturn GetAllAsCollection<TReturn,T>() where TReturn: ICollection<T>
        {
            var allEnums = Enum.GetValues(typeof(T));
            var list = Activator.CreateInstance<TReturn>();
            for (int i = 0; i < allEnums.Length; i++)
                list.Add((T)allEnums.GetValue(i));

            return list;
        }
    }
}
