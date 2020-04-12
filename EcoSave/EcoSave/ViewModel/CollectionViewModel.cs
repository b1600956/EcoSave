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

        private ObservableCollection<Material> materialList;

        public ObservableCollection<Material> MaterialList
        {
            get { return materialList; }
            set
            {
                materialList = value;
                OnPropertyChanged();
            }
        }

        private int materialCount;

        public int MaterialCount
        {
            get { return materialCount; }
            set { 
                materialCount = value;
                OnPropertyChanged();
            }
        }



        public ICommand AddMaterial { get; set; }
        public CollectionViewModel()
        {
            AvailableMaterialList = new ObservableCollection<Material>();
            MaterialList = new ObservableCollection<Material>();
            GetAvailableMaterials();
            AddMaterial = new Command<Material>(AddMaterialExecute);
        }

        private async void GetAvailableMaterials()
        {
            MaterialList = await MaterialDA.GetMaterialsById(CollectorViewModel.Collector.MaterialCollection);
            AvailableMaterialList = await MaterialDA.GetAvailableMaterialsById(CollectorViewModel.Collector.MaterialCollection);
            if(MaterialList != null)
            {
                MaterialCount = MaterialList.Count;
            }
            else
            {
                MaterialCount = 0;
                MaterialList = new ObservableCollection<Material>();
            }
            
        }
        private async void AddMaterialExecute(Material material)
        {
            Collector collector = CollectorViewModel.Collector;
            if (collector.MaterialCollection == null)
                collector.MaterialCollection = new List<string>();
            collector.MaterialCollection.Add(material.MaterialID);
            await CollectorDA.UpdateCollector(collector);
            if (material.CollectorList == null)
                material.CollectorList = new List<string>();
            material.CollectorList.Add(collector.Username);
            await MaterialDA.UpdateMaterial(material);
            AvailableMaterialList.Remove(material);
            MaterialList.Add(material);
            MaterialCount = MaterialList.Count;
            //await Application.Current.MainPage.DisplayAlert("Add Material into Collection", "Material "+material.MaterialName+" is successfully added.", "OK");
            //await Application.Current.MainPage.Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
