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
    public class ActivityOrder : Activity
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<Orders> orderTable = MobileService.GetTable<Orders>();
        private IMobileServiceTable<Users> userTable = MobileService.GetTable<Users>();
        private ListView view;
        private String businessName;
        private ActionBarHelper topbar;
        private ImageButton gAddCategory;
        private List<String> userNames = new List<String>();
        //private ListView listviewCategory;
        List<Orders> items;
        Users user;
        public static readonly int PickImageId = 1000;
        
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Console.WriteLine("SUCCESS WOOT");
            SetContentView(Resource.Layout.layoutCategory);
            view = FindViewById<ListView>(Resource.Id.viewCategories);
            businessName = "Windsor Plywood";
            user = JsonConvert.DeserializeObject<Users>(Intent.GetStringExtra("user"));
            LinearLayout display = FindViewById<LinearLayout>(Resource.Id.linearLayout10);
            topbar = new ActionBarHelper(this, user);
            topbar.Start();
            ActionBarHelper.GetPicture(this, topbar.GetUser());
            IMobileServiceTableQuery<Orders> query;
            if (user.role != businessName)
                 query = orderTable.Where(Orders => Orders.userId == user.Id && Orders.Completed == "1");
             else
             {
                query = orderTable.Where(Orders => Orders.Completed == "1");
                
            }
            items = await query.ToListAsync();

            foreach (Orders item in items)
            {
                IMobileServiceTableQuery<Users> queryUser = userTable.Where(Users => Users.Id == item.userId);
                List<Users> temp = await queryUser.ToListAsync();
                userNames.Add(temp[0].email);
            }
            /*EditText et = new EditText(this);
            AlertDialog.Builder ad = new AlertDialog.Builder(this);
            ad.SetTitle("Type text");
            ad.SetView(et); // <----
            ad.Show();*/

            orderAdapter adapter = new orderAdapter(this, items, userNames);
            view.Adapter = adapter;
            //IMobileServiceTableQuery<Orders> query = orderTable.Where(ProductCategory => ProductCategory.Business == businessName);
            //items = await query.ToListAsync();
            //productCategoryAdapter adapter = new productCategoryAdapter(this, items, businessName);
            //view.Adapter = adapter;
            view.ItemClick += listView_ItemClick;
            view.ItemLongClick += ListView_LongClick;
           /* if (user.role != businessName)
                query = orderTable.Where(Orders => Orders.Id == orderId && Orders.Completed == "0");
            else
            {
                query = orderTable.Where(Orders => Orders.Completed == "0");
            }*/

        }

        private async void ListView_LongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            if (user.role == businessName)
            {
                String str = view.GetItemAtPosition(e.Position).ToString();
                IMobileServiceTableQuery<Orders> query = orderTable.Where(Orders => Orders.Id == str);
                List<Orders> order = await query.ToListAsync();
                order[0].Completed = "0";
                await orderTable.UpdateAsync(order[0]);
                items.RemoveAt(e.Position);
                userNames.RemoveAt(e.Position);
                orderAdapter adapter = new orderAdapter(this, items, userNames);
                view.Adapter = adapter;
            }
        }

        private void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            String categoryName = view.GetItemAtPosition(e.Position).ToString();
            var intent = new Intent(this, typeof(ActivityOrderItems));
            intent.PutExtra("businessName", businessName);
            intent.PutExtra("orderId", view.GetItemAtPosition(e.Position).ToString());
            intent.PutExtra("user", JsonConvert.SerializeObject(topbar.GetUser()));
            StartActivity(intent);
        }


    }
}