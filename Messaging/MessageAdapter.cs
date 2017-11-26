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

namespace Messaging
{
    class MessageAdapter : BaseAdapter
    {
        private List<MessageObject> messageList;
        private Context context;

        public MessageAdapter(Context context, List<MessageObject> messageList)
        {
            this.context = context;
            this.messageList = messageList;
        }
        

        public override int Count
        {
            get { return this.messageList.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return this.messageList[position];
        }

        public override long GetItemId(int position)
        {
            return Convert.ToInt16(messageList[position].id);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            View row = inflater.Inflate(Resource.Layout.message, null);
            
            TextView messageRow = row.FindViewById<TextView>(Resource.Id.message);
            Console.WriteLine(this.messageList[position].message);
            messageRow.Text = this.messageList[position].message;

            TextView messageTime = row.FindViewById<TextView>(Resource.Id.messageTime);
            messageTime.Text = this.messageList[position].sent_time;

            TextView contactName = row.FindViewById<TextView>(Resource.Id.senderName);
            contactName.Text = this.messageList[position].sender;

            return row;
        }
    }
}