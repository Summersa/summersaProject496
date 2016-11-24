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

namespace App8
{
    class OnSignInEvent : EventArgs
    {
        private string gEmail;
        private string gPassword;

        public string Email
        {
            get { return gEmail; }
            set { gEmail = value; }
        }
        public string Password
        {
            get { return gPassword; }
            set { gPassword = value; }
        }

        public OnSignInEvent(string email, string password) : base()
        {
            
            Email = email;
            Password = password;
         
        }
    }
        class dialogSignIn : DialogFragment
    {
        //public IMobileServiceTable<Users> userTable = MobileService.GetTable<Users>();
        private EditText gTxtEmail;
        private EditText gTxtPassword;
        private Button gBtnSignIn;

        public event EventHandler<OnSignInEvent> gOnSignInComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.layoutSignInFragment, container, false);

            gTxtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            gTxtPassword = view.FindViewById<EditText>(Resource.Id.txtPasswordSignIn);
            gBtnSignIn = view.FindViewById<Button>(Resource.Id.btnSignUp);

            gBtnSignIn.Click += gBtnSignUp_Click;

            return view;
            // return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void gBtnSignUp_Click(object sender, EventArgs e)
        {
            
           // Console.WriteLine("String to compare: {0} {1}", gTxtPassword.Text, gTxtEmail.Text);
           gOnSignInComplete.Invoke(this, new OnSignInEvent(gTxtEmail.Text,gTxtPassword.Text));
           this.Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);//sets ugly top bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialogAnimation;// set animation
        }
    }
}