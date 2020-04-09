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
                if(selectedCollector != null)
                    Submission.Collector = selectedCollector.Username;
                CanPropose = CheckFields();
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
                ProposeSubmission = new Command(ProposeSubmissionExecute, CanProposeM);
                UpdateSubmission = new Command(UpdateSubmissionExecute, CanUpdateM);
                CreateSubmission = new Command(CreateSubmissionExecute, CanCreateM);
                if (Material == default(Material))
                {
                    Material = new Material();
                }
                if (Submission == default(Submission))
                {
                    Submission = new Submission();
                }
                else
                {
                    InitializeFromSubmission();
                }
                if(RecyclerViewModel.Recycler != null && Material != null)
                    GetCollectorList();
        }

        private async void GetCollectorList()
        {
            CollectorList = await CollectorDA.GetCollectorsByUsername(Material.CollectorList);
        }
        private async void ProposeSubmissionExecute(object obj)
        {
            if (!string.IsNullOrWhiteSpace(Submission.Collector))
            {
                var newGuid = Guid.NewGuid();
                string id = Convert.ToBase64String(newGuid.ToByteArray());
                Submission.SubmissionID = id.Remove(id.Length - 2, 2);
                Submission.Recycler = RecyclerViewModel.Recycler.Username;
                Submission.Status = StatusProposed;
                Submission.Material = Material.MaterialID;
                await SubmissionDA.AddSubmission(Submission);
                await Application.Current.MainPage.DisplayAlert("Submit Material to Recycle", "You have successfully made an appointment with " + Submission.Collector + " on " + Submission.ProposedDate.ToString("d"), "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        private bool CanProposeM(object arg)
        {
            return CanPropose;
        }
        private async void UpdateSubmissionExecute(object obj)
        {
            UpdateStatus = string.Empty;
            Material material = new Material();
                    if (MaterialName.ToLower() != Material.MaterialName.ToLower())
                    {
                        material = await MaterialDA.GetMaterialByName(MaterialName);
                    }
            if (material != null)
                {
                    if (CollectorViewModel.Collector.MaterialCollection.Contains(material.MaterialID))
                    {
                    Material = material;
                    UpdateSubmissionForAll();
                        await Application.Current.MainPage.DisplayAlert("Record Material Submission", "You have successfully updated the submission.", "OK");
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        UpdateStatus = "You do not collect this type of material!";
                    }
                }
                else
                {
                    UpdateStatus = "Recycled Material Type not found!";
                }
        }

        private async void UpdateSubmissionForAll()
        {
            Submission.Material = Material.MaterialID;
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
            Collector = CollectorViewModel.Collector;
            if (Recycler == null)
            {
                CreateStatus = "Recycler not found!";
            }
            else
            {
                if (Material == null)
                {
                    CreateStatus = "Recycled Material Type not found!";
                }
                else
                {
                    if (Collector.MaterialCollection.Contains(Material.MaterialID))
                    {
                        Submission.Recycler = Recycler.Username;
                        Submission.ProposedDate = DateTime.Today;
                        Submission.Collector = Collector.Username;
                        var newGuid = Guid.NewGuid();
                        string id = Convert.ToBase64String(newGuid.ToByteArray());
                        Submission.SubmissionID = id.Remove(id.Length - 2, 2);
                        Submission.Status = StatusProposed;
                        Submission.Material = Material.MaterialID;
                        await SubmissionDA.AddSubmission(Submission);
                        UpdateSubmissionForAll();
                        await Application.Current.MainPage.DisplayAlert("Record Material Submission", "You have successfully recorded the submission.", "OK");
                        await Application.Current.MainPage.Navigation.PopAsync();

                    }
                    else
                    {
                        CreateStatus = "You do not collect this type of material!";
                    }

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
            if(Recycler != null)
                RecyclerUsername = Recycler.Username;
            Collector = await CollectorDA.GetCollectorByUsername(Submission.Collector);
            Material = await MaterialDA.GetMaterialById(Submission.Material);
            if (Material != null)
                MaterialName = Material.MaterialName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
