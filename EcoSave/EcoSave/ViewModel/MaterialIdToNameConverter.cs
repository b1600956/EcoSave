using EcoSave.Model;
using EcoSave.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EcoSave.ViewModel
{
    class MaterialIdToNameConverter : IValueConverter
    {
        public static List<Material> AllMaterials { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Material material = AllMaterials.Where(a => a.MaterialID == value as string).FirstOrDefault();
            if(material != null)
            {
                return material.MaterialName;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
