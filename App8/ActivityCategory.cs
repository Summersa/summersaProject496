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
        private ImageButton gAddCategory;
        //private ListView listviewCategory;
        List<ProductCategory> items;
        public static readonly int PickImageId = 1000;
        
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.layoutCategory);
            view = FindViewById<ListView>(Resource.Id.viewCategories);
            //listviewCategory = FindViewById<ListView>(Resource.Id.viewCategories);
            gAddCategory = FindViewById<ImageButton>(Resource.Id.btnAddCategory);
            //businessName = "Windsor Plywood";
            gAddCategory.Click += AddCategory_Click;
            businessName = "Windsor Plywood";//Intent.GetStringExtra("businessNames").ToString();
            Users user = JsonConvert.DeserializeObject<Users>(Intent.GetStringExtra("user"));
            LinearLayout display = FindViewById<LinearLayout>(Resource.Id.linearLayout10);
            if(user.role == businessName) display.Visibility = ViewStates.Visible;
            topbar = new ActionBarHelper(this, user);
            topbar.Start();
            ActionBarHelper.GetPicture(this, topbar.GetUser());

            IMobileServiceTableQuery<ProductCategory> query = categoreyTable.Where(ProductCategory => ProductCategory.Business == businessName);
            items = await query.ToListAsync();


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


            productCategoryAdapter adapter = new productCategoryAdapter(this, items, businessName);
            view.Adapter = adapter;
            view.ItemClick += listView_ItemClick;
        }

        private void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            String categoryName = view.GetItemAtPosition(e.Position).ToString();
            var intent = new Intent(this, typeof(ActivityProduct));
            intent.PutExtra("categoryName", categoryName);
            intent.PutExtra("businessName", businessName);
            intent.PutExtra("user", JsonConvert.SerializeObject(topbar.GetUser()));
            StartActivity(intent);
        }

        private void AddCategory_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            dialogAddCategory addCategory = new dialogAddCategory();
            addCategory.Show(transaction, "dialog fragment");
            addCategory.gAddCategoryEventComplete += addCategory_AddCategoryEventComplete;
        }

        private async void addCategory_AddCategoryEventComplete(object sender, AddCategoryEvent e)
        {
            ProductCategory item = new ProductCategory { Name = e.Description, Business = businessName };
            await OnlinePicture.Upload(this, e.Picture, businessName.Replace(" ", "").ToLower(), "category"+item.Name.Replace(" ", ""));
            await MobileService.GetTable<ProductCategory>().InsertAsync(item);
            items.Add(item);
            productCategoryAdapter adapter = new productCategoryAdapter(this, items, businessName);
            view.Adapter = adapter;
        }

        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                
                String businessNameI = Intent.GetStringExtra("businessNames").ToString().ToLower();
                businessNameI = businessNameI.Replace(" ", "");
                String newCategoryNameI = Intent.GetStringExtra("newCategoryName").ToString();
                //await OnlinePicture.Upload(this,data.Data, Replace("  ", string.empty); businessNameI,newCategoryNameI);
                await OnlinePicture.Upload(this,data.Data, businessNameI,newCategoryNameI.Replace(" ", ""));
            }
        }
    }
}