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
    public class User
    {
        [JsonProperty("Username")]
        public string Username { get; set; }
        [JsonProperty("Password")]
        public string Password { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Status")]
        public string Status { get; set; }
        [JsonProperty("Banned")]
        public string Banned { get; set; }
        [JsonProperty("Contacts")]
        public string ContactListString { get; set; }
        [JsonProperty("Units")]
        public string UnitListString { get; set; }

        public List<Contact> ContactList { get; set; }
        public List<Unit> UnitList { get; set; }
        [JsonProperty("FriendRequests")]
        public string FriendRequestsString { get; set; }

        public List<FriendRequest> FriendRequestList;

        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
            this.Banned = "false";
            this.UnitList = new List<Unit>();
            this.FriendRequestList = new List<FriendRequest>();
            this.ContactList = new List<Contact>();
            this.Status = "User";
        }
    }
}