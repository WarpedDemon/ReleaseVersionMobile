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
using static Android.Views.View;

namespace Messaging
{
    class ContactsAdapter : BaseAdapter<Contact>
    {

        private List<Contact> contacts;
        private Context context;

        public ContactsAdapter(Context context, List<Contact> contacts)
        {
            this.context = context;
            this.contacts = contacts;
        }

        public override Contact this[int position] => throw new NotImplementedException();

        public override int Count
        {
            get { return this.contacts.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return this.contacts[position];
        }

        public override long GetItemId(int position)
        {
            return Convert.ToInt16(contacts[position].Id);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            View row = inflater.Inflate(Resource.Layout.listview_row, null);

            TextView UsernameSlot = (TextView)row.FindViewById(Resource.Id.contactName);
            TextView IdSlot = (TextView)row.FindViewById(Resource.Id.contactId);

            UsernameSlot.Text = contacts[position].Username;
          



            return row;


        }
 
    }  
      
}