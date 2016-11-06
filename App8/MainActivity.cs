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
using Android.Database;
using Android.Provider;
using Android.Graphics;
using System.Net;
using Newtonsoft.Json;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;

namespace App8
{
    [Activity(Label = "DeliveryApp", MainLauncher = true, Icon = "@drawable/icon", Theme ="@style/CustomActionBarTheme", WindowSoftInputMode = Android.Views.SoftInput.StateHidden)]
    public class MainActivity : Activity
    {

        private Button gbtnSignUp;
        private Button gbtnSignIn;
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<Users> userTable = MobileService.GetTable<Users>();



        protected override async void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.layoutLogin);
            gbtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp1);
            gbtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn1);
            gbtnSignUp.Click += gbtnSignUp_Click;
            gbtnSignIn.Click += gbtnSignIn_Click;

        }

        private void gbtnSignIn_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            dialogSignIn signInDialog = new dialogSignIn();
            signInDialog.Show(transaction, "dialog fragment");

            signInDialog.gOnSignInComplete += signInDialog_OnSignInComplete;
          
        }

    

        void gbtnSignUp_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            dialogSignUp signUpDialog = new dialogSignUp();
            signUpDialog.Show(transaction, "dialog fragment");

            signUpDialog.gOnSignUpComplete += signUpDialog_OnSignUpComplete;
        }

        private async void signUpDialog_OnSignUpComplete(object sender, OnSignUpEvent e)
        {
            Users user = new Users { username = e.UserName, email = e.Email, password = e.Password, role = "Windsor Plywood" };
            await MobileService.GetTable<Users>().InsertAsync(user);
            // e.UserName
        }
        private async void signInDialog_OnSignInComplete(object sender, OnSignInEvent e)
        {
            Console.WriteLine("WE JUST PRESSED LOG IN BUTTON");
            IMobileServiceTableQuery<Users> query = userTable.Where(Users => Users.email == e.Email).Take(1);
            List<Users> items;
            try { items = await query.ToListAsync(); }
            catch
            {
                Toast.MakeText(this, "Could Not Reach Database", ToastLength.Long).Show();
                return;
            }
            Console.WriteLine("String to compare: {0} {1}", items[0].password, e.Password);
            if (items.Count != 0 && items[0].password.Equals(e.Password, StringComparison.Ordinal)) // if you find a matching email and the password is correct
            {
                //Users us = new Users {username="dan", role = "user", Id = "1"};
  
                var intent = new Intent(this, typeof(ActivityCategory));
                intent.PutExtra("user", JsonConvert.SerializeObject(items[0]));
                Console.WriteLine("WE LOGGED IN FROM RETRIEVING USER FROM DATABASE");
                StartActivity(intent);
            }
        }

    }
}

