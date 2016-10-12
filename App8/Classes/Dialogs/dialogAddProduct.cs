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
    class AddProductEvent : EventArgs
    {
        private Android.Net.Uri gPicture;
        private string gDescription;
        private string gName;
        private double gPrice;
        public string Name
        {
            get { return gName; }
            set { gName = value; }
        }
        public string Description
        {
            get { return gDescription; }
            set { gDescription = value; }
        }
        public double Price
        {
            get { return gPrice; }
            set { gPrice = value; }
        }
        public Android.Net.Uri Picture
        {
            get { return gPicture; }
            set { gPicture = value; }
        }


        public AddProductEvent(string name,string description, double price,Android.Net.Uri picture) : base()
        {
            Name = name;
            Description = description;
            Price = price;
            Picture = picture;
         
        }
    }
        class dialogAddProduct : DialogFragment
    {
       
        //public IMobileServiceTable<Users> userTable = MobileService.GetTable<Users>();
        private EditText gTxtDescription;
        private EditText gTxtName;
        private EditText gTxtPrice;
        private Button gBtnPicture;
        private Button gBtnFinish;
        private Android.Net.Uri picture;

        public event EventHandler<AddProductEvent> gAddProductEventComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragmentAddProduct, container, false);

            gTxtDescription = view.FindViewById<EditText>(Resource.Id.txtDescription);
            gTxtName = view.FindViewById<EditText>(Resource.Id.txtName);
            gTxtPrice = view.FindViewById<EditText>(Resource.Id.txtPrice);
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
           gAddProductEventComplete.Invoke(this, new AddProductEvent(gTxtName.Text,gTxtDescription.Text, double.Parse(gTxtPrice.Text) ,picture));
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