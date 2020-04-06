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
    public partial class SubmissionHistoryView : ContentPage
    {
        public SubmissionHistoryView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (LoginViewModel.UserType == StartViewModel.AdminUserType)
            {
                MyListView.ItemsSource = await SubmissionDA.GetSubmissionsByMaterial(SubmissionHistoryVM.Material);
            }
            else if (LoginViewModel.UserType == StartViewModel.CollectorUserType)
            {
                MyListView.ItemsSource = await SubmissionDA.GetSubmissionsForCollector(SubmissionHistoryVM.Material, CollectorViewModel.Collector);
            }
            else
            {
                MyListView.ItemsSource = await SubmissionDA.GetSubmissionsForRecycler(SubmissionHistoryVM.Material, RecyclerViewModel.Recycler);
            }
        }
    }
}