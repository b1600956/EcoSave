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
    class RecycleMaterialVM : INotifyPropertyChanged
    {
        private Material selectedMaterial;

        public Material SelectedMaterial
        {
            get
            {
                return selectedMaterial;
            }
            set
            {
                selectedMaterial = value;
                if(selectedMaterial != null)
                    RecycleMaterial.Execute(selectedMaterial);
                selectedMaterial = null;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Material> MaterialList { get; set; }
        public ICommand RecycleMaterial { get; set; }
        public RecycleMaterialVM()
        {
            MaterialList = new ObservableCollection<Material>();
            GetAllMaterials();
            RecycleMaterial = new Command<Material>(RecycleMaterialExecute);
        }

        private async void GetAllMaterials()
        {
            MaterialList = await MaterialDA.GetAllMaterials();
        }
        private void RecycleMaterialExecute(Material material)
        {
            SubmissionViewModel.Material = material;
            Application.Current.MainPage.Navigation.PushAsync(new Views.ProposeSubmissionView());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
