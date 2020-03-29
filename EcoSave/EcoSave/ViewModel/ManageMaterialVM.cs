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
    class ManageMaterialVM : INotifyPropertyChanged
    {
        private Material selectedMaterial;

        public Material SelectedMaterial
        {
            get { return selectedMaterial; }
            set 
            { 
                selectedMaterial = value;
                if(selectedMaterial != null)
                    ViewMaterialDetail.Execute(selectedMaterial);
                selectedMaterial = null;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Material> MaterialList { get; set; }
        public ICommand AddNewMaterial { get; set; }
        public ICommand ViewMaterialDetail { get; set; }
        public ManageMaterialVM()
        {
            MaterialList = new ObservableCollection<Material>();
            AddNewMaterial = new Command(AddNewMaterialExecute);
            ViewMaterialDetail = new Command<Material>(ViewMaterialDetailExecute);
            GetAllMaterials();
        }

        private async void AddNewMaterialExecute(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.AddMaterialView());
        }
        private async void ViewMaterialDetailExecute(Material material)
        {
            MaterialViewModel.Material = material;
            await Application.Current.MainPage.Navigation.PushAsync(new Views.MaterialDetailView());
        }

        private async void GetAllMaterials()
        {
            MaterialList = await MaterialDA.GetAllMaterials();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
