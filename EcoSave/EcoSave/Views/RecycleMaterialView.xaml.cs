using EcoSave.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcoSave.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecycleMaterialView : ContentPage
    {
        public RecycleMaterialView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            MyListView.ItemsSource = await MaterialDA.GetAllMaterials();
        }
    }
}