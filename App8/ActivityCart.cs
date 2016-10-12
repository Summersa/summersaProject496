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
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace App8
{
    [Activity(Label = "Product Descriptions", Theme = "@style/CustomActionBarTheme")]
    public class ActivityCart : Activity
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<Cart> cartTable = MobileService.GetTable<Cart>();
        private ListView view;
        private String businessName;
        private String newCategoryName;
        private ActionBarHelper topbar;
        private ImageButton gAddCategory;
        //private ListView listviewCategory;
        List<Cart> items;
        public static readonly int PickImageId = 1000;
        
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.layoutCart);
            view = FindViewById<ListView>(Resource.Id.viewCategories);
            businessName = Intent.GetStringExtra("businessNames").ToString();
            Users user = JsonConvert.DeserializeObject<Users>(Intent.GetStringExtra("user"));
            topbar = new ActionBarHelper(this, user);
            topbar.Start();
            ActionBarHelper.GetPicture(this, topbar.GetUser());

            IMobileServiceTableQuery<Cart> query = cartTable.Where(Cart => Cart.userId == user.Id);
            items = await query.ToListAsync(); 
            cartAdapter adapter = new cartAdapter(this, items, businessName);
            view.Adapter = adapter;
 
        }

    }
}