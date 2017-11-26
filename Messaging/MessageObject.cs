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
    public class MessageObject : Java.Lang.Object
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("sender")]
        public string sender { get; set; }
        [JsonProperty("message")]
        public string message { get; set; }
        [JsonProperty("sent_time")]
        public string sent_time { get; set; }
    }

    public class RootObject
    {
        public string Contact { get; set; }
        public List<MessageObject> Messages { get; set; }
        public string ConversationId { get; set; }
    }
}