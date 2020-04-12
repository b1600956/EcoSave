using EcoSave.Utilities;
using EcoSave.ViewModel;
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
    public partial class AddCollectionView : ContentPage
    {
        public AddCollectionView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            MyListView.ItemsSource = await MaterialDA.GetAvailableMaterialsById(CollectorViewModel.Collector.MaterialCollection);
            addedMaterialList.ItemsSource = await MaterialDA.GetMaterialsById(CollectorViewModel.Collector.MaterialCollection);
        }
    }
}