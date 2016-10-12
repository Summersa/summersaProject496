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
    class AddCategoryEvent : EventArgs
    {
        private Android.Net.Uri gPicture;
        private string gDescription;

        public string Description
        {
            get { return gDescription; }
            set { gDescription = value; }
        }
        public Android.Net.Uri Picture
        {
            get { return gPicture; }
            set { gPicture = value; }
        }


        public AddCategoryEvent(string description, Android.Net.Uri picture) : base()
        {
            
            Description = description;
            Picture = picture;
         
        }
    }
        class dialogAddCategory : DialogFragment
    {
       
        //public IMobileServiceTable<Users> userTable = MobileService.GetTable<Users>();
        private EditText gTxtDescription;
        private Button gBtnPicture;
        private Button gBtnFinish;
        private Android.Net.Uri picture;

        public event EventHandler<AddCategoryEvent> gAddCategoryEventComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragmentAddCategory, container, false);

            gTxtDescription = view.FindViewById<EditText>(Resource.Id.txtDescription);
            gBtnPicture = view.FindViewById<Button>(Resource.Id.btnPicture);
            gBtnFinish = view.FindViewById<Button>(Resource.Id.btnFinish);
            gBtnFinish.Click += gBtnSignUp_Click;
            gBtnPicture.Click += addPicture_Click;

            return view;
            // return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void addPicture_Click(object sender, EventArgs e)
        {
            Activity.Intent = new Intent();
            Activity.Intent.SetType("image/*");
            Activity.Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Activity.Intent, "Select Picture"), 1000);
        }
        public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == 1000) && (resultCode == Result.Ok) && (data != null))
            {

                picture = data.Data;
            }
        }
        private void gBtnSignUp_Click(object sender, EventArgs e)
        {
            
           // Console.WriteLine("String to compare: {0} {1}", gTxtPassword.Text, gTxtEmail.Text);
           gAddCategoryEventComplete.Invoke(this, new AddCategoryEvent(gTxtDescription.Text,picture));
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