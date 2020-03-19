using EcoSave.Model;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EcoSave.Utilities
{
    class AdminDA
    {
        static readonly FirebaseClient Firebase = new FirebaseClient("https://bookapp-d865a.firebaseio.com/");
        public static async Task<List<User>> GetAllAdmins()
        {
            try
            {
                return (await Firebase
                    .Child("Admins")
                    .OnceAsync<User>()).Select(item => new User
                    {
                        Username = item.Object.Username,
                        Password = item.Object.Password
                    }).ToList();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception ADA1", ex.Message, "OK");
                return null;
            }
        }
        public static async Task<User> GetAdmin(User user)
        {
            try
            {
                var allAdmins = await GetAllAdmins();
                if (allAdmins != null)
                {
                    return allAdmins.Where(a => a.Username == user.Username).FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Firebase Exception ADA2", ex.Message, "OK");
                return null;
            }
        }
    }
}
