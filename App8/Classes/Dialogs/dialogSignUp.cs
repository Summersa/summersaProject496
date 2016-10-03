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

namespace App8
{

    class OnSignUpEvent : EventArgs
    {
        private string gUserName;
        private string gEmail;
        private string gPassword;
        
        public string UserName
        {
            get { return gUserName; }
            set { gUserName = value; }
        }

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

        public OnSignUpEvent(string userName,string email, string password) : base()
        {
            UserName = userName;
            Email = email;
            Password = password;
        }
    }

    class dialogSignUp : DialogFragment
    {
        private EditText gTxtUsername;
        private EditText gTxtEmail;
        private EditText gTxtPassword;
        private EditText gTxtPassword2;
        private Button gBtnSignUp;

        public event EventHandler<OnSignUpEvent> gOnSignUpComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.layoutLoginFragment, container, false);


            gTxtUsername = view.FindViewById<EditText>(Resource.Id.txtUserName);
            gTxtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            gTxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            gTxtPassword2 = view.FindViewById<EditText>(Resource.Id.txtPassword2);
            gBtnSignUp = view.FindViewById<Button>(Resource.Id.btnSignUp);

            gBtnSignUp.Click += gBtnSignUp_Click;

            return view;
            // return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void gBtnSignUp_Click(object sender, EventArgs e)
        {
            if (!gTxtPassword.Text.Equals(gTxtPassword2.Text, StringComparison.Ordinal))
            {
                this.Dismiss();
                return;
            }
            //throw new NotImplementedException();
            else
            {
                gOnSignUpComplete.Invoke(this, new OnSignUpEvent(gTxtUsername.Text, gTxtEmail.Text, gTxtPassword.Text));
                this.Dismiss();
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);//sets ugly top bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialogAnimation;// set animation
        }
    }
}