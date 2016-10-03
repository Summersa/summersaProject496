using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using Org.Apache.Http.Client;
using Newtonsoft.Json;
using System.ComponentModel;

namespace App8
{
    [Activity(Label = "Business",Theme = "@style/CustomActionBarTheme")]
    public class ActivityBusiness : Activity
    {
       // int count = 1;
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<Businesses> businessTable = MobileService.GetTable<Businesses>();
        private ListView companyListView;
        public List<String> businessNames;
        private ActionBarHelper topbar;
       // private List<TodoItem> companies;
        //  public ProgressBar progressBar = FindViewById<ProgressBar>(Resource.Id.progress);
        private ProgressBar progress;

        protected override async void OnCreate(Bundle bundle)
        {
            SetContentView(Resource.Layout.layoutBusiness);
            CurrentPlatform.Init();
            //ActionBar.SetDisplayHomeAsUpEnabled(false);
            //ActionBar.SetDisplayShowTitleEnabled(false);
            //  ActionBar.SetCustomView(Resource.Layout.layoutName);
            //ActionBar.SetDisplayShowCustomEnabled(true);




            //progress = ProgressDialog.Show(this, "", "Loading...");
            //progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            //Console.WriteLine("444444444444444444444444444");
            base.OnCreate(bundle);
            Users user = JsonConvert.DeserializeObject<Users>(Intent.GetStringExtra("User"));
            topbar = new ActionBarHelper(this, user);
            topbar.Start();
            ActionBarHelper.GetPicture(this, user);


            //background.DoWork += (o, e) =>
            //{
                
            //};
            //background.RunWorkerAsync();
            // Set our view from the "main" layout resource
            //Button button = FindViewById<Button>(Resource.Id.MyButton);
            progress = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            companyListView = FindViewById<ListView>(Resource.Id.companyListView);

            //ADD ITEM TO DATABASE
            //TodoItem item = new TodoItem { Text = "Awesome item" };
            //await MobileService.GetTable<TodoItem>().InsertAsync(item);
            //Console.WriteLine("66666666666666666666666");


            //------------------------------    GOT ITEMS IN DATABASE THROUGH QUERY
            IMobileServiceTableQuery<Businesses> query = businessTable.Take(50);
            List<Businesses> items = await query.ToListAsync();
            if(items.Count==0)
            {

                Businesses item = new Businesses { name = "Windsor Plywood" };
                await MobileService.GetTable<Businesses>().InsertAsync(item);

            }




            //--------------------new stuff
            /* businessNames = new List<string>();
             foreach (Businesses item in items)
             {
               businessNames.Add(item.name);
             }*/
            //ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, businessNames);
            // ArrayAdapter<TodoItem> adapter = new ArrayAdapter<TodoItem>(this, Android.Resource.Layout.SimpleListItem1, items);
            //companyListView.Adapter = adapter;

            businessAdapter adapter = new businessAdapter(this, items);
            companyListView.Adapter = adapter;
            RunOnUiThread(() => { progress.Visibility = ViewStates.Invisible; });
            companyListView.ItemClick += listView_ItemClick;

        }

        void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //   Console.WriteLine(companies[e.Position].Text);

            //ArrayAdapter<TodoItem> adapter = new ArrayAdapter<TodoItem>(this, Android.Resource.Layout.SimpleListItem1, companies);
            //companyListView.Adapter = adapter;
            String businessName = companyListView.GetItemAtPosition(e.Position).ToString();
            //String businessName = obj.ToString();
            //Console.WriteLine("String to compare: {0}", businessName);
            var intent = new Intent(this, typeof(ActivityCategory));
            intent.PutExtra("businessNames",businessName);
            intent.PutExtra("user", JsonConvert.SerializeObject(topbar.GetUser()));
            StartActivity(intent);
            Console.WriteLine("1111111111111111111111111111111111");

        }
    }
}