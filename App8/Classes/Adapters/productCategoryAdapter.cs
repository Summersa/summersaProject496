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
    class productCategoryAdapter : BaseAdapter<ProductCategory>
    {
        private List<ProductCategory> mItems;
        private Context mContext;

        public productCategoryAdapter (Context context, List<ProductCategory> items)
        {
            mItems = items;
            mContext = context;
        }

        public override ProductCategory this[int position]
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.adapterLayoutCategory,null,false);
            }
            TextView textView = row.FindViewById<TextView>(Resource.Id.textCategory);
            textView.Text = mItems[position].Name;
       
            ImageView imageView = row.FindViewById<ImageView>(Resource.Id.imageView1);
            String category = mItems[position].Business.ToLower();
            category = category.Replace(" ", "");
            String resource = mItems[position].Name.Replace(" ", "");
            //textView.bit = mItems[position].Name;
            //var imageBitmap = OnlinePicture.Stream(String.Format("https://storagedatabase666.blob.core.windows.net/{0}/{1}",mItems[position].Business, mItems[position].Name));
            imageView.SetImageBitmap(OnlinePicture.Stream(String.Format("https://storagedatabase666.blob.core.windows.net/{0}/{1}", category,resource)));
            return row;
        }
    }
}