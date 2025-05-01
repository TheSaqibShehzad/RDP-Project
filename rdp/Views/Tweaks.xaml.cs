using Newtonsoft.Json.Linq;
using rdp.Auth;
using rdp.CustomClasses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace rdp.Views
{
    /// <summary>
    /// Interaction logic for Tweaks.xaml
    /// </summary>
    public partial class Tweaks : Page
    {
        public Tweaks()
        {
            InitializeComponent();
        }
        public static void ApiTest(string batchID, bool takesTime, bool isPowershell = true)
        {
            string apiUrl = $"{ApiConfig.BaseUrl}/api/v1/batch";

            // Prepare headers
            var authKey = HardwareInfo.GetAuthkey();
            var cipherAuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(authKey));
            var cipherBatchID = Convert.ToBase64String(Encoding.UTF8.GetBytes(batchID));

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", cipherAuthKey);
                client.DefaultRequestHeaders.TryAddWithoutValidation("BatchID", cipherBatchID);


                HttpResponseMessage response;
                try
                {
                    response = client.GetAsync(apiUrl).Result;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("HTTP request failed: " + ex.Message);
                    return;
                }

                int numericStatusCode = (int)response.StatusCode;
                string responseContent = response.Content.ReadAsStringAsync().Result;

                switch (numericStatusCode)
                {
                    case 400:
                    case 401:
                    case 500:
                        // show error
                        MessageBox.Show("fail");

                        return;

                    case 200:
                        JObject data = JObject.Parse(responseContent);
                        bool success = data["success"].Value<bool>();

                        if (success)
                        {
                            string batchContent = data["batchContent"].Value<string>();
                            string batchTitle = data["batchTitle"].Value<string>();
                            //batchContent += "\r\nexit";
                            Debug.WriteLine(batchContent);

                            var p = new Process
                            {
                                StartInfo = new ProcessStartInfo
                                {
                                    FileName = isPowershell ? "powershell.exe" : "cmd.exe",
                                    Arguments = isPowershell ? "-NoProfile -ExecutionPolicy Bypass -Command -" : "",
                                    Verb = "runas",
                                    UseShellExecute = false,
                                    RedirectStandardInput = true,
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    CreateNoWindow = false
                                }
                            };

                            p.Start();

                            p.StandardInput.WriteLine(batchContent);
                            p.StandardInput.Close();

                            p.WaitForExit();

                            string output = p.StandardOutput.ReadToEnd();
                            string error = p.StandardError.ReadToEnd();

                            if (p.ExitCode == 0 && string.IsNullOrEmpty(error))
                            {
                                Debug.WriteLine("Script executed successfully.");
                                Debug.WriteLine("Output: " + output);
                                MessageBox.Show("Success");
                            }
                            else
                            {
                                Debug.WriteLine("Script execution failed.");
                                Debug.WriteLine("Error: " + error);
                                MessageBox.Show("fail");
                                Debug.WriteLine("Exit Code: " + p.ExitCode);
                            }
                        }
                        return;

                    default:
                        // show error
                        MessageBox.Show("fail");
                        return;
                }
            }
        }





        public async static void DeleteBatch(string batchID, bool takesTime, bool isPowershell = true)
        {
            //var loadingScreen = new LoadingScreenTweaks();
            ApiToggleButton popup = new ApiToggleButton();

            //if (takesTime)
            //{
            //    loadingScreen.Show();
            //}

            //string sessionID = StandardUtilityFinal.Auth.api.sessionTokenAPI;
            string apiUrl = $"{ApiConfig.BaseUrl}/api/v1/deletebatch";
            var options = new RestClientOptions(apiUrl)
            {
                ThrowOnAnyError = false,
            };
            var client = new RestClient(options);

            // authkey
            var AuthKey = HardwareInfo.GetAuthkey();
            var plainTextAuthKey = System.Text.Encoding.UTF8.GetBytes(AuthKey);
            var cipherAuthKey = System.Convert.ToBase64String(plainTextAuthKey);
            client.AddDefaultHeader("Authorization", cipherAuthKey);

            // batchid
            var plainTextbatchID = System.Text.Encoding.UTF8.GetBytes(batchID);
            var cipherbatchID = System.Convert.ToBase64String(plainTextbatchID);
            client.AddDefaultHeader("BatchID", cipherbatchID);
            var request = new RestRequest();

            RestResponse response = await client.ExecuteGetAsync(request);
            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;

            switch (numericStatusCode)
            {
                case 400:
                    //if (takesTime)
                    //{
                    //    loadingScreen.Hide();
                    //}
                    //popup.ShowDeleteScriptFailedPopup();
                    return;

                case 401:
                    //if (takesTime)
                    //{
                    //    loadingScreen.Hide();
                    //}
                    //popup.ShowDeleteScriptFailedPopup();
                    return;

                case 500:
                    //if (takesTime)
                    //{
                    //    loadingScreen.Hide();
                    //}
                    //popup.ShowDeleteScriptFailedPopup();
                    return;

                case 200:
                    var data = System.Text.Json.JsonSerializer.Deserialize<JsonNode>(response.Content);
                    bool success = data["success"].GetValue<bool>();
                    if (success)
                    {
                        var batchContent = data["batchContent"].GetValue<string>();
                        var batchTitle = data["batchTitle"].GetValue<string>();
                        batchContent += "\r\nexit";
                        Debug.WriteLine(batchContent);

                        if (isPowershell)
                        {

                            //var p = new Process();

                            //p.StartInfo.FileName = "powershell.exe";
                            //p.StartInfo.Arguments = "-NoProfile -ExecutionPolicy Bypass -Command -"; // The '-' indicates that PowerShell will accept input from stdin
                            //p.StartInfo.Verb = "runas";
                            //p.StartInfo.UseShellExecute = false;
                            //p.StartInfo.RedirectStandardInput = true;
                            //p.StartInfo.RedirectStandardOutput = true;
                            //p.StartInfo.RedirectStandardError = true;
                            //p.StartInfo.CreateNoWindow = true;

                            //p.Start();

                            //p.StandardInput.WriteLine(batchContent);

                            //var output = await p.StandardOutput.ReadToEndAsync();
                            //var error = await p.StandardError.ReadToEndAsync();


                            var p = new Process
                            {
                                StartInfo = new ProcessStartInfo
                                {
                                    FileName = "powershell.exe",
                                    Arguments = "-NoProfile -ExecutionPolicy Bypass -Command -",
                                    Verb = "runas",
                                    UseShellExecute = false,
                                    RedirectStandardInput = true,
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    CreateNoWindow = true
                                }
                            };

                            p.Start();

                            // Write the batch content to PowerShell's input
                            await p.StandardInput.WriteLineAsync(batchContent);

                            // Close the input stream to signal that input is done
                            p.StandardInput.Close();

                            // Wait for the process to exit
                            //await p.WaitForExitAsync();

                            // Read the standard output and error
                            var output = await p.StandardOutput.ReadToEndAsync();
                            var error = await p.StandardError.ReadToEndAsync();

                            // Check the process exit code (0 means success)
                            if (p.ExitCode == 0 && string.IsNullOrEmpty(error))
                            {
                                // The script ran successfully
                                Debug.WriteLine("Script executed successfully.");
                                Debug.WriteLine("Output: " + output);
                            }
                            else
                            {
                                // There was an error during execution
                                Debug.WriteLine("Script execution failed.");
                                Debug.WriteLine("Error: " + error);
                                Debug.WriteLine("Exit Code: " + p.ExitCode);
                            }


                        }
                        else
                        {
                            //var p = new Process
                            //{
                            //    StartInfo = new ProcessStartInfo
                            //    {
                            //        FileName = "C:\\Windows\\System32\\cmd.exe",
                            //        Verb = "runas",
                            //        UseShellExecute = false,
                            //        RedirectStandardInput = true,
                            //        RedirectStandardOutput = true,
                            //        RedirectStandardError = true,
                            //        CreateNoWindow = true
                            //    }
                            //};

                            //p.Start();

                            //await p.StandardInput.WriteLineAsync(batchContent);
                            //p.StandardInput.Close();
                            //await p.WaitForExitAsync();

                            //var output = await p.StandardOutput.ReadToEndAsync();
                            //var error = await p.StandardError.ReadToEndAsync();
                        }

                        //if (takesTime)
                        //{
                        //    loadingScreen.Hide();
                        //}

                        //popup.ShowDeleteScriptSuccessPopup();
                    }
                    return;
                default:
                    //popup.ShowDeleteScriptFailedPopup();
                    return;
            }
        }
        private void Gauge_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Gauge_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void Gauge_Loaded_2(object sender, RoutedEventArgs e)
        {

        }

        private void UCMitigations_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
