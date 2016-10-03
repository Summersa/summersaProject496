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
    class dialogProfile : DialogFragment
    {
        private Button changePicture;
        public static readonly int PickImageId = 1000;
        private Users user;
        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragmentProfile, container, false);
            changePicture = view.FindViewById<Button>(Resource.Id.changePicture);


            changePicture.Click += ChangePictureClick;

            return view;
            // return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void ChangePictureClick(object sender, EventArgs e)
        {
            Activity.Intent = new Intent();
            Activity.Intent.SetType("image/*");
            Activity.Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Activity.Intent, "Select Picture"), PickImageId);
           // 
            //Toast.MakeText(act, "Touched Image", ToastLength.Long).Show();
            //OnlinePicture.Upload(this,);
        }

        public void GetUser(Users user)
        {
            this.user = user;
        }

        public override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {

                String containerName = "profilepictures";

                String referenceName = "dan";

                //await OnlinePicture.Upload(this,data.Data, Replace("  ", string.empty); businessNameI,newCategoryNameI);
                await OnlinePicture.Upload(Activity, data.Data, containerName, referenceName);
                //ActionBarHelper.GetPicture(Activity, user);
            }
        }
    }
}