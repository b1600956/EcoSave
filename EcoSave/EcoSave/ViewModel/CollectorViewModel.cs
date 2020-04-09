using EcoSave.Model;
using EcoSave.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EcoSave.ViewModel
{
    class CollectorViewModel : INotifyPropertyChanged
    {
        public static Collector Collector { get; set; }

        private bool canSignUp;
        public bool CanSignUp
        {
            get
            {
                return canSignUp;
            }
            set
            {
                canSignUp = value;
                OnPropertyChanged();
            }
        }

        private bool canUpdate;
        public bool CanUpdate
        {
            get
            {
                return canUpdate;
            }
            set
            {
                canUpdate = value;
                OnPropertyChanged();
            }
        }

        private string signUpStatus;
        public string SignUpStatus
        {
            get
            {
                return signUpStatus;
            }
            set
            {
                signUpStatus = value;
                OnPropertyChanged();
            }
        }
        private string confirmPassword;

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                CanSignUp = CheckFields() && !string.IsNullOrWhiteSpace(ConfirmPassword);
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get { return Collector.Username; }
            set
            {
                Collector.Username = value;
                CanUpdate = CheckFields();
                CanSignUp = CanUpdate && !string.IsNullOrWhiteSpace(ConfirmPassword);
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get { return Collector.Password; }
            set
            {
                Collector.Password = value;
                CanUpdate = CheckFields();
                CanSignUp = CanUpdate && !string.IsNullOrWhiteSpace(ConfirmPassword);
                OnPropertyChanged();
            }
        }
        public string FullName
        {
            get { return Collector.FullName; }
            set
            {
                Collector.FullName = value;
                CanUpdate = CheckFields();
                CanSignUp = CanUpdate && !string.IsNullOrWhiteSpace(ConfirmPassword);
                OnPropertyChanged();
            }
        }

        public int TotalPoints
        {
            get { return Collector.TotalPoints; }
            set
            {
                Collector.TotalPoints = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get { return Collector.Address; }
            set
            {
                Collector.Address = value;
                CanUpdate = CheckFields();
                CanSignUp = CanUpdate && !string.IsNullOrWhiteSpace(ConfirmPassword);
                OnPropertyChanged();
            }
        }

        private bool CheckFields()
        {
            bool result = !string.IsNullOrWhiteSpace(Collector.Username) &&
                          !string.IsNullOrWhiteSpace(Collector.Password) &&
                          !string.IsNullOrWhiteSpace(Collector.FullName) &&
                          !string.IsNullOrWhiteSpace(Collector.Address);
            return result;
        }

        public ICommand SignUp { get; set; }
        public ICommand OpenUpdateCollectorView { get; set; }
        public ICommand UpdateCollector { get; set; }
        public ICommand OpenAddCollectionView { get; set; }
        public ICommand OpenSubmissionView { get; set; }
        public ICommand OpenMaterialSubmissionView { get; set; }
        public ICommand SignOut { get; set; }
        public CollectorViewModel()
        {
            if (Collector == default(Collector))
            {
                Collector = new Collector();
            }
            SignUp = new Command(SignUpExecute, CanSignUpM);
            OpenUpdateCollectorView = new Command(OpenUpdateCollectorExecute);
            UpdateCollector = new Command(UpdateCollectorExecute, CanUpdateM);
            OpenAddCollectionView = new Command(OpenAddCollectionExecute);
            OpenSubmissionView = new Command(OpenSubmissionExecute);
            OpenMaterialSubmissionView = new Command(OpenMaterialSubmissionExecute);
            SignOut = new Command(SignOutExecute);
        }

        private async void SignUpExecute(object obj)
        {
            SignUpStatus = string.Empty;
            if (CheckPassword())
            {
                Collector collector = await CollectorDA.GetCollector(Collector);
                if (collector == null)
                {
                    Collector.TotalPoints = 0;
                    Collector.MaterialCollection = new List<string>();
                    await CollectorDA.AddCollector(Collector);
                    Username = string.Empty;
                    Password = string.Empty;
                    FullName = string.Empty;
                    Address = string.Empty;
                    ConfirmPassword = string.Empty;
                    await Application.Current.MainPage.DisplayAlert("Sign Up", "You have successfully created a Collector account ", "OK");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    SignUpStatus = "Username already exists! Please choose another username";
                }
            }
            else
            {
                SignUpStatus = "Confirmation password is not matched with your password";
            }
        }

        private bool CheckPassword()
        {
            if (ConfirmPassword == Password)
            {
                return true;
            }
            return false;
        }


        private bool CanSignUpM(object arg)
        {
            return CanSignUp;
        }
        private void OpenUpdateCollectorExecute(object obj)
        {
            Application.Current.MainPage.Navigation.PushAsync(new Views.UpdateCollectorView());
        }
        private async void UpdateCollectorExecute(object obj)
        {
            await CollectorDA.UpdateCollector(Collector);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private bool CanUpdateM(object arg)
        {
            return CanUpdate;
        }

        private void OpenAddCollectionExecute(object obj)
        {
            Application.Current.MainPage.Navigation.PushAsync(new Views.AddCollectionView());
        }
        private void OpenSubmissionExecute(object obj)
        {
            Application.Current.MainPage.Navigation.PushAsync(new Views.SubmissionView());
        }
        private void OpenMaterialSubmissionExecute(object obj)
        {
            Application.Current.MainPage.Navigation.PushAsync(new Views.MaterialSubmissionView());
        }
        private void SignOutExecute(object obj)
        {
            Application.Current.Properties["loggedIn"] = 0;
            Collector = null;
            Application.Current.MainPage = new NavigationPage(new Views.StartView());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
