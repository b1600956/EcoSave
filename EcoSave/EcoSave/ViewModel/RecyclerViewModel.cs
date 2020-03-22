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
    class RecyclerViewModel : INotifyPropertyChanged
    {
        public const string EcoLevelOne = "NewBie";
        public const string EcoLevelTwo = "Eco-Saver";
        public const string EcoLevelThree = "Eco-Hero";
        public const string EcoLevelFour = "Eco-Warrior";
        public static Recycler Recycler { get; set; }

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
            get { return Recycler.Username; }
            set
            {
                Recycler.Username = value;
                CanUpdate = CheckFields();
                CanSignUp = CanUpdate && !string.IsNullOrWhiteSpace(ConfirmPassword);
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get { return Recycler.Password; }
            set
            {
                Recycler.Password = value;
                CanUpdate = CheckFields();
                CanSignUp = CanUpdate && !string.IsNullOrWhiteSpace(ConfirmPassword);
                OnPropertyChanged();
            }
        }
        public string FullName
        {
            get { return Recycler.FullName; }
            set
            {
                Recycler.FullName = value;
                CanUpdate = CheckFields();
                CanSignUp = CanUpdate && !string.IsNullOrWhiteSpace(ConfirmPassword);
                OnPropertyChanged();
            }
        }

        private bool CheckFields()
        {
            bool result = !string.IsNullOrWhiteSpace(Recycler.Username) &&
                          !string.IsNullOrWhiteSpace(Recycler.Password) &&
                          !string.IsNullOrWhiteSpace(Recycler.FullName);
            return result;
        }

        public ICommand SignUp { get; set; }
        public ICommand OpenUpdateRecyclerView { get; set; }
        public ICommand UpdateRecycler { get; set; }
        public ICommand OpenRecycleMaterialView { get; set; }
        public ICommand OpenMaterialSubmissionView { get; set; }
        public ICommand SignOut { get; set; }
        public RecyclerViewModel()
        {
            if(Recycler == default(Recycler))
            {
                Recycler = new Recycler();
            }
            SignUp = new Command(SignUpExecute, CanSignUpM);
            OpenUpdateRecyclerView = new Command(OpenUpdateRecyclerExecute);
            UpdateRecycler = new Command(UpdateRecyclerExecute, CanUpdateM);
            OpenRecycleMaterialView = new Command(OpenRecycleMaterialExecute);
            OpenMaterialSubmissionView = new Command(OpenMaterialSubmissionExecute);
            SignOut = new Command(SignOutExecute);
        }

        private async void SignUpExecute(object obj)
        {
            SignUpStatus = string.Empty;
            if (CheckPassword())
            {
                Recycler recycler = await RecyclerDA.GetRecycler(Recycler);
                if ( recycler == null)
                {
                    Recycler.EcoLevel = EcoLevelOne;
                    Recycler.TotalPoints = 0;
                    await RecyclerDA.AddRecycler(Recycler);
                    Username = string.Empty;
                    Password = string.Empty;
                    FullName = string.Empty;
                    ConfirmPassword = string.Empty;
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

        private void OpenUpdateRecyclerExecute(object obj)
        {
            Application.Current.MainPage.Navigation.PushAsync(new Views.UpdateRecyclerView());
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

        private async void UpdateRecyclerExecute(object obj)
        {
            await RecyclerDA.UpdateRecycler(Recycler);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private bool CanUpdateM(object arg)
        {
            return CanUpdate;
        }
        private void OpenRecycleMaterialExecute(object obj)
        {
            Application.Current.MainPage.Navigation.PushAsync(new Views.RecycleMaterialView());
        }
        private void OpenMaterialSubmissionExecute(object obj)
        {
            Application.Current.MainPage.Navigation.PushAsync(new Views.MaterialSubmissionView());
        }
        private void SignOutExecute(object obj)
        {
            Application.Current.Properties["loggedIn"] = 0;
            Recycler = null;
            Application.Current.MainPage = new NavigationPage(new Views.StartView());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
