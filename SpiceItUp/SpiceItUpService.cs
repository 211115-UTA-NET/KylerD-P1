using SpiceItUp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    public class SpiceItUpService
    {
        private static readonly HttpClient HttpClient = new();

        Uri server = new("https://localhost:7106");

        public async Task<List<User>> GetUserFirstNameAsync(string firstName)
        {
            Uri requestUri = new(server, $"api/user/?firstName={firstName}");
            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            HttpResponseMessage response = await HttpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<List<User>>();

            return users;
        }
    }
}
