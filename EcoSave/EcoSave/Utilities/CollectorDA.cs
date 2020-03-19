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
    class CollectorDA
    {
        static readonly FirebaseClient Firebase = new FirebaseClient("https://bookapp-d865a.firebaseio.com/");
        public static async Task<List<Collector>> GetAllCollectors()
        {
            try
            {
                return (await Firebase
                    .Child("Users/Collectors")
                    .OnceAsync<Collector>()).Select(item => new Collector
                    {
                        Username = item.Object.Username,
                        Password = item.Object.Password,
                        FullName = item.Object.FullName,
                        TotalPoints = item.Object.TotalPoints,
                        Address = item.Object.Address,
                        MaterialCollection = item.Object.MaterialCollection
                    }).ToList();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception CDA1", ex.Message, "OK");
                return null;
            }
        }
        public static async Task<Collector> GetCollector(User user)
        {
            try
            {
                var allCollectors = await GetAllCollectors();
                if (allCollectors != null)
                {
                    return allCollectors.Where(a => a.Username == user.Username).FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception CDA2", ex.Message, "OK");
                return null;
            }
        }

        public static async Task<ObservableCollection<Collector>> GetCollectorsByUsername(List<string> collectors)
        {
            try
            {
                var allCollectors = await GetAllCollectors();
                ObservableCollection<Collector> collectorList = new ObservableCollection<Collector>();
                if (allCollectors != null)
                {
                    
                    foreach (Collector collector in allCollectors)
                    {
                        if (collectors.Contains(collector.Username))
                            collectorList.Add(collector);
                    }
                    return collectorList;
                }
                return null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception CDA3", ex.Message, "OK");
                return null;
            }
        }
        public static async Task AddCollector(Collector collector)
        {
            try
            {
                if (collector != null)
                {
                    await Firebase.Child("Users/Collectors").PostAsync(collector);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception CDA4", ex.Message, "OK");
            }
        }
        public static async Task UpdateCollector(Collector collector)
        {
            try
            {
                if (collector != null)
                {
                    var toUpdateCollector = (await Firebase.Child("Users/Collectors")
                        .OnceAsync<Collector>()).Where(a => a.Object.Username == collector.Username).FirstOrDefault();
                    await Firebase.Child("Users/Collectors").Child(toUpdateCollector.Key).PutAsync(collector);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception CDA5", ex.Message, "OK");
            }
        }
        public static async Task<Collector> GetCollectorByUsername(string username)
        {
            try
            {
                var allCollectors = await GetAllCollectors();
                if (allCollectors != null)
                {
                    return allCollectors.Where(a => a.Username == username).FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception CDA6", ex.Message, "OK");
                return null;
            }
        }
    }
}
