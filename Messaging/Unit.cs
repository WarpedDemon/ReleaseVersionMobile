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
using Newtonsoft.Json;

namespace Messaging
{
    public class Unit : Java.Lang.Object
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Lecturer")]
        public string Lecturer { get; set; }
        [JsonProperty("startDate")]
        public string startDate { get; set; }
        [JsonProperty("finishDate")]
        public string finishDate { get; set; }
        [JsonProperty("courseName")]
        public string courseName { get; set; }
        [JsonProperty("location")]
        public string location { get; set; }

        public int ListPosition { get; set; }

        public Unit(string Id, string courseName, string Lecturer, string startDate, string finishDate, string location)
        {
            this.Id = Id;
            this.courseName = courseName;
            this.Lecturer = Lecturer;
            this.startDate = startDate;
            this.finishDate = finishDate;
            this.location = location;
        }
        
        //public List<MessageObject> unitList { get; set; }
        //private List<Contact> unitList = new List<Contact>();
    }

}