using EcoSave.Model;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EcoSave.Utilities
{
    class MaterialDA
    {
        static readonly FirebaseClient Firebase = new FirebaseClient("https://bookapp-d865a.firebaseio.com/");

        public static async Task<ObservableCollection<Material>> GetAllMaterials()
        {
            try
            {
                List<Material> materials = (await Firebase
                    .Child("Materials")
                    .OnceAsync<Material>()).Select(item => new Material
                    {
                        MaterialID = item.Object.MaterialID,
                        MaterialName = item.Object.MaterialName,
                        Description = item.Object.Description,
                        PointsPerKg = item.Object.PointsPerKg,
                        CollectorList = item.Object.CollectorList
                    }).ToList();

                ObservableCollection<Material> materialsList = new ObservableCollection<Material>();
                foreach (Material material in materials)
                {
                    materialsList.Add(material);
                }
                return materialsList;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception MDA1", ex.Message, "OK");
                return null;
            }
        }
        public static async Task<ObservableCollection<Material>> GetMaterialsById(List<string> materialCollection)
        {
            try
            {
                var materials = await GetAllMaterials();

                ObservableCollection<Material> materialsList = new ObservableCollection<Material>();
                if (materials != null)
                {
                    foreach (Material material in materials)
                    {
                        if (materialCollection == null || !materialCollection.Contains(material.MaterialID))
                            materialsList.Add(material);
                    }
                    return materialsList;
                }
                return null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception MDA2", ex.Message, "OK");
                return null;
            }
        }
        public static async Task AddMaterial(Material material)
        {
            try
            {
                if (material != null)
                {
                    await Firebase.Child("Materials").PostAsync(material);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception MDA3", ex.Message, "OK");
            }
        }
        public static async Task UpdateMaterial(Material material)
        {
            try
            {
                if (material != null)
                {
                    var toUpdateMaterial = (await Firebase.Child("Materials")
                        .OnceAsync<Material>()).Where(a => a.Object.MaterialID == material.MaterialID).FirstOrDefault();
                    await Firebase.Child("Materials").Child(toUpdateMaterial.Key).PutAsync(material);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception MDA4", ex.Message, "OK");
            }
        }
        public static async Task DeleteMaterial(Material material)
        {
            try
            {
                if (material != null)
                {
                    var toDeleteMaterial = (await Firebase.Child("Materials")
                        .OnceAsync<Material>()).Where(a => a.Object.MaterialID == material.MaterialID).FirstOrDefault();
                    await Firebase.Child("Materials").Child(toDeleteMaterial.Key).DeleteAsync();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception MDA5", ex.Message, "OK");
            }
        }

        public static async Task<Material> GetMaterialById(string id)
        {
            try
            {
                var allMaterials = await GetAllMaterials();
                if (allMaterials != null)
                {
                    return allMaterials.Where(a => a.MaterialID == id).FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception MDA6", ex.Message, "OK");
                return null;
            }
        }

        public static async Task<Material> GetMaterialByName(string name)
        {
            try
            {
                var allMaterials = await GetAllMaterials();
                if (allMaterials != null)
                {
                    return allMaterials.Where(a => a.MaterialName.ToLower() == name.ToLower()).FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception MDA7", ex.Message, "OK");
                return null;
            }
        }

    }
}
