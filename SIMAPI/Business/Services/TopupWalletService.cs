using Newtonsoft.Json;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SIMAPI.Business.Services
{
    public class TopupWalletService : ITopupWalletService
    {
        private readonly HttpClient _httpClient;
        public TopupWalletService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("LeapTel");
            _httpClient.BaseAddress = new Uri("https://leap-tel.co.uk/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var loginData = new TopupLoginRequest { username = username, password = password };
            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("transaction/login/", content);

            if (!response.IsSuccessStatusCode)
                return "Error";

            var result = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(result);
            return loginResponse.data.access_token;
        }

        public async Task<string> UpdateBalanceAsync(string token, BalanceUpdateRequest balanceUpdate)
        {
            // Serialize the request body as JSON
            var jsonBody = JsonConvert.SerializeObject(balanceUpdate);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Create the HTTP POST request
            var request = new HttpRequestMessage(HttpMethod.Post, "transaction/update-agency-balance/")
            {
                Content = content
            };

            // Add the Bearer token to the Authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Send the request
            var response = await _httpClient.SendAsync(request);

            // Return either the response content or error
            if (!response.IsSuccessStatusCode)
                return $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";

            return await response.Content.ReadAsStringAsync();
        }
    }

    public class TopupLoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class LoginResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public LoginData data { get; set; } // or access_token based on API response
    }

   

    public class LoginData
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
