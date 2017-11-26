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
    public class FriendRequest
    {
        [JsonProperty("Username")]
        public string Username;
        [JsonProperty("Id")]
        public string Id;
    }
}