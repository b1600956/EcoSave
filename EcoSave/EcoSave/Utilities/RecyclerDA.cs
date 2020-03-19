using EcoSave.Model;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EcoSave.Utilities
{
    class RecyclerDA
    {
        static readonly FirebaseClient Firebase = new FirebaseClient("https://bookapp-d865a.firebaseio.com/");
        public static async Task<List<Recycler>> GetAllRecyclers()
        {
            try
            {
                return (await Firebase
                    .Child("Users/Recyclers")
                    .OnceAsync<Recycler>()).Select(item => new Recycler
                    {
                        Username = item.Object.Username,
                        Password = item.Object.Password,
                        FullName = item.Object.FullName,
                        TotalPoints = item.Object.TotalPoints,
                        EcoLevel = item.Object.EcoLevel
                    }).ToList();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception RDA1", ex.Message, "OK");
                return null;
            }
        }
        public static async Task<Recycler> GetRecycler(User user)
        {
            try
            {
                var allRecyclers = await GetAllRecyclers();
                if (allRecyclers != null)
                {
                    return allRecyclers.Where(a => a.Username == user.Username).FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception RDA2", ex.Message, "OK");
                return null;
            }
        }
        public static async Task AddRecycler(Recycler recycler)
        {
            try
            {
                if (recycler != null)
                {
                    await Firebase.Child("Users/Recyclers").PostAsync(recycler);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception RDA3", ex.Message, "OK");
            }
        }
        public static async Task UpdateRecycler(Recycler recycler)
        {
            try
            {
                if (recycler != null)
                {
                    var toUpdateRecycler = (await Firebase.Child("Users/Recyclers")
                        .OnceAsync<Recycler>()).Where(a => a.Object.Username == recycler.Username).FirstOrDefault();
                    await Firebase.Child("Users/Recyclers").Child(toUpdateRecycler.Key).PutAsync(recycler);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception RDA4", ex.Message, "OK");
            }
        }
        public static async Task<Recycler> GetRecyclerByUsername(string username)
        {
            try
            {
                var allRecyclers = await GetAllRecyclers();
                if (allRecyclers != null)
                {
                    return allRecyclers.Where(a => a.Username == username).FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception RDA5", ex.Message, "OK");
                return null;
            }
        }
    }
}
