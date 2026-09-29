using System.Net.Http.Headers;
using System.Text;

namespace InventoryManagerApp
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient;

        public MainPage()
        {
            InitializeComponent();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5072/")
            };
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string userName = txtUserName.Text ?? "";
            string password = txtPassword.Text ?? "";

            if (string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlertAsync(
                    "Error",
                    "Please enter a User Name and Password.",
                    "OK");

                return;
            }

            try
            {
                string credentials = $"{userName}:{password}";

                string encodedCredentials =
                    Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(credentials));

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic",
                        encodedCredentials);

                HttpResponseMessage response =
                    await _httpClient.GetAsync("api/Authentication");

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlertAsync(
                        "Success",
                        "Login Successful",
                        "OK");

                    await Shell.Current.GoToAsync(nameof(DataEntryPage));
                }
                else
                {
                    await DisplayAlertAsync(
                        "Error",
                        "Invalid User ID or Password",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync(
                    "Connection Error",
                    $"Unable to connect to the web service: {ex.Message}",
                    "OK");
            }
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
        }
    }
}