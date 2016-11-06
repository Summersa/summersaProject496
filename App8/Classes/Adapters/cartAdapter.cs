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
    class cartAdapter : BaseAdapter<Cart>
    {
        private List<Cart> mItems;
        private Context mContext;
        private string gbusinessName;

        public cartAdapter (Context context, List<Cart> items, string businessName)
        {
            mItems = items;
            mContext = context;
            gbusinessName = businessName;
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
                return mItems.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if(row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.adapterLayoutCart,null,false);
            }
            TextView textViewName = row.FindViewById<TextView>(Resource.Id.txtName);
            textViewName.Text = mItems[position].Name;
            TextView textViewPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            textViewPrice.Text = "CAD$ "+ mItems[position].Price;
            Button btnViewQuantity = row.FindViewById<Button>(Resource.Id.btnQuantity);
            btnViewQuantity.Text = mItems[position].Quantity;
            ImageView imageView = row.FindViewById<ImageView>(Resource.Id.imageView1);
            gbusinessName = gbusinessName.Replace(" ", "").ToLower();
            String resource = mItems[position].Name.Replace(" ", "");
            //textView.bit = mItems[position].Name;
            //var imageBitmap = OnlinePicture.Stream(String.Format("https://storagedatabase666.blob.core.windows.net/{0}/{1}",mItems[position].Business, mItems[position].Name));
            imageView.SetImageBitmap(OnlinePicture.Stream(String.Format("https://storagedatabase666.blob.core.windows.net/{0}/product{1}", gbusinessName,resource)));//business name = {0}, 
            return row;
        }
    }
}