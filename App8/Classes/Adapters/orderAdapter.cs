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

    class orderAdapter : BaseAdapter<Orders>
    {
        private List<Orders> mItems;
        private Context mContext;
        private List<String> userNames;

        public orderAdapter(Context context, List<Orders> items, List<String> userNames)
        {
            mItems = items;
            mContext = context;
            this.userNames = userNames;
        }

        public override Orders this[int position]
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
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.adapterLayoutOrders, null, false);
            }
            TextView textView = row.FindViewById<TextView>(Resource.Id.textOrders);
            int newpos = position + 1;
            textView.Text = "Order #" + newpos;

            TextView textName = row.FindViewById<TextView>(Resource.Id.textUser);
            textName.Text = userNames[position];

            TextView textPhone = row.FindViewById<TextView>(Resource.Id.textPhone);
            textPhone.Text = mItems[position].Phone;
            TextView textAddress = row.FindViewById<TextView>(Resource.Id.textAddress);
            textAddress.Text = mItems[position].Address;
            return row;
        }

    }
}