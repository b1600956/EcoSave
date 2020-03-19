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
    class SelectSubmissionVM : INotifyPropertyChanged
    {
        private string searchResult;
        public string SearchResult
        {
            get
            {
                return searchResult;
            }
            set
            {
                searchResult = value;
                OnPropertyChanged();
            }
        }

        private Submission selectedSubmission;

        public Submission SelectedSubmission
        {
            get
            {
                return selectedSubmission;
            }
            set
            {
                selectedSubmission = value;
                OpenRecordMaterialView.Execute(selectedSubmission);
                selectedSubmission = null;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Submission> submissionList;

        public ObservableCollection<Submission> SubmissionList
        {
            get { return submissionList; }
            set 
            { 
                submissionList = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchSubmission { get; set; }
        public ICommand OpenRecordMaterialView { get; set; }
        public ICommand CreateSubmission { get; set; }
        public SelectSubmissionVM()
        {
            SubmissionList = new ObservableCollection<Submission>();
            GetSubmissionList();
            SearchSubmission = new Command<string>(SearchSubmissionExecute);
            OpenRecordMaterialView = new Command<Submission>(OpenRecordMaterialExecute);
            CreateSubmission = new Command(CreateSubmissionExecute);
        }

        private async void GetSubmissionList()
        {
            SubmissionList = await SubmissionDA.GetProposedSubmissionsByCollector(CollectorViewModel.Collector);
        }

        private void SearchSubmissionExecute(string query)
        {
            SearchResult = string.Empty;
            ObservableCollection<Submission> newSubmissionList = new ObservableCollection<Submission>();
            foreach(Submission submission in SubmissionList)
            {
                if(submission.Recycler == query)
                {
                    newSubmissionList.Add(submission);
                }
            }
            SubmissionList = newSubmissionList;
            if(SubmissionList.Count == 0)
            {
                SearchResult = "No Submissions found for "+query;
            }
        }

        private void OpenRecordMaterialExecute(Submission submission)
        {
            SubmissionViewModel.Submission = submission;
            Application.Current.MainPage.Navigation.PushAsync(new Views.RecordMaterialView());
        }

        private void CreateSubmissionExecute(object obj)
        {
            Application.Current.MainPage.Navigation.PushAsync(new Views.RecordMaterialView());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
