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
    class cartAdapter : BaseAdapter<Cart>
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<Cart> cartTable = MobileService.GetTable<Cart>();
        private List<Cart> mItems;
        private Context mContext;
        private string gbusinessName;
        private List<int> positionL;

        public cartAdapter (Context context, List<Cart> items, string businessName)
        {
            mItems = items;
            mContext = context;
            gbusinessName = businessName;
            positionL = new List<int>();
        }

        public override Cart this[int position]
        {
            get
            {
                return mItems[position];
            }
        }

        public override int Count
        {
            get
            {
                try
                { return mItems.Count; }
                catch
                { 
                    return 0;
                }
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Button btnDelete;
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.adapterLayoutCart, null, false);
            


            
            btnDelete = row.FindViewById<Button>(Resource.Id.btnDel);
            //btnDelete.Tag = position;
            btnDelete.SetTag(Resource.Id.btnDel, position);
            //positionL.Add(position);
            btnDelete.Click += async (object sender, EventArgs e) =>
                {
                    Console.WriteLine("Before Position{0}", btnDelete.Tag);
                    int DelPos = (int)(((Button)sender).GetTag(Resource.Id.btnDel));
                    //mItems.RemoveAt(j);
                    Console.WriteLine("Before DelPos{0}", DelPos);
                    Cart item = mItems[DelPos];
                    await cartTable.DeleteAsync(item);
                    mItems.RemoveAt(DelPos);
                    
                    //mItems.RemoveAt(DelPos);
                    Console.WriteLine("Position{0}", btnDelete.Tag);
                    //RemoveAllViewsInLayout();

                    //cartAdapter adapter = new cartAdapter(mContext, mItems, gbusinessName);
                    //view.Adapter = adapter;

                    NotifyDataSetChanged();
                    return;

                };

            //textView.bit = mItems[position].Name;
            //var imageBitmap = OnlinePicture.Stream(String.Format("https://storagedatabase666.blob.core.windows.net/{0}/{1}",mItems[position].Business, mItems[position].Name));
            
        }    
            else
            {
                btnDelete = row.FindViewById<Button>(Resource.Id.btnDel);
           
            }
            //ListView view = row.FindViewById<ListView>(Resource.Id.viewCategories);
            TextView textViewName = row.FindViewById<TextView>(Resource.Id.txtName);
            textViewName.Text = mItems[position].Name;
            TextView textViewPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            textViewPrice.Text = "CAD$ " + mItems[position].Price;
            Button btnViewQuantity = row.FindViewById<Button>(Resource.Id.btnQuantity);
            btnViewQuantity.Text = mItems[position].Quantity;
            ImageView imageView = row.FindViewById<ImageView>(Resource.Id.imageView1);
            gbusinessName = gbusinessName.Replace(" ", "").ToLower();
            String resource = mItems[position].Name.Replace(" ", "");
            imageView.SetImageBitmap(OnlinePicture.Stream(String.Format("https://storagedatabase666.blob.core.windows.net/{0}/product{1}", gbusinessName, resource)));//business name = {0}, 
            return row;
        }
    }
}