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
    [Activity(Label = "Product Descriptions", Theme = "@style/CustomActionBarTheme")]
    public class ActivityOrderItems : Activity
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<Orders> orderTable = MobileService.GetTable<Orders>();
        private ListView view;
        private String businessName;
        private ActionBarHelper topbar;
        private ImageButton gAddCategory;
        //private ListView listviewCategory;
        List<Orders> items;
        public static readonly int PickImageId = 1000;
        
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Console.WriteLine("SUCCESS WOOT");
            SetContentView(Resource.Layout.layoutCategory);
            view = FindViewById<ListView>(Resource.Id.viewCategories);
            businessName = "Windsor Plywood";
            String orderId = Intent.GetStringExtra("orderId");
            Users user = JsonConvert.DeserializeObject<Users>(Intent.GetStringExtra("user"));
            LinearLayout display = FindViewById<LinearLayout>(Resource.Id.linearLayout10);
            topbar = new ActionBarHelper(this, user);
            topbar.Start();
            topbar.textViewChange("Order Items");
            ActionBarHelper.GetPicture(this, topbar.GetUser());
            Console.WriteLine("its still working");
            IMobileServiceTableQuery<Orders> query = orderTable.Where(Orders => Orders.Id == orderId);

            items = await query.ToListAsync();
            //items[0].Products.TrimEnd('.');
            String[] products, quantities, prices;
            //Console.WriteLine("Produc{0}", items[0].Id);
            Console.WriteLine("Products{0}", items[0].Products);
            products = items[0].Products.Split('.');
            quantities = items[0].Quantities.Split('.');
            prices = items[0].Prices.Split('.');
            
            //products[products.Length] = null;
            //quantities[products.Length-1] = null;
            //prices[products.Length-1] = null;
            orderItemsAdapter adapter = new orderItemsAdapter(this, products,quantities, prices, businessName);
            view.Adapter = adapter;
            //IMobileServiceTableQuery<Orders> query = orderTable.Where(ProductCategory => ProductCategory.Business == businessName);
            //items = await query.ToListAsync();
            //productCategoryAdapter adapter = new productCategoryAdapter(this, items, businessName);
            //view.Adapter = adapter;
            view.ItemClick += listView_ItemClick;


        }

        private void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            String categoryName = view.GetItemAtPosition(e.Position).ToString();
            var intent = new Intent(this, typeof(ActivityProduct));
            intent.PutExtra("businessName", businessName);
            intent.PutExtra("orderId", view.GetItemAtPosition(e.Position).ToString());
            intent.PutExtra("user", JsonConvert.SerializeObject(topbar.GetUser()));
            StartActivity(intent);
        }


    }
}