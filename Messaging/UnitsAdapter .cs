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
    class UnitsAdapter : BaseAdapter<Unit>
    {

        private List<Unit> units;
        private Context context;

        public UnitsAdapter(Context context, List<Unit> units)
        {
            this.context = context;
            this.units = units;
        }

        public override Unit this[int position] => throw new NotImplementedException();

        public override int Count
        {
            get { return this.units.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return this.units[position];
        }

        public override long GetItemId(int position)
        {
            return Convert.ToInt16(units[position].Id);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            View row = inflater.Inflate(Resource.Layout.unitlistveiw_row, null);

            TextView idSlot = (TextView)row.FindViewById(Resource.Id.unitId);
            TextView nameSlot = (TextView)row.FindViewById(Resource.Id.unitLecturer);
            TextView startSlot = (TextView)row.FindViewById(Resource.Id.startDate);
            TextView finishSlot = (TextView)row.FindViewById(Resource.Id.finishDate);
            TextView courseSlot = (TextView)row.FindViewById(Resource.Id.courseName);
            TextView locationSlot = (TextView)row.FindViewById(Resource.Id.courseLocation);

            idSlot.Text = units[position].Id;
            nameSlot.Text = units[position].Lecturer;
            startSlot.Text = units[position].startDate;
            finishSlot.Text = units[position].finishDate;
            courseSlot.Text = units[position].courseName;
            locationSlot.Text = units[position].location;



            return row;


        }
 
    }  
      
}