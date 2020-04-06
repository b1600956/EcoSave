using EcoSave.Model;
using EcoSave.Utilities;
using EcoSave.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcoSave.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubmissionView : ContentPage
    {
        public SubmissionView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            MyListView.ItemsSource = await SubmissionDA.GetProposedSubmissionsByCollector(CollectorViewModel.Collector);
        }

        //private void searchBar_SearchButtonPressed(object sender, EventArgs e)
        //{
        //    SearchBar searchBar = (SearchBar)sender;
        //    ObservableCollection<Submission> newSubmissionList = new ObservableCollection<Submission>();
        //    foreach (Submission submission in MyListView.ItemsSource)
        //    {
        //        if (submission.Recycler == searchBar.Text)
        //        {
        //            newSubmissionList.Add(submission);
        //        }
        //    }
        //    MyListView.ItemsSource = newSubmissionList;
        //}
    }
}