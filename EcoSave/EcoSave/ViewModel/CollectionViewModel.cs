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
    class CollectionViewModel : INotifyPropertyChanged
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
                    AddMaterial.Execute(selectedMaterial);
                selectedMaterial = null;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Material> availableMaterialList;

        public ObservableCollection<Material> AvailableMaterialList
        {
            get { return availableMaterialList; }
            set
            { 
                availableMaterialList = value;
                OnPropertyChanged();
            }
        }


        public ICommand AddMaterial { get; set; }
        public CollectionViewModel()
        {
            AvailableMaterialList = new ObservableCollection<Material>();
            GetAvailableMaterials();
            AddMaterial = new Command<Material>(AddMaterialExecute);
        }

        private async void GetAvailableMaterials()
        {
            ObservableCollection<Material> AllMaterials = new ObservableCollection<Material>();
            AllMaterials = await MaterialDA.GetAllMaterials();
            ObservableCollection<Material> MaterialCollectionList = new ObservableCollection<Material>();
            MaterialCollectionList = await MaterialDA.GetMaterialsById(CollectorViewModel.Collector.MaterialCollection);
            if (AllMaterials != null)
            {
                foreach (Material material in AllMaterials)
                {
                    if (!MaterialCollectionList.Contains(material))
                    {
                        AvailableMaterialList.Add(material);
                    }
                }
            }
        }
        private async void AddMaterialExecute(Material material)
        {
            Collector collector = CollectorViewModel.Collector;
            collector.MaterialCollection.Add(material.MaterialID);
            await CollectorDA.UpdateCollector(collector);
            material.CollectorList.Add(collector.Username);
            await MaterialDA.UpdateMaterial(material);
            AvailableMaterialList.Remove(material);
            await Application.Current.MainPage.DisplayAlert("Add Material into Collection", "Material "+material.MaterialName+" is successfully added.", "OK");
            //await Application.Current.MainPage.Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
