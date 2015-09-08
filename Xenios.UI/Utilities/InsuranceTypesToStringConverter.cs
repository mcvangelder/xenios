using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Xenios.Domain.Enums;

namespace Xenios.UI.Utilities
{
    public class InsuranceTypesListConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return CombineIntoSingle((ICollection<Domain.Enums.InsuranceTypes>)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return SplitIntoList((Domain.Enums.InsuranceTypes)value);
        }

        private static InsuranceTypes CombineIntoSingle(ICollection<InsuranceTypes> value)
        {
            InsuranceTypes temp = InsuranceTypes.Unspecified;
            foreach (var type in value)
                temp |= type;
            return temp;
        }

        private ICollection<InsuranceTypes> SplitIntoList(Domain.Enums.InsuranceTypes value)
        {
            var split = new List<InsuranceTypes>();
            foreach (var item in EnumHelper.GetAllAsCollection<List<InsuranceTypes>, InsuranceTypes>())
            {
                if (item == InsuranceTypes.Unspecified)
                    continue;

                if ((value & item) == item)
                    split.Add(item);
            }

            return split;
        }
    }
}
