using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EcoSave.ViewModel
{
    class StartViewModel
    { 
        public const string CollectorUserType = "Collector";
        public const string RecyclerUserType = "Recycler";
        public const string AdminUserType = "Admin";
        public ICommand UserAsAdmin { get; set; }
        public ICommand UserAsRecycler { get; set; }
        public ICommand UserAsCollector { get; set; }
        public StartViewModel()
        {
            UserAsAdmin = new Command(UserAsAdminExecute);
            UserAsRecycler = new Command(UserAsRecyclerExecute);
            UserAsCollector = new Command(UserAsCollectorExecute);
        }

        private void UserAsCollectorExecute(object obj)
        {
            LoginViewModel.UserType = CollectorUserType;
            Application.Current.MainPage.Navigation.PushAsync(new Views.LoginView());
        }

        private void UserAsRecyclerExecute(object obj)
        {
            LoginViewModel.UserType = RecyclerUserType;
            Application.Current.MainPage.Navigation.PushAsync(new Views.LoginView());
        }

        private void UserAsAdminExecute(object obj)
        {
            LoginViewModel.UserType = AdminUserType;
            Application.Current.MainPage.Navigation.PushAsync(new Views.AdminLoginView());
        }
    }
}
