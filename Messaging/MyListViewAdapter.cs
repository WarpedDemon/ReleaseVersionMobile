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

namespace Messaging
{
    class MyListViewAdapter : BaseAdapter<string>
    {
        private List<string> mItems;
        private Context mContext;

        public MyListViewAdapter(Context context, List<string> items)
        {
            mItems = items;
            mContext = context;
        }


        public override int Count
        {
            get { return mItems.Count;  }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override string this[int position]
        {
            get { return mItems[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if(row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.listview_row, null, false);
            }

            TextView txtName = row.FindViewById<TextView>(Resource.Id.contactName);

            txtName.Text = mItems[position];

            return row;

        }

    }
}