using System;
using Android.App;
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
using SendGrid;
using System.Net.Mail;
using System.Net;
using Android.Views.InputMethods;
/*EditText et = new EditText(this);
AlertDialog.Builder ad = new AlertDialog.Builder(this);
ad.SetTitle ("Type text");
ad.SetView(et); // <----
ad.Show();*/
namespace App8
{
    [Activity(Label = "DeliveryApp", MainLauncher = true, Icon = "@drawable/icon", Theme ="@style/CustomActionBarTheme", WindowSoftInputMode = Android.Views.SoftInput.StateHidden)]
    public class MainActivity : Activity
    {
        private dialogSignIn signInDialog;
        private dialogSignUp signUpDialog;
        private Button gbtnSignUp;
        private Button gbtnSignIn;
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<Users> userTable = MobileService.GetTable<Users>();
        private IMobileServiceTable<Verificate> verifyTable = MobileService.GetTable<Verificate>();


        protected override async void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.layoutLogin);
            gbtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp1);
            gbtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn1);
            gbtnSignUp.Click += gbtnSignUp_Click;
            gbtnSignIn.Click += gbtnSignIn_Click;
            /*SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo("anthonytyran@hotmail.com");
            myMessage.From = new MailAddress("anthonytyran@hotmail.com", "Anthony Summers");
            myMessage.Subject = "Testing the SendGrid Library";
            myMessage.Text = "Hello World!";

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential("azure_d994d44538e7b536651c95092059223d@azure.com", "Bleach332!!");

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email, which returns an awaitable task.
            await transportWeb.DeliverAsync(myMessage);
            transportWeb.DeliverAsync(myMessage).Wait();*/
        }

        private void gbtnSignIn_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            signInDialog = new dialogSignIn();
            signInDialog.Show(transaction, "dialog fragment");

            signInDialog.gOnSignInComplete += signInDialog_OnSignInComplete;

        }

    

        void gbtnSignUp_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            signUpDialog = new dialogSignUp();
            signUpDialog.Show(transaction, "dialog fragment");

            signUpDialog.gOnSignUpComplete += signUpDialog_OnSignUpComplete;
        }

        private async void signUpDialog_OnSignUpComplete(object sender, OnSignUpEvent e)
        {
            signUpDialog.Dismiss();
            Users user = new Users { username = e.UserName, email = e.Email, password = e.Password, role = "user", verified = false };
            try { await MobileService.GetTable<Users>().InsertAsync(user); }
            catch
            {
                Toast.MakeText(this, "Could Not Reach Database", ToastLength.Long).Show();
                return;
            }
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo("summersa3@mailinator.com");
            myMessage.From = new MailAddress("anthonytyran@hotmail.com", "Anthony Summers");
            myMessage.Subject = "Verification Delivery App!!!";

            Random random = new Random();
            String chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            String rand1 = "";
            for(int i=0;i<3;i++)
            {
                rand1 = rand1 + chars[random.Next(chars.Length)];
            }
            myMessage.Text = "Hello here is your verification code:\n" + rand1;
            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential("azure_d994d44538e7b536651c95092059223d@azure.com", "Bleach332!!");

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email, which returns an awaitable task.
            await transportWeb.DeliverAsync(myMessage);
            //transportWeb.DeliverAsync(myMessage).Wait();
            Verificate v = new Verificate { email = user.email, verifyString = rand1};
            await MobileService.GetTable<Verificate>().InsertAsync(v);
        }
        private async void signInDialog_OnSignInComplete(object sender, OnSignInEvent e)
        {
            //signInDialog.Dismiss();
            Console.WriteLine("WE JUST PRESSED LOG IN BUTTON");
            IMobileServiceTableQuery<Users> query = userTable.Where(Users => Users.email == e.Email).Take(1);
            List<Users> items;
            try { items = await query.ToListAsync(); }
            catch
            {
                Toast.MakeText(this, "Could Not Reach Database", ToastLength.Long).Show();
                return;
            }
            if(items.Count == 0)
            {
                Toast.MakeText(this, "Incorrect UserName or Password", ToastLength.Long).Show();
                return;
            }
            Console.WriteLine("String to compare: {0} {1}", items[0].password, e.Password);

            if (items[0].password.Equals(e.Password, StringComparison.Ordinal))
            {
                /*catch
                {
                    Toast.MakeText(this, "Incorrect UserName or Password", ToastLength.Long).Show();
                    return;
                }*/
                if (items[0].verified == false)
                {
                    EditText et = new EditText(this);
                    AlertDialog.Builder ad = new AlertDialog.Builder(this);
                    ad.SetTitle("Email not Verified Type in Verified String");
                    ad.SetView(et); // <----
                    ad.SetPositiveButton("OK", async (s, ev) =>
                    {
                        IMobileServiceTableQuery<Verificate> v = verifyTable.Where(Verificate => Verificate.email == items[0].email).Take(1);
                        List<Verificate> listV;
                        listV = await v.ToListAsync();
                        if (et.Text == listV[0].verifyString)
                        {
                            items[0].verified = true;
                            await MobileService.GetTable<Users>().UpdateAsync(items[0]);
                            return;
                        }
                        else
                        {

                            ad.Dispose();
                            return;
                        }
                    });
                    ad.Show();
                    return;
                }
                else if (items.Count != 0 && items[0].password.Equals(e.Password, StringComparison.Ordinal)) // if you find a matching email and the password is correct
                {
                    //Users us = new Users {username="dan", role = "user", Id = "1"};

                    var intent = new Intent(this, typeof(ActivityCategory));
                    intent.PutExtra("user", JsonConvert.SerializeObject(items[0]));
                    Console.WriteLine("WE LOGGED IN FROM RETRIEVING USER FROM DATABASE");
                    StartActivity(intent);
                }
            }
            else
            {
                //hideSoftKeyboard();
                //Toast.MakeText(this, "Incorrect UserName or Password", ToastLength.Long).Show();
                return;
            }
            //hideSoftKeyboard();
        }
        public void hideSoftKeyboard()
        {
            InputMethodManager inputMethodManager =
            (InputMethodManager)this.GetSystemService(
                    Activity.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(
                this.CurrentFocus.WindowToken, 0);
        }
    }
}

