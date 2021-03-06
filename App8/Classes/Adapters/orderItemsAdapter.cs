using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class orderItemsAdapter : BaseAdapter<string>
    {
        private string[] mItems,mItems2,mItems3;
        private Context mContext;
        private string gbusinessName;

        public override int Count
        {
            get
            {
                return mItems.Length;
            }
        }

        public orderItemsAdapter(Context context, string[] items, string[] quantities, string[] prices, string businessName)
        {
            mItems = items;
            mItems2 = quantities;
            mItems3 = prices;
            mContext = context;
            gbusinessName = businessName;

        }

        public override string this[int position]
        {
            get
            {
                return mItems[position];
            }
        }

     

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Console.WriteLine("MADE IT");
            View row = convertView;
            if(row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.adapterLayoutOrderItems,null,false);
            }

            TextView textViewName = row.FindViewById<TextView>(Resource.Id.txtName);
            textViewName.Text = mItems[position];
            TextView textViewPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            textViewPrice.Text = "CAD$ " + mItems3[position];
            Button btnViewQuantity = row.FindViewById<Button>(Resource.Id.btnQuantity);
            btnViewQuantity.Text = mItems2[position];
            ImageView imageView = row.FindViewById<ImageView>(Resource.Id.imageView1);
            gbusinessName = gbusinessName.Replace(" ", "").ToLower();
   
            String resource = mItems[position].Replace(" ", "");
            Console.WriteLine("Resource:{0}",resource);
            //textView.bit = mItems[position].Name;
            //var imageBitmap = OnlinePicture.Stream(String.Format("https://storagedatabase666.blob.core.windows.net/{0}/{1}",mItems[position].Business, mItems[position].Name));
            if(resource !="" || resource != null)
                imageView.SetImageBitmap(OnlinePicture.Stream(String.Format("https://storagedatabase666.blob.core.windows.net/{0}/product{1}", gbusinessName, resource)));//business name = {0}, 
            return row;
        }
        
    }
}