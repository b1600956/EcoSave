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
    public partial class RecyclerMainView : ContentPage
    {
        public RecyclerMainView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            fullName_view.Text = RecyclerViewModel.Recycler.FullName;
            totalPoints_view.Text = RecyclerViewModel.Recycler.TotalPoints.ToString();
        }
    }
}