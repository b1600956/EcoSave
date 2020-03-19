using EcoSave.Model;
using EcoSave.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EcoSave.ViewModel
{
    class MainViewModel
    {
        public ICommand OpenManageMaterialView { get; set; }
        public ICommand OpenMaterialSubmissionView { get; set; }
        public ICommand SignOut { get; set; }
        public MainViewModel()
        {
            OpenManageMaterialView = new Command(OpenManageMaterialExecute);
            OpenMaterialSubmissionView = new Command(OpenMaterialSubmissionExecute);
            SignOut = new Command(SignOutExecute);
        }

        private async void SignOutExecute(object obj)
        {
            Application.Current.Properties["loggedIn"] = 0;
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void OpenMaterialSubmissionExecute(object obj)
        {
            Application.Current.MainPage.Navigation.PushAsync(new Views.MaterialSubmissionView());
        }

        private void OpenManageMaterialExecute(object obj)
        {
            Application.Current.MainPage.Navigation.PushAsync(new Views.ManageMaterialView());
        }
    }
}
