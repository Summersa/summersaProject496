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
    public class ActivityCategory : Activity
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<ProductCategory> categoreyTable = MobileService.GetTable<ProductCategory>();
        private ListView view;
        private String businessName;
        private String newCategoryName;
        private ActionBarHelper topbar;
        public static readonly int PickImageId = 1000;
        
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.layoutCategory);
            view = FindViewById<ListView>(Resource.Id.viewCategories);
            businessName = Intent.GetStringExtra("businessNames").ToString();
            Users user = JsonConvert.DeserializeObject<Users>(Intent.GetStringExtra("user"));
            topbar = new ActionBarHelper(this, user);
            topbar.Start();
            ActionBarHelper.GetPicture(this, topbar.GetUser());
            //Console.WriteLine("1111111111111111111111111111111111");
            IMobileServiceTableQuery<ProductCategory> query = categoreyTable.Where(ProductCategory => ProductCategory.Business == businessName);
            List<ProductCategory> items = await query.ToListAsync();


            if (items.Count == 0)
            {
                newCategoryName = "Decking";
                ProductCategory item = new ProductCategory { Name = newCategoryName, Business = businessName};
                await MobileService.GetTable<ProductCategory>().InsertAsync(item);
                Intent = new Intent();
                Intent.PutExtra("businessNames", businessName);
                Intent.PutExtra("newCategoryName", newCategoryName);
                Intent.SetType("image/*");
                Intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
            }


            productCategoryAdapter adapter = new productCategoryAdapter(this, items);
            view.Adapter = adapter;

        }
        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                
                String businessNameI = Intent.GetStringExtra("businessNames").ToString().ToLower();
                businessNameI = businessNameI.Replace(" ", "");
                String newCategoryNameI = Intent.GetStringExtra("newCategoryName").ToString();
                Console.WriteLine("String to compare::::::: {0}", businessNameI);
                //await OnlinePicture.Upload(this,data.Data, Replace("  ", string.empty); businessNameI,newCategoryNameI);
                await OnlinePicture.Upload(this,data.Data, businessNameI,newCategoryNameI.Replace(" ", ""));
            }
        }
    }
}