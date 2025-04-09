using rdp.Auth;
using rdp.DTO;
using rdp.Views;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System;

namespace rdp.CustomClasses
{
    public class ApiToggleButton : ToggleButton
    {
        private readonly HttpClient client = new HttpClient();

        public ApiToggleButton()
        {
            this.Checked += OnToggleChecked;
        }

        private async void OnToggleChecked(object sender, RoutedEventArgs e)
        {
            //var data = false;
            //var home = Application.Current.Windows.OfType<HomeScreen>().FirstOrDefault();
            //if (home != null)
            //{
            //    data = home.isDataFetched;
            //}

            //if (!UserSession.IsPremiumUser && data)
            //{
                string apiUrl = $"{ApiConfig.BaseUrl}/api/v1/toggle";
                string authKey = HardwareInfo.GetAuthkey();
                var content = new StringContent($"{{\"authKey\":\"{authKey}\"}}", System.Text.Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<User>>(result);
                        if (apiResponse != null && apiResponse.Status == "success")
                        {
                            UserSession.RemainingTweaks = apiResponse.Data.FreeTweaks;
                            if (apiResponse.Data.FreeTweaks <= 0)
                            {
                                //EnableDisableToggleButtons();
                                //ShowFreeTweaksNotAvailablePopup();
                            }
                            else
                            {
                                //ShowRemainingFreeTweaksPopup(apiResponse.Data.FreeTweaks);
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Error occured at OnToggleChecked");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception occured at OnToggleChecked");
                }
            //}
        }







       
    }

}
