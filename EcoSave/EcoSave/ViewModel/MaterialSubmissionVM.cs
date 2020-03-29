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
    class MaterialSubmissionVM :INotifyPropertyChanged
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
                    ViewSubmissionHistory.Execute(selectedMaterial);
                selectedMaterial = null;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Material> MaterialList { get; set; }

        public ICommand ViewSubmissionHistory { get; set; }
        public MaterialSubmissionVM()
        {
            MaterialList = new ObservableCollection<Material>();
            ViewSubmissionHistory = new Command<Material>(ViewSubmissionHistoryExecute);
            GetAllMaterials();
        }

        private void ViewSubmissionHistoryExecute(Material material)
        {
            SubmissionHistoryVM.Material = material;
            Application.Current.MainPage.Navigation.PushAsync(new Views.SubmissionHistoryView());
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
