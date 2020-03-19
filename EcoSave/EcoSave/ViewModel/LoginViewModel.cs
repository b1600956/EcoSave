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
    class LoginViewModel : INotifyPropertyChanged
    {
        public static string UserType { get; set; }
        private bool canSign;
        public bool CanSign
        {
            get
            {
                return canSign;
            }
            set
            {
                canSign = value;
                OnPropertyChanged();
            }
        }

        private bool showPassword;
        public bool ShowPassword
        {
            get
            {
                return !showPassword;
            }
            set
            {
                showPassword = value;
                OnPropertyChanged();
            }
        }

        private string loginStatus;
        public string LoginStatus
        {
            get
            {
                return loginStatus;
            }
            set
            {
                loginStatus = value;
                OnPropertyChanged();
            }
        }

        public User User { get; set; }

        public string Username
        {
            get { return User.Username; }
            set 
            { 
                User.Username = value;
                CanSign = CheckUsernameAndPassword();
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return User.Password; }
            set
            {
                User.Password = value;
                CanSign = CheckUsernameAndPassword();
                OnPropertyChanged();
            }
        }
        private bool CheckUsernameAndPassword()
        {
            bool result = !string.IsNullOrWhiteSpace(User.Username) &&
                          !string.IsNullOrWhiteSpace(User.Password);
            return result;
        }
        private bool CanSignIn(object arg)
        {
            return CanSign;
        }

        public ICommand SignIn { get; set; }
        public ICommand OpenSignUpView { get; set; }

        public LoginViewModel()
        {
            SignIn = new Command(SignInExecute, CanSignIn);
            OpenSignUpView = new Command(OpenSignUpExecute);
            User = new User();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void SignInExecute(object obj)
        {
            LoginStatus = string.Empty;
            if (CheckUsernameAndPassword())
            {
                User a = new User();
                ContentPage view = new ContentPage();
                if(UserType == StartViewModel.AdminUserType)
                {
                    a = await AdminDA.GetAdmin(User);
                    view = new Views.AdminMainView();

                }else if(UserType == StartViewModel.RecyclerUserType)
                {
                    a = await RecyclerDA.GetRecycler(User);
                    view = new Views.RecyclerMainView();
                }
                else
                {
                    a = await CollectorDA.GetCollector(User);
                    view = new Views.CollectorMainView();
                }
                if (a != null)
                {
                    if (a.Password == Password)
                    {
                        if (Application.Current.Properties.ContainsKey("loggedIn"))
                        {
                            Application.Current.Properties["loggedIn"] = 1;
                        }
                        else
                        {
                            Application.Current.Properties.Add("loggedIn", 1);
                            await Application.Current.SavePropertiesAsync();
                        }
                        if(a is Recycler)
                        {
                            RecyclerViewModel.Recycler = (Recycler)a;
                        }else if(a is Collector){
                            CollectorViewModel.Collector = (Collector)a;
                        }
                        Username = string.Empty;
                        Password = string.Empty;
                        await Application.Current.MainPage.Navigation.PushAsync(view);
                    }
                    else
                    {
                        Application.Current.Properties["loggedIn"] = 0;
                        LoginStatus = "Username or password is wrong!";
                    }
                }
                else
                {
                    LoginStatus = "Username or password not found!";
                }
            }
        }
        private void OpenSignUpExecute(object obj)
        {
            Username = string.Empty;
            Password = string.Empty;
            if(UserType == StartViewModel.RecyclerUserType)
            {
                Application.Current.MainPage.Navigation.PushAsync(
                new Views.RecyclerSignUpView());
            }
            else
            {
                Application.Current.MainPage.Navigation.PushAsync(
                new Views.CollectorSignUpView());
            }
        }
    }
}
