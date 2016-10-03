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
using Java.Lang;

namespace App8
{
    class businessAdapter : BaseAdapter<Businesses>
    {
        private List<Businesses> mItems;
        private Context mContext;

        public businessAdapter(Context context, List<Businesses> items)
        {
            mItems = items;
            mContext = context;
        }

        public override Businesses this[int position]
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.adapterLayoutBusiness,null,false);
            }
            TextView textView = row.FindViewById<TextView>(Resource.Id.textBusiness);
            textView.Text = mItems[position].name;
            return row;
        }
        
    }
}