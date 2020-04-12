using EcoSave.Model;
using EcoSave.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EcoSave.ViewModel
{
    class ProposeSubmissionVM : INotifyPropertyChanged
    {
        private bool canPropose;

        public bool CanPropose
        {
            get { return canPropose; }
            set
            {
                canPropose = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Collector> CollectorList { get; set; }

        private Collector selectedCollector;

        public Collector SelectedCollector
        {
            get
            {
                return selectedCollector;
            }
            set
            {
                selectedCollector = value;
                CanPropose = CheckFields();
                OnPropertyChanged();
            }
        }

        public static Material Material { get; set; }

        private DateTime proposedDate;
        public DateTime ProposedDate
        {
            get
            {
                return proposedDate;
            }
            set
            {
                proposedDate = value;
                CanPropose = CheckFields();
                OnPropertyChanged();
            }
        }

        private bool CheckFields()
        {
            return SelectedCollector != null && ProposedDate != default(DateTime) && DateTime.Compare(ProposedDate, DateTime.Today) >= 0;
        }

        public ICommand ProposeSubmission { get; set; }

        public ProposeSubmissionVM()
        {
            CollectorList = new ObservableCollection<Collector>();
            ProposeSubmission = new Command(ProposeSubmissionExecute, CanProposeM);
            if (RecyclerViewModel.Recycler != null && Material != null)
                GetCollectorList();
        }

        private async void GetCollectorList()
        {
            CollectorList = await CollectorDA.GetCollectorsByUsername(Material.CollectorList);
        }

        private async void ProposeSubmissionExecute(object obj)
        {
            Submission submission = new Submission();
            var newGuid = Guid.NewGuid();
            string id = Convert.ToBase64String(newGuid.ToByteArray());
            submission.SubmissionID = id.Remove(id.Length - 2, 2);
            submission.Recycler = RecyclerViewModel.Recycler.Username;
            submission.Collector = SelectedCollector.Username;
            submission.Status = SubmissionViewModel.StatusProposed;
            submission.Material = Material.MaterialID;
            submission.ProposedDate = ProposedDate;
            await SubmissionDA.AddSubmission(submission);
            await Application.Current.MainPage.DisplayAlert("Submit Material to Recycle", "You have successfully made an appointment with " + submission.Collector + " on " + submission.ProposedDate.ToString("d"), "OK");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private bool CanProposeM(object arg)
        {
            return CanPropose;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
