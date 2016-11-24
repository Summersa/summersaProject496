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
using static Android.App.ActionBar;

namespace App8
{
    [Activity(Label = "Product Descriptions", Theme = "@style/CustomActionBarTheme", WindowSoftInputMode = Android.Views.SoftInput.StateHidden)]
    public class ActivityProduct : Activity
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<Product> categoreyTable = MobileService.GetTable<Product>();
        private ListView view;
        private String categoryName;
        private String businessName;
        private String newCategoryName;
        private ActionBarHelper topbar;
        private ImageButton gAddCategory;
        List<Product> items;
        public static readonly int PickImageId = 1000;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.layoutCategory);
            view = FindViewById<ListView>(Resource.Id.viewCategories);
            gAddCategory = FindViewById<ImageButton>(Resource.Id.btnAddCategory);
            //businessName = "Windsor Plywood";
            gAddCategory.Click += AddCategory_Click;
            categoryName = Intent.GetStringExtra("categoryName").ToString();
            businessName = Intent.GetStringExtra("businessName").ToString();
            Users user = JsonConvert.DeserializeObject<Users>(Intent.GetStringExtra("user"));
            LinearLayout display = FindViewById<LinearLayout>(Resource.Id.linearLayout10);
            topbar = new ActionBarHelper(this, user);
            topbar.Start();
            ActionBarHelper.GetPicture(this, topbar.GetUser());

            IMobileServiceTableQuery<Product> query = categoreyTable.Where(Product => Product.productCategory == categoryName);
            items = await query.ToListAsync();



            productAdapter adapter = new productAdapter(this, items, businessName);
            view.Adapter = adapter;
            view.ItemClick += listView_ItemClick;
           /* if (user.role == businessName)
            {
                LinearLayout.LayoutParams size1 = new LinearLayout.LayoutParams(LayoutParams.WrapContent, 0);
                size1.Weight = 95f;
                view.LayoutParameters = size1;
            }*/
        }
        private void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(ActivityView));
            intent.PutExtra("businessName", businessName);
            intent.PutExtra("product", view.GetItemAtPosition(e.Position).ToString());
            intent.PutExtra("user", JsonConvert.SerializeObject(topbar.GetUser()));
            StartActivity(intent);
        }

        private void AddCategory_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            dialogAddProduct addProduct = new dialogAddProduct();
            addProduct.Show(transaction, "dialog fragment");
            addProduct.gAddProductEventComplete += addProduct_AddProductEventComplete;
        }

        private async void addProduct_AddProductEventComplete(object sender, AddProductEvent e)
        {
            Product item = new Product { Name = e.Name,description = e.Description, price = e.Price.ToString(), productCategory = categoryName };
            await OnlinePicture.Upload(this, e.Picture, businessName.Replace(" ", "").ToLower(), "product"+item.Name.Replace(" ", ""));
            await MobileService.GetTable<Product>().InsertAsync(item);
            items.Add(item);
            productAdapter adapter = new productAdapter(this, items, businessName);
            view.Adapter = adapter;
        }

        /*protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                
                String businessNameI = Intent.GetStringExtra("categoryName").ToString().ToLower();
                businessNameI = businessNameI.Replace(" ", "");
                String newCategoryNameI = Intent.GetStringExtra("newCategoryName").ToString();
                //await OnlinePicture.Upload(this,data.Data, Replace("  ", string.empty); businessNameI,newCategoryNameI);
                await OnlinePicture.Upload(this,data.Data, businessNameI,newCategoryNameI.Replace(" ", ""));
            }
        }*/
    }
}