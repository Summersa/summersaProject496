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
            //progress = ProgressDialog.Show(this, "", "Loading...");
            //progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            base.OnCreate(bundle);
            Users user = JsonConvert.DeserializeObject<Users>(Intent.GetStringExtra("User"));
            topbar = new ActionBarHelper(this, user);
            topbar.Start();
            ActionBarHelper.GetPicture(this, user);

            progress = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            companyListView = FindViewById<ListView>(Resource.Id.companyListView);
            //------------------------------    UNCOMMENT FOR DATABASE BACK
             IMobileServiceTableQuery<Businesses> query = businessTable.Take(50);
             List<Businesses> items = await query.ToListAsync();
             if(items.Count==0)
             {

                 Businesses item = new Businesses { name = "Windsor Plywood" };
                 await MobileService.GetTable<Businesses>().InsertAsync(item);

             }





             businessAdapter adapter = new businessAdapter(this, items);
             companyListView.Adapter = adapter;
             RunOnUiThread(() => { progress.Visibility = ViewStates.Invisible; });
             companyListView.ItemClick += listView_ItemClick;
            //var intent = new Intent(this, typeof(ActivityCategory));
            //StartActivity(intent);

        }

        void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            String businessName = companyListView.GetItemAtPosition(e.Position).ToString();
            var intent = new Intent(this, typeof(ActivityCategory));
            intent.PutExtra("businessNames",businessName);
            intent.PutExtra("user", JsonConvert.SerializeObject(topbar.GetUser()));
            StartActivity(intent);

        }
    }
}