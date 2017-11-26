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
    public class HttpResponse
    {   
        [JsonProperty ("Status")]
        public int Status { get; set; }

        [JsonProperty ("Response")]
        public string Response { get; set; }
    }
}