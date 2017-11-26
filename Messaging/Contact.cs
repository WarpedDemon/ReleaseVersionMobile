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
    public class Contact : Java.Lang.Object
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Username")]
        public string Username { get; set; }

        public int ListPosition { get; set; }
        
        public List<MessageObject> messageList { get; set; }
        private List<Contact> contactList = new List<Contact>();
    }

}