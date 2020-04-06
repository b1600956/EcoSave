using EcoSave.Model;
using EcoSave.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EcoSave.ViewModel
{
    class MaterialIdToNameConverter : IValueConverter
    {
        public string materialName;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GetMaterialById(value);
            return materialName;
        }

        private async void GetMaterialById(object value)
        {
            Material material = await MaterialDA.GetMaterialById(value as string);
            materialName = material.MaterialName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
