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
    public partial class ProposeSubmissionView : ContentPage
    {
        public ProposeSubmissionView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            MyListView.ItemsSource = await CollectorDA.GetCollectorsByUsername(SubmissionViewModel.Material.CollectorList);
        }
    }
}