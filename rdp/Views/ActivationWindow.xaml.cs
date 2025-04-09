using rdp.Auth;
using rdp.CustomClasses;
using KeyAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using rdp.DTO;

namespace rdp.Views
{
    /// <summary>
    /// Interaction logic for ActivationWindow.xaml
    /// </summary>
    public partial class ActivationWindow : Window
    {
        static string name = Creds.name;
        static string ownerID = Creds.ownerid;
        static string version = Creds.version;
        private static readonly string UserDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RDP-Project", "user_data.json");

        public static api KeyAuthApp = new api(name, ownerID, version);
        private readonly HttpClient client = new HttpClient();

        public ActivationWindow()
        {
            InitializeComponent();
            CheckForSavedLogin();
        }

        private void CheckForSavedLogin()
        {
            try
            {
                // Create directory if it doesn't exist
                string directory = Path.GetDirectoryName(UserDataPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Check if user data file exists
                if (File.Exists(UserDataPath))
                {
                    string json = File.ReadAllText(UserDataPath);
                    User savedUser = JsonConvert.DeserializeObject<User>(json);

                    if (savedUser != null && savedUser.IsActivated && !string.IsNullOrEmpty(savedUser.LicenseKey))
                    {
                        KeyAuthApp.init();
                        KeyAuthApp.license(savedUser.LicenseKey);
                        
                        if (KeyAuthApp.response.success)
                        {
                            // Open the main window directly
                            var nextWindow = new MainWindow();
                            nextWindow.Show();
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but continue with normal activation flow
                Debug.WriteLine($"Error checking saved login: {ex.Message}");
            }
        }

        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            try
            {
                dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
            }
            catch
            {
                dtDateTime = DateTime.MaxValue;
            }
            return dtDateTime;
        }
        private async void ActivateButton_Click(object sender, RoutedEventArgs e)
        {
            KeyAuthApp.init();
            KeyAuthApp.license(ActivationCodeTextBox.Text);
            if (KeyAuthApp.response.success)
            {
                Debug.WriteLine("\n Status: " + KeyAuthApp.response.message);
                Debug.WriteLine("\n Subscribed In!"); // at this point, the client has been authenticated. Put the code you want to run after here
                var data = KeyAuthApp.user_data;

                // Save user data to local storage
                try
                {
                    // Create directory if it doesn't exist
                    string directory = Path.GetDirectoryName(UserDataPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    User user = new User
                    {
                        Username = data.username,
                        Ip = data.ip,
                        Hwid = data.hwid,
                        CreateDate = data.createdate,
                        LastLogin = data.lastlogin,
                        LicenseKey = ActivationCodeTextBox.Text,
                        IsActivated = true
                    };

                    // Add subscription data if available
                    if (data.subscriptions != null && data.subscriptions.Count > 0)
                    {
                        var subscription = data.subscriptions[0];
                        if (!string.IsNullOrEmpty(subscription.expiry))
                        {
                            if (long.TryParse(subscription.expiry, out long expiryUnix))
                            {
                                user.ExpiryDate = UnixTimeToDateTime(expiryUnix);
                            }
                        }
                    }

                    string json = JsonConvert.SerializeObject(user, Formatting.Indented);
                    File.WriteAllText(UserDataPath, json);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error saving user data: {ex.Message}");
                }

                // Open the next window
                var nextWindow = new MainWindow(); // Replace 'NextWindow' with your actual window class
                nextWindow.Show();

                // Close the current window
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed. Please make sure that the key is valid.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
