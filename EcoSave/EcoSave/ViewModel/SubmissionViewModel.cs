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
    class SubmissionViewModel : INotifyPropertyChanged
    {
        public const string StatusProposed = "Proposed";
        public const string StatusSubmitted = "Submitted";

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

        private bool canUpdate;

        public bool CanUpdate
        {
            get { return canUpdate; }
            set
            {
                canUpdate = value;
                OnPropertyChanged();
            }
        }

        private bool canCreate;

        public bool CanCreate
        {
            get { return canCreate; }
            set
            {
                canCreate = value;
                OnPropertyChanged();
            }
        }

        private string updateStatus;
        public string UpdateStatus
        {
            get
            {
                return updateStatus;
            }
            set
            {
                updateStatus = value;
                OnPropertyChanged();
            }
        }

        private string createStatus;
        public string CreateStatus
        {
            get
            {
                return createStatus;
            }
            set
            {
                createStatus = value;
                OnPropertyChanged();
            }
        }

        public static Material Material { get; set; }
        public static Submission Submission { get; set; }
        public Recycler Recycler { get; set; }
        public Collector Collector { get; set; }
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
                Submission.Collector = selectedCollector.Username;
                CanPropose = CheckFields();
                selectedCollector = null;
                OnPropertyChanged();
            }
        }
        public DateTime ProposedDate
        {
            get
            {
                return Submission.ProposedDate;
            }
            set
            {
                Submission.ProposedDate = value;
                CanPropose = CheckFields();
                OnPropertyChanged();
            }
        }

        public int WeightInKg
        {
            get
            {
                return Submission.WeightInKg;
            }
            set
            {
                Submission.WeightInKg = value;
                CanUpdate = WeightInKg > 0 && !string.IsNullOrWhiteSpace(MaterialName);
                CanCreate = CanUpdate && !string.IsNullOrWhiteSpace(RecyclerUsername);
                OnPropertyChanged();
            }
        }

        private string materialName;

        public string MaterialName
        {
            get 
            {
                if (Material != null)
                    return Material.MaterialName;
                return materialName; 
            }
            set 
            { 
                materialName = value;
                CanUpdate = WeightInKg > 0 && !string.IsNullOrWhiteSpace(MaterialName);
                CanCreate = CanUpdate && !string.IsNullOrWhiteSpace(RecyclerUsername);
                OnPropertyChanged();
            }
        }

        private string recyclerUsername;

        public string RecyclerUsername
        {
            get
            {
                if (Recycler != null)
                    return Recycler.Username;
                return recyclerUsername;
            }
            set
            {
                recyclerUsername = value;
                CanUpdate = WeightInKg > 0 && !string.IsNullOrWhiteSpace(MaterialName);
                CanCreate = CanUpdate && !string.IsNullOrWhiteSpace(RecyclerUsername);
                OnPropertyChanged();
            }
        }


        private bool CheckFields()
        {
            return SelectedCollector != null && ProposedDate != default(DateTime) && DateTime.Compare(ProposedDate,DateTime.Today) >= 0;
        }

        public ICommand ProposeSubmission { get; set; }
        public ICommand UpdateSubmission { get; set; }
        public ICommand CreateSubmission { get; set; }
        public SubmissionViewModel()
        {
            CollectorList = new ObservableCollection<Collector>();
            Recycler = new Recycler();
            Collector = new Collector();
            GetCollectorList();
            ProposeSubmission = new Command(ProposeSubmissionExecute, CanProposeM);
            UpdateSubmission = new Command(UpdateSubmissionExecute, CanUpdateM);
            CreateSubmission = new Command(CreateSubmissionExecute, CanCreateM);
            if(Material == default(Material))
            {
                Material = new Material();
            }
            if(Submission == default(Submission))
            {
                Submission = new Submission();
            }
            else
            {
                InitializeFromSubmission();
            }
        }

        private async void GetCollectorList()
        {
            CollectorList = await CollectorDA.GetCollectorsByUsername(Material.CollectorList);
        }
        private async void ProposeSubmissionExecute(object obj)
        {
            Submission.Recycler = RecyclerViewModel.Recycler.Username;
            Submission.Status = StatusProposed;
            Submission.Material = Material.MaterialID;
            await SubmissionDA.AddSubmission(Submission);
            await Application.Current.MainPage.DisplayAlert("Submit Material to Recycle", "You have successfully made an appointment with " + Submission.Collector + " on " + Submission.ProposedDate.ToString("d"), "OK");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private bool CanProposeM(object arg)
        {
            return CanPropose;
        }
        private async void UpdateSubmissionExecute(object obj)
        {
            UpdateStatus = string.Empty;
            if (MaterialName.ToLower() != Material.MaterialName.ToLower())
            {
                Material = await MaterialDA.GetMaterialByName(MaterialName);
            }
            if (Material != null)
            {
                UpdateSubmissionForAll();
                await Application.Current.MainPage.DisplayAlert("Record Material Submission", "You have successfully updated the submission.", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                UpdateStatus = "Recycled Material Type not found!";
            }
        }

        private async void UpdateSubmissionForAll()
        {
            Submission.Status = StatusSubmitted;
            Submission.PointsAwarded = WeightInKg * Material.PointsPerKg;
            Submission.ActualDate = DateTime.Today;
            Recycler.TotalPoints += Submission.PointsAwarded;
            UpdateRecyclerEcoLevel();
            await RecyclerDA.UpdateRecycler(Recycler);
            Collector.TotalPoints += Submission.PointsAwarded;
            await CollectorDA.UpdateCollector(Collector);
            await SubmissionDA.UpdateSubmission(Submission);
        }

        private void UpdateRecyclerEcoLevel()
        {
            if(Recycler.TotalPoints >= 1000)
            {
                Recycler.EcoLevel = RecyclerViewModel.EcoLevelFour;
            }
            else if(Recycler.TotalPoints >= 500)
            {
                Recycler.EcoLevel = RecyclerViewModel.EcoLevelThree;
            }
            else if(Recycler.TotalPoints >= 100)
            {
                Recycler.EcoLevel = RecyclerViewModel.EcoLevelTwo;
            }
        }

        private bool CanUpdateM(object arg)
        {
            return CanUpdate;
        }

        private async void CreateSubmissionExecute(object obj)
        {
            CreateStatus = string.Empty;
            Recycler = await RecyclerDA.GetRecyclerByUsername(RecyclerUsername);
            Material = await MaterialDA.GetMaterialByName(MaterialName);
            if (Material == null)
            {
                CreateStatus = "Recycled Material Type not found!";
            }
            else
            {
                if(Recycler == null)
                {
                    CreateStatus = "Recycler not found!";
                }
                else
                {
                    Submission.Recycler = Recycler.Username;
                    Submission.Material = Material.MaterialName;
                    UpdateSubmissionForAll();
                    await Application.Current.MainPage.DisplayAlert("Record Material Submission", "You have successfully recorded the submission.", "OK");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
        }

        private bool CanCreateM(object arg)
        {
            return CanCreate;
        }
        private async void InitializeFromSubmission()
        {
            Recycler = await RecyclerDA.GetRecyclerByUsername(Submission.Recycler);
            Collector = await CollectorDA.GetCollectorByUsername(Submission.Collector);
            Material = await MaterialDA.GetMaterialById(Submission.Material);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
