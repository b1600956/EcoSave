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
    class MaterialViewModel : INotifyPropertyChanged
    {
        public static Material Material { get; set; }

        public string MaterialName
        {
            get { return Material.MaterialName; }
            set
            {
                Material.MaterialName = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return Material.Description; }
            set
            {
                Material.Description = value;
                OnPropertyChanged();
            }
        }
        public int PointsPerKg
        {
            get { return Material.PointsPerKg; }
            set
            {
                Material.PointsPerKg = value;
                OnPropertyChanged();
            }
        }

        public ICommand UploadMaterial { get; set; }
        public ICommand UpdateMaterial { get; set; }
        public ICommand DeleteMaterial { get; set; }

        public MaterialViewModel()
        {
            if(Material == default(Material)) //check
            {
                Material = new Material();
            }
            UploadMaterial = new Command(UploadMaterialExecute);
            UpdateMaterial = new Command(UpdateMaterialExecute);
            DeleteMaterial = new Command(DeleteMaterialExecute);
        }

        private async void UploadMaterialExecute(object obj)
        {
            Material.MaterialID = Guid.NewGuid().ToString();
            await MaterialDA.AddMaterial(Material);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void UpdateMaterialExecute(object obj)
        {
            await MaterialDA.UpdateMaterial(Material);
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        private async void DeleteMaterialExecute(object obj)
        {
            await MaterialDA.DeleteMaterial(Material);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
