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

        private string selectedSort;

        public string SelectedSort
        {
            get
            {
                return selectedSort;
            }
            set
            {
                selectedSort = value;
                if(selectedSort == "Actual Date")
                {
                    SortSubmissionsByActualDate.Execute(null);
                }else if(selectedSort == "Status")
                {
                    SortSubmissionsByStatus.Execute(null);
                }
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

        private ObservableCollection<string> sortList;

        public ObservableCollection<string> SortList
        {
            get { return sortList; }
            set
            {
                sortList = value;
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
            SortList = new ObservableCollection<string>();
            SortSubmissionsByActualDate = new Command(SortSubmissionsByActualDateExecute);
            SortSubmissionsByStatus = new Command(SortSubmissionsByStatusExecute);
            GetSortList();
            GetSubmissionList();
        }

        private void GetSortList()
        {
            SortList.Add("Actual Date");
            SortList.Add("Status");
            //await SubmissionDA.GetAllSubmissions();
        }

        private void SortSubmissionsByActualDateExecute(object obj)
        {
            if(SubmissionList != null) {
                List<Submission> sortedList = (new List<Submission>(SubmissionList)).OrderBy(s => s.ActualDate).ToList();
                SubmissionList = new ObservableCollection<Submission>(sortedList);
            }
        }

        private void SortSubmissionsByStatusExecute(object obj)
        {
            if (SubmissionList != null)
            {
                List<Submission> sortedList= (new List<Submission>(SubmissionList)).OrderBy(s => s.Status).ToList();
                SubmissionList = new ObservableCollection<Submission>(sortedList);
            }
        }

        private void GetTotalWeightAndPoints()
        {
            TotalWeight = 0;
            TotalPoints = 0;
            if (SubmissionList != null)
            {
                foreach (Submission submission in SubmissionList)
                {
                    TotalWeight += submission.WeightInKg;
                    TotalPoints += submission.PointsAwarded;
                }
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
            if (SubmissionList.Count == 0)
            {
                SubmissionHistroryResult = "No Submission History for " + Material.MaterialName;
            }
            GetTotalWeightAndPoints();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
