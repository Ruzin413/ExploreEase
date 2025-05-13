using Models.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Services.Services
{
    public class KhaltiService
    {
        private readonly HttpClient _httpClient;

        public KhaltiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> InitiatePaymentAsync(KhaltiRequest request)
        {
            var url = "https://dev.khalti.com/api/v2/epayment/initiate/";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            // ✅ Use this instead of AuthenticationHeaderValue
            requestMessage.Headers.TryAddWithoutValidation("Authorization", "test_secret_key_3b23b2bd112044b4a194710a3ad94c4f");

            var json = JsonConvert.SerializeObject(request);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(requestMessage);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine(result); // Log output for debugging

            return response.IsSuccessStatusCode ? result : null;
        }
    }
}
