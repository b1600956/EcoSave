using EcoSave.Model;
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
    public partial class CollectorMainView : ContentPage
    {
        public CollectorMainView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            fullName_view.Text = CollectorViewModel.Collector.FullName;
            Collector collector = await CollectorDA.GetCollectorByUsername(CollectorViewModel.Collector.Username);
            if(collector != null)
            {
                totalPoints_view.Text = collector.TotalPoints.ToString();
            }
        }
    }
}