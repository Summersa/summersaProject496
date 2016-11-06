using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Android;
using System.ComponentModel;
using Newtonsoft.Json;

namespace App8
{
    class ActionBarHelper
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<Users> userTable = MobileService.GetTable<Users>();
        private Activity act;
        private Users user;
        
        public ActionBarHelper() { }
        public ActionBarHelper(Activity activity)
        {
            act = activity;
        }
        public ActionBarHelper(Activity activity, Users user)
        {
            act = activity;
            this.user = user;
        }
        public void Start()
        {
            act.ActionBar.SetCustomView(Resource.Layout.actionBar);
            act.ActionBar.SetDisplayShowCustomEnabled(true);
            ImageButton bt = (ImageButton)act.FindViewById(Resource.Id.profilePicture);
            ImageButton cartClick = (ImageButton)act.FindViewById(Resource.Id.btnCart);

            bt.Click += delegate {
                FragmentTransaction transaction = act.FragmentManager.BeginTransaction();
                dialogProfile profileOptions = new dialogProfile();
                profileOptions.GetUser(user);
                profileOptions.Show(transaction, "dialog fragment");
            };
            cartClick.Click += delegate
            {
                var intent = new Intent(act, typeof(ActivityCart));
                intent.PutExtra("businessName", "Windsor Plywood");
                intent.PutExtra("user", JsonConvert.SerializeObject(user));
                act.StartActivity(intent);
            };

        }

        public Users GetUser()
        {
            return user;
        }
        public void SetActivity(Activity activity)
        {
            act=activity;
        }
        public static void GetPicture(Activity activity,Users user)
        {

           ImageButton profileImage = (ImageButton)activity.FindViewById(Resource.Id.profilePicture);
            try
            {
                profileImage.SetImageBitmap(OnlinePicture.Stream(String.Format("https://storagedatabase666.blob.core.windows.net/profilepictures/{0}", user.username)));
            }
            catch{}
        }
    }
}