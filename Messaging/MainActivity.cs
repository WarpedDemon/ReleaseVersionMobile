using Android.App;
using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


using System.Collections.Generic;
using Android.Views.Animations;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Android.Graphics;
using Newtonsoft.Json.Linq;
using Android.Content.PM;
using Android.Views.InputMethods;

namespace Messaging
{
    [Activity(Label = "Messaging", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        private User LoggedUser;
        private Button loginButton;
        public Button RegisterButton;

        //private ListView mListView;
        private WebClient mClient = new WebClient();

        // private List<string> mItems;

        private List<Contact> contactList = new List<Contact>();
        private Unit currentUnitView;

        private EditText usernameInput;
        private EditText passwordInput;

        private TextView title;

        Contact currentContact;
        int currentContactPosition;
        Uri mUri;

        public ListView messageListView;
        ListView list;
        ListView UnitListView;
        Unit NewUnit;

        public ImageButton SendButton;
        public ImageButton BackButton;
        public ImageButton RefreshButton;
        public ImageButton FriendRequestsPageButton;
        public ImageButton AddContactButton;
        public ImageButton AddNewContactButton;
        public ImageButton AddContactBackButton;
        public ImageButton RegisterSubmitButton;
        public ImageButton RegisterBackButton;
        public ImageButton AddUnitButton;
        public ImageButton UnitBackButton;
        public ImageButton UnitPageButton;
        public ImageButton UnitViewBackButton;
        public ImageButton AddUnitBackButton;
        public ImageButton AddUnitSubmitButton;

        public EditText ContactInput;

        public ProgressBar loginLoader;
        public ProgressBar MessengerLoader;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            mClient.Proxy = null;

            SetContentView(Resource.Layout.Messenger);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);
            title = FindViewById<TextView>(Resource.Id.textView4);
            loginLoader = FindViewById<ProgressBar>(Resource.Id.loginLoader);
            mUri = new Uri("http://arcane-headland-59258.herokuapp.com/PHPAPI.php");

            loginButton = FindViewById<Button>(Resource.Id.button1);
            loginButton.Click += LoginButton_Click;
            //commmented
            RegisterButton = FindViewById<Button>(Resource.Id.registerButton);
            RegisterButton.Click += RegisterButton_Click;
        }
        
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.RegisterView);

            RegisterSubmitButton = FindViewById<ImageButton>(Resource.Id.RegisterSubmitButton);
            RegisterSubmitButton.Click += RegisterSubmitButton_Click;
            RegisterBackButton = FindViewById<ImageButton>(Resource.Id.RegisterBackButton);
            RegisterBackButton.Click += RegisterBackButton_Click;
        }

        private void RegisterSubmitButton_Click(object sender, EventArgs e)
        {
            EditText NewUsernameInput = FindViewById<EditText>(Resource.Id.NewUsernameInput);
            EditText NewEmailInput = FindViewById<EditText>(Resource.Id.NewEmailInput);
            EditText NewPasswordInput = FindViewById<EditText>(Resource.Id.NewPasswordInput);
            EditText NewPasswordConfirmInput = FindViewById<EditText>(Resource.Id.NewPasswordConfirmInput);

            string NewUsernameInputString = NewUsernameInput.Text;
            string NewEmailInputString = NewEmailInput.Text;
            string NewPasswordInputString = NewPasswordInput.Text;
            string NewPasswordConfirmInputString = NewPasswordConfirmInput.Text;

            if (NewUsernameInputString == "" || NewEmailInputString == "" || NewPasswordInputString == "" || NewPasswordConfirmInputString == "")
            {
                Toast.MakeText(this, "Please enter all fields!", ToastLength.Short).Show();
            }
            else if (NewPasswordInputString != NewPasswordConfirmInputString)
            {
                Toast.MakeText(this, "Both passwords arent the same please check!", ToastLength.Short).Show();
            }
            else
            {
                //Create new User to upload...
                User user = new User(NewUsernameInputString, NewPasswordInputString);

                //Create POST parameters
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("Username", "Admin");
                parameters.Add("Password", "nanikorebakadesu");
                parameters.Add("NewUser", JsonConvert.SerializeObject(user));
                parameters.Add("Request", "NewUser");

                mClient.UploadValuesAsync(mUri, parameters);
                RegisterSubmitButton.Enabled = false;
                RegisterBackButton.Enabled = false;
                mClient.UploadValuesCompleted += CompletedRegister;
            }
        }

        private void CompletedRegister(object sender, UploadValuesCompletedEventArgs e)
        {
            mClient.UploadValuesCompleted -= CompletedRegister;

            RegisterSubmitButton.Enabled = true;
            RegisterBackButton.Enabled = true;

            string data = Encoding.UTF8.GetString(e.Result);
            HttpResponse response = JsonConvert.DeserializeObject<HttpResponse>(data);

            if(response.Status != 202)
            {
                //Response didn't come back ok...
                Toast.MakeText(this, "Something went wrong :/", ToastLength.Short).Show();
            } else
            {
                SwitchToLogin();
                Toast.MakeText(this, "Successfull created new user!", ToastLength.Long).Show();
            }
        }
        
        private void SwitchToLogin()
        {
            SetContentView(Resource.Layout.Login);
            loginButton = FindViewById<Button>(Resource.Id.button1);
            RegisterButton = FindViewById<Button>(Resource.Id.registerButton);
            loginButton.Click += LoginButton_Click;
            RegisterButton.Click += RegisterButton_Click;
        }
        private void RegisterBackButton_Click(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.Login);
            loginButton = FindViewById<Button>(Resource.Id.button1);
            RegisterButton = FindViewById<Button>(Resource.Id.registerButton);
            loginButton.Click += LoginButton_Click;
            RegisterButton.Click += RegisterButton_Click;
        }
        
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Refreshing chat...", ToastLength.Long).Show();
            MessengerLoader.Visibility = ViewStates.Visible;
            RefreshButton.Enabled = false;
            SendButton.Enabled = false;
            BackButton.Enabled = false;

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("Username", LoggedUser.Username);
            parameters.Add("Password", LoggedUser.Password);
            parameters.Add("Request", "GetConversations");

            mClient.UploadValuesCompleted += ConversationRefreshComplete;
            mClient.UploadValuesAsync(mUri, parameters);


        }

        private void LoginCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string data = Encoding.UTF8.GetString(e.Result);
            Console.WriteLine("WE FOUND SOME DATA " + data);
            LoggedUser = JsonConvert.DeserializeObject<User>(data);


            LoggedUser.ContactList = JsonConvert.DeserializeObject<List<Contact>>(LoggedUser.ContactListString);
            if (LoggedUser.ContactList == null) { LoggedUser.ContactList = new List<Contact>(); }
            LoggedUser.FriendRequestList = JsonConvert.DeserializeObject<List<FriendRequest>>(LoggedUser.FriendRequestsString);
            if (LoggedUser.FriendRequestList == null) { LoggedUser.FriendRequestList = new List<FriendRequest>(); }
            List<Unit> unitList = JsonConvert.DeserializeObject<List<Unit>>(LoggedUser.UnitListString);
            LoggedUser.UnitList = unitList;
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("Username", LoggedUser.Username);
            parameters.Add("Password", LoggedUser.Password);
            parameters.Add("Request", "GetConversations");

            Uri mUri = new Uri("http://arcane-headland-59258.herokuapp.com/PHPAPI.php");

            mClient.UploadValuesCompleted -= LoginCompleted;
            mClient.UploadValuesCompleted += ConversationsCompleted;

            mClient.UploadValuesAsync(mUri, parameters);
        }

        public void ServiceException() {
            //data = Encoding.UTF8.GetString(e.Result);
            Console.WriteLine("Check Network Connection Or Service Maybe Be Down.");
        }

        private void ConversationsCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            mClient.UploadValuesCompleted -= ConversationsCompleted;
            if (e.Result == null)
            {
                ServiceException();
                return;
            }
            string data = Encoding.UTF8.GetString(e.Result);
            Console.WriteLine("Data HERE?: " + data);
            List<RootObject> obj = JsonConvert.DeserializeObject<List<RootObject>>(data);


            AssignMessages(obj);
            SetContentView(Resource.Layout.Main);
            UnitPageButton = FindViewById<ImageButton>(Resource.Id.GoToUnitButton);
            UnitPageButton.Click += UnitsPage;

            FriendRequestsPageButton = FindViewById<ImageButton>(Resource.Id.GoToFriendRequestsButton);
            if(LoggedUser.FriendRequestList.Count > 0)
            {
                FriendRequestsPageButton.SetBackgroundResource(Resource.Drawable.ic_info);
            }
           
            FriendRequestsPageButton.Click += FriendRequestsPage;
            LoadContacts();

        }

        private void FriendRequestsPage(object sender, EventArgs e)
        {
            Toast.MakeText(this, "This area isn't finished yet. Come back later...", ToastLength.Short).Show();
            FriendRequestsPageButton.Enabled = false;
        }

        private void UnitsPage(object sender, EventArgs e)
        {
           

            SetContentView(Resource.Layout.Units);

            //Assign Button element to variable.
            AddUnitButton = FindViewById<ImageButton>(Resource.Id.AddUnitButton);
            AddUnitButton.Click += AddUnitButton_Click;
            UnitBackButton = FindViewById<ImageButton>(Resource.Id.UnitBackButton);
            UnitBackButton.Click += SwitchToContacts_Button;


            //Assign list view with adapter.
            UnitListView = FindViewById<ListView>(Resource.Id.UnitListView);
            UnitListView.ItemClick += UnitListView_ItemClick;
            UnitListView.Adapter = new UnitsAdapter(this, LoggedUser.UnitList);

        }

        private void UnitListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int ListPosition = e.Position;
            currentUnitView = LoggedUser.UnitList[ListPosition];
            SetContentView(Resource.Layout.UnitView);
            //Populate Unit View Page with necessary details...
            TextView UnitPageTitle = FindViewById<TextView>(Resource.Id.UnitPageTitle);
            TextView UnitId = FindViewById<TextView>(Resource.Id.UnitId);
            TextView UnitName = FindViewById<TextView>(Resource.Id.UnitName);
            TextView UnitLecturer = FindViewById<TextView>(Resource.Id.UnitLecturer);
            TextView UnitStartDate = FindViewById<TextView>(Resource.Id.UnitStartDate);
            TextView UnitEndDate = FindViewById<TextView>(Resource.Id.UnitEndDate);
            TextView UnitLocation = FindViewById<TextView>(Resource.Id.UnitLocation);

            UnitViewBackButton = FindViewById<ImageButton>(Resource.Id.UnitViewBackButton);

            UnitPageTitle.Text = "Unit - " + currentUnitView.Id;
            UnitId.Text = currentUnitView.Id;
            UnitName.Text = currentUnitView.courseName;
            UnitLecturer.Text = currentUnitView.Lecturer;
            UnitStartDate.Text = currentUnitView.startDate;
            UnitEndDate.Text = currentUnitView.finishDate;
            UnitLocation.Text = currentUnitView.location;

            UnitViewBackButton.Click += UnitsPage;

        }



        private void UnitBackButton_Click(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.Main);
        }

        private void AddUnitButton_Click(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.NewUnitView);

            AddUnitBackButton = FindViewById<ImageButton>(Resource.Id.AddNewUnitBackButton);
            AddUnitBackButton.Click += UnitsPage;
            AddUnitSubmitButton = FindViewById<ImageButton>(Resource.Id.AddNewUnitSubmitButton);
            AddUnitSubmitButton.Click += AddUnitSubmitButton_Click;
        }

        private void AddUnitSubmitButton_Click(object sender, EventArgs e)
        {
            EditText IdInput = FindViewById<EditText>(Resource.Id.NewUnitIdInput);
            EditText NameInput = FindViewById<EditText>(Resource.Id.NewUnitNameInput);
            EditText LecturerInput = FindViewById<EditText>(Resource.Id.NewUnitLecturerInput);
            EditText StartDateInput = FindViewById<EditText>(Resource.Id.NewUnitStartDateInput);
            EditText EndDateInput = FindViewById<EditText>(Resource.Id.NewUnitEndDateInput);
            EditText LocationInput = FindViewById<EditText>(Resource.Id.NewUnitLocationInput);

            string Id = IdInput.Text;
            string Name = NameInput.Text;
            string Lecturer = LecturerInput.Text;
            string StartDate = StartDateInput.Text;
            string EndDate = EndDateInput.Text;
            string Location = LocationInput.Text;
            if (Id == "" || Name == "" || Lecturer == "" || StartDate == "" || EndDate == "" || Location == "")
            {
                Toast.MakeText(this, "Please enter all fields!", ToastLength.Short).Show();
            } else
            {
                NewUnit = new Unit(Id, Name, Lecturer, StartDate, EndDate, Location);
                PostNewUnit(NewUnit);
            }
           
        }

        private void PostNewUnit(Unit unit) 
        {
            AddUnitBackButton.Enabled = false;
            AddUnitSubmitButton.Enabled = false;

            string UnitJsonData = JsonConvert.SerializeObject(unit);
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("Username", LoggedUser.Username);
            parameters.Add("Password", LoggedUser.Password);
            parameters.Add("Request", "AddUnit");
            parameters.Add("Unit", UnitJsonData);

            mClient.UploadValuesAsync(mUri, parameters);
            mClient.UploadValuesCompleted += CompletedUploadUnit;

        }

        private void CompletedUploadUnit(object sender, UploadValuesCompletedEventArgs e)
        {
            mClient.UploadValuesCompleted -= CompletedUploadUnit;
            string data = Encoding.UTF8.GetString(e.Result);
            HttpResponse response = JsonConvert.DeserializeObject<HttpResponse>(data);

            AddUnitBackButton.Enabled = true;
            AddUnitSubmitButton.Enabled = true;

            if (response.Status == 202)
            {
                LoggedUser.UnitList.Add(NewUnit);
                UnitsPageNormal();
                Toast.MakeText(this, "Successfully added unit!", ToastLength.Long).Show();
            } else
            {
                Toast.MakeText(this, "Error (" + response.Status + ") : " + response.Response, ToastLength.Long).Show();
            }

     
        }

        private void UnitsPageNormal()
        {
            SetContentView(Resource.Layout.Units);

            //Assign Button element to variable.
            AddUnitButton = FindViewById<ImageButton>(Resource.Id.AddUnitButton);
            AddUnitButton.Click += AddUnitButton_Click;
            UnitBackButton = FindViewById<ImageButton>(Resource.Id.UnitBackButton);
            UnitBackButton.Click += SwitchToContacts_Button;


            //Assign list view with adapter.
            UnitListView = FindViewById<ListView>(Resource.Id.UnitListView);
            UnitListView.ItemClick += UnitListView_ItemClick;
            UnitListView.Adapter = new UnitsAdapter(this, LoggedUser.UnitList);
        }

        private void SwitchToContacts_Button(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.Main);
            ListView contactListView = FindViewById<ListView>(Resource.Id.ListView);
            contactListView.ItemClick += List_ItemClick;
            contactListView.Adapter = new ContactsAdapter(this, LoggedUser.ContactList);
            AddContactButton = FindViewById<ImageButton>(Resource.Id.AddContactButton);
            AddContactButton.Click += AddNewContactPage;

            UnitPageButton = FindViewById<ImageButton>(Resource.Id.GoToUnitButton);
            UnitPageButton.Click += UnitsPage;
        }

        private void AssignMessages(List<RootObject> obj)
        {
            for (int i = 0; i < obj.Count; i++)
            {
                for (int j = 0; j < LoggedUser.ContactList.Count; j++)
                {
                    string Contact = obj[i].Contact;
                    Contact CurrentContact = LoggedUser.ContactList[j];
                    Console.WriteLine("Testing '" + Contact + "' against '" + LoggedUser.ContactList[j].Username + "'");
                    if (CurrentContact.Username == Contact)
                    {
                        Console.WriteLine("Assigned user '" + Contact + "' to '" + LoggedUser.ContactList[j].Username + "'");
                        LoggedUser.ContactList[j].messageList = obj[i].Messages;
                    }
                }
            }
        }


        private void LoadUnits()
        {
            int count = 0;
            foreach (Unit unit in LoggedUser.UnitList)
            {

                unit.ListPosition = count;
                //Console.WriteLine("User " + unit.Username + ", assigned to list position " + count);
                count++;
            }

            list = (ListView)FindViewById(Resource.Id.ListView);
            list.ItemClick += List_ItemClick;
            list.Adapter = new UnitsAdapter(this, LoggedUser.UnitList);

        }
      
        private void LoadContacts()
        {
            int count = 0;
            foreach (Contact contact in LoggedUser.ContactList)
            {
                
                contact.ListPosition = count;
                Console.WriteLine("User " + contact.Username + ", assigned to list position " + count);
                count++;
            }

            list = (ListView)FindViewById(Resource.Id.ListView);
            list.ItemClick += List_ItemClick;
            list.Adapter = new ContactsAdapter(this, LoggedUser.ContactList);

            AddContactButton = FindViewById<ImageButton>(Resource.Id.AddContactButton);
            AddContactButton.Click += AddNewContactPage;

        }

        private void AddNewContactPage(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.AddContact);

            AddNewContactButton = FindViewById<ImageButton>(Resource.Id.AddNewContactButton);
            ContactInput = FindViewById<EditText>(Resource.Id.ContactInput);
            AddContactBackButton = FindViewById<ImageButton>(Resource.Id.AddContactBackButton);

            AddNewContactButton.Click += AddNewContact;
            AddContactBackButton.Click += SwitchToContacts_Button;
        }

        private void AddNewContact(object sender, EventArgs e)
        {
            AddNewContactButton.Enabled = false;
            string contact = ContactInput.Text;
            if(contact != "")
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("Username", LoggedUser.Username);
                parameters.Add("Password", LoggedUser.Password);
                parameters.Add("Request", "FriendRequest");
                DbContact newContact = new DbContact();
                newContact.Username = contact;
                newContact.Id = "0";
                parameters.Add("Contact", JsonConvert.SerializeObject(newContact));

                mClient.UploadValuesAsync(mUri, parameters);
                mClient.UploadValuesCompleted += FriendRequestSent;
            } else
            {
                Toast.MakeText(this, "Please enter a contact name!", ToastLength.Long).Show(); ;
            }
        }

        private void FriendRequestSent(object sender, UploadValuesCompletedEventArgs e)
        {
            mClient.UploadValuesCompleted -= FriendRequestSent;
            string data = Encoding.UTF8.GetString(e.Result);
            Console.WriteLine(data);
            HttpResponse response = JsonConvert.DeserializeObject<HttpResponse>(data);
            if(response.Status == 400)
            {
                Console.WriteLine("Bad Request! Fuck off lel!");
                Toast.MakeText(this, "Failed to send request: " + response.Response, ToastLength.Long).Show();

                //Reset button and text...
                AddNewContactButton.Enabled = true;
                ContactInput.Text = "";
            } else
            {
                Console.WriteLine("Data response: " + response.Response);
                Toast.MakeText(this, "Friend Request Sent!", ToastLength.Short).Show();
                SwitchToContacts();
            }

        }

        private void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Console.WriteLine(e.Position);

            SetContentView(Resource.Layout.Messenger);

            SendButton = FindViewById<ImageButton>(Resource.Id.SendButton);
            SendButton.Click += SendButton_Click;

            BackButton = FindViewById<ImageButton>(Resource.Id.BackButton);
            BackButton.Click += SwitchToContacts_Button;

            ListView messageListView = FindViewById<ListView>(Resource.Id.messageListView);
            TextView contactTitle = FindViewById<TextView>(Resource.Id.contactNameHeader);
            contactTitle.Text = LoggedUser.ContactList[e.Position].Username;
            if (LoggedUser.ContactList[e.Position].messageList == null)
            {
                LoggedUser.ContactList[e.Position].messageList = new List<MessageObject>();
            }
            LoggedUser.ContactList[e.Position].ListPosition = e.Position;
            MessageAdapter adapter = new MessageAdapter(this, LoggedUser.ContactList[e.Position].messageList);
            messageListView.Adapter = adapter;
            messageListView.SetSelection(adapter.Count - 1);

            DbContact newContact = new DbContact();
            newContact.Username = LoggedUser.ContactList[e.Position].Username;
            newContact.Id = LoggedUser.ContactList[e.Position].Id;

            currentContact = LoggedUser.ContactList[e.Position];
            currentContactPosition = e.Position;

            RefreshButton = FindViewById<ImageButton>(Resource.Id.RefreshButton);
            RefreshButton.Click += RefreshButton_Click;

            MessengerLoader = FindViewById<ProgressBar>(Resource.Id.MessengerLoader);


        }

        private void SwitchToContacts()
        {
            SetContentView(Resource.Layout.Main);
            ListView contactListView = FindViewById<ListView>(Resource.Id.ListView);
            contactListView.ItemClick += List_ItemClick;
            contactListView.Adapter = new ContactsAdapter(this, LoggedUser.ContactList);
            AddContactButton = FindViewById<ImageButton>(Resource.Id.AddContactButton);
            AddContactButton.Click += AddNewContactPage;
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            EditText TextInput = FindViewById<EditText>(Resource.Id.MessageInput);
            string Message = TextInput.Text;
            if (Message != "")
            {
                Console.WriteLine(Message);
                string json_data = JsonConvert.SerializeObject(currentContact);
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("Username", LoggedUser.Username);
                parameters.Add("Password", LoggedUser.Password);
                parameters.Add("Request", "NewMessage");
                parameters.Add("Message", Message);
                parameters.Add("Contact", json_data);
                Console.WriteLine(json_data);
                mClient.UploadValuesCompleted += Message_Sent;
                mClient.UploadValuesAsync(mUri, parameters);

                BackButton.Enabled = false;
                RefreshButton.Enabled = false;
                SendButton.Enabled = false;
                MessengerLoader.Visibility = ViewStates.Visible;

            }
        }

        private void Message_Sent(object sender, UploadValuesCompletedEventArgs e)
        {
            
            mClient.UploadValuesCompleted -= Message_Sent;
            Console.WriteLine("MESSAGE SENT");
            EditText MessageInput = FindViewById<EditText>(Resource.Id.MessageInput);

            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(MessageInput.WindowToken, 0);

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("Username", LoggedUser.Username);
            parameters.Add("Password", LoggedUser.Password);
            parameters.Add("Request", "GetConversations");

            mClient.UploadValuesCompleted += ConversationRefreshComplete;
            mClient.UploadValuesAsync(mUri, parameters);

            

        }

        private void ConversationRefreshComplete(object sender, UploadValuesCompletedEventArgs e)
        {
            mClient.UploadValuesCompleted -= ConversationRefreshComplete;
            if (e.Result == null)
            {
                ServiceException();
                return;
            }
            string data = Encoding.UTF8.GetString(e.Result);
            Console.WriteLine(data);
            List<RootObject> obj = JsonConvert.DeserializeObject<List<RootObject>>(data);

            AssignMessages(obj);

            ListView messageListView = FindViewById<ListView>(Resource.Id.messageListView);
            Console.WriteLine("LIST POS: " + currentContact.ListPosition);
            //Console.WriteLine(LoggedUser.ContactList[currentContact.ListPosition].messageList[0].message);
            MessageAdapter adapter = new MessageAdapter(this, LoggedUser.ContactList[currentContact.ListPosition].messageList);
            messageListView.Adapter = adapter;
          
            messageListView.SetSelection(adapter.Count - 1);
            TextView messageInput = FindViewById<TextView>(Resource.Id.MessageInput);
            messageInput.Text = "";
            SendButton.Enabled = true;
            RefreshButton.Enabled = true;
            BackButton.Enabled = true;

            MessengerLoader.Visibility = ViewStates.Invisible;
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Console.WriteLine(mItems[e.Position]);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            loginButton.Visibility = ViewStates.Invisible;
            loginLoader.Visibility = ViewStates.Visible;
            Uri mUri = new Uri("http://arcane-headland-59258.herokuapp.com/PHPAPI.php");
            NameValueCollection parameters = new NameValueCollection();
            usernameInput = FindViewById<EditText>(Resource.Id.editText2);
            passwordInput = FindViewById<EditText>(Resource.Id.editText1);
            string Username = usernameInput.Text;
            string Password = passwordInput.Text;

            if(Username == "" || Password == "")
            {
                Toast.MakeText(this, "Please enter both a username and password!", ToastLength.Long).Show();
                return;
            }
            Console.WriteLine(Username);
            parameters.Add("Username", Username);
            parameters.Add("Password", Password);
            parameters.Add("Request", "Login");
            mClient.UploadValuesCompleted += LoginCompleted;
            mClient.UploadValuesAsync(mUri, parameters);
            loginButton.Enabled = false;



            Toast.MakeText(this, "Logging in...", ToastLength.Long).Show();  
            

            //SetContentView(Resource.Layout.Main);
        }

        private void MClient_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string data = "[" + Encoding.UTF8.GetString(e.Result) + "]";
            List<Contact> formattedData = JsonConvert.DeserializeObject<List<Contact>>(data);

            //Toast.MakeText(this, "Look!: You are an " + formattedData[0].Status, ToastLength.Long).Show();

            contactList.Add(formattedData[0]);

            SetContentView(Resource.Layout.Main);

        }

        private string CreateConversationString(string user1, string user2)
        {

            int index = 0;
            while(user1[index] == user2[index])
            {
                index++;

                if(index > user1.Length-1)
                {
                    return "conversation_" + user1 + "_" + user2;
                } 
                if(index > user2.Length-1)
                {
                    return "conversation_" + user2 + "_" + user1;
                }
            }

            if (Char.ToLower(user1[index]) < Char.ToLower(user2[index]))
            {
                return "conversation_" + user1 + "_" + user2;
            }
            else
            {
                return "conversation_" + user2 + "_" + user1;
            }

        }
    }


}

