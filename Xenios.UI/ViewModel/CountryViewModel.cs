using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Xenios.Domain.Models;

namespace Xenios.UI.ViewModel
{
    public class CountryViewModel
    {
        public CountryViewModel(Country country)
        {
            Image = new BitmapImage(new Uri(country.ImageUri));
            Name = country.Name;
        }

        public BitmapImage Image { get; set; }
        public String Name { get; set; }
    }
}
