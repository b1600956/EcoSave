using EcoSave.Model;
using EcoSave.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EcoSave.ViewModel
{
    class SubmissionHistoryVM : INotifyPropertyChanged
    {
        public static Material Material { get; set; }

        private string submissionHistoryResult;

        public string SubmissionHistroryResult
        {
            get { return submissionHistoryResult; }
            set { submissionHistoryResult = value;
                OnPropertyChanged();
            }
        }

        private bool can;
        public bool Can
        {
            get
            {
                return can;
            }
            set
            {
                can = value;
                OnPropertyChanged();
            }
        }

        private int totalWeight;

        public int TotalWeight
        {
            get { return totalWeight; }
            set { totalWeight = value;
                OnPropertyChanged();
            }
        }

        private int totalPoints;

        public int TotalPoints
        {
            get { return totalPoints; }
            set { totalPoints = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Submission> submissionsList;

        public ObservableCollection<Submission> SubmissionList
        {
            get { return submissionsList; }
            set { submissionsList = value;
                OnPropertyChanged();
            }
        }


        public ICommand SortSubmissionsByActualDate { get; set; }
        public ICommand SortSubmissionsByStatus { get; set; }
        public SubmissionHistoryVM()
        {
            SubmissionList = new ObservableCollection<Submission>();
            SortSubmissionsByActualDate = new Command(SortSubmissionsByActualDateExecute, CanM);
            SortSubmissionsByStatus = new Command(SortSubmissionsByStatusExecute, CanM);
            GetSubmissionList();
            if (Can)
            {
                GetTotalWeightAndPoints();
            }  
        }

        private void SortSubmissionsByActualDateExecute(object obj)
        {
            SubmissionList = new ObservableCollection<Submission>((new List<Submission>(SubmissionList)).OrderBy(s => s.ActualDate));
        }

        private void SortSubmissionsByStatusExecute(object obj)
        {
            SubmissionList = new ObservableCollection<Submission>((new List<Submission>(SubmissionList)).OrderBy(s => s.Status));
        }

        private bool CanM(object arg)
        {
            return Can;
        }

        private void GetTotalWeightAndPoints()
        {
            TotalWeight = 0;
            TotalPoints = 0;
            foreach(Submission submission in SubmissionList)
            {
                TotalWeight += submission.WeightInKg;
                TotalPoints += submission.PointsAwarded;
            }
        }

        private async void GetSubmissionList()
        {
            if (LoginViewModel.UserType == StartViewModel.AdminUserType)
            {
                SubmissionList = await SubmissionDA.GetSubmissionsByMaterial(Material);
            }
            else if (LoginViewModel.UserType == StartViewModel.CollectorUserType)
            {
                SubmissionList = await SubmissionDA.GetSubmissionsForCollector(Material, CollectorViewModel.Collector);
            }
            else
            {
                SubmissionList = await SubmissionDA.GetSubmissionsForRecycler(Material, RecyclerViewModel.Recycler);
            }
            if(SubmissionList.Count == 0)
            {
                SubmissionHistroryResult = "No Submission History for " + Material.MaterialName;
                Can = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
