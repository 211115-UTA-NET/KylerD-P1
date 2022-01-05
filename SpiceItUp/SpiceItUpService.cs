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

        public Task<List<User>> GetUserFirstName(string firstName)
        {
            Uri requestUri = new(server, $"/user/FirstName?firstName={firstName}");
            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            HttpResponseMessage response;
            
            try
            {
                response = HttpClient.Send(request);
            }
            catch (HttpRequestException ex)
            {
                throw new("Network error", ex);
            }
            
            response.EnsureSuccessStatusCode();

            var users = response.Content.ReadFromJsonAsync<List<User>>();

            return users;
        }

        public Task<List<User>> GetUserLastName(string lastName)
        {
            Uri requestUri = new(server, $"/user/LastName?lastName={lastName}");
            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            HttpResponseMessage response;

            try
            {
                response = HttpClient.Send(request);
            }
            catch (HttpRequestException ex)
            {
                throw new("Network error", ex);
            }

            response.EnsureSuccessStatusCode();

            var users = response.Content.ReadFromJsonAsync<List<User>>();

            return users;
        }

        public Task<List<User>> GetCustomerList()
        {
            Uri requestUri = new(server, $"/user");
            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            HttpResponseMessage response;

            try
            {
                response = HttpClient.Send(request);
            }
            catch (HttpRequestException ex)
            {
                throw new("Network error", ex);
            }

            response.EnsureSuccessStatusCode();

            var users = response.Content.ReadFromJsonAsync<List<User>>();

            return users;
        }

        public Task<List<Transaction>> GetCustomerTransactionList(int customerID)
        {
            Uri requestUri = new(server, $"/transaction/customer?id={customerID}");
            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            HttpResponseMessage response;

            try
            {
                response = HttpClient.Send(request);
            }
            catch (HttpRequestException ex)
            {
                throw new("Network error", ex);
            }

            response.EnsureSuccessStatusCode();

            var transactions = response.Content.ReadFromJsonAsync<List<Transaction>>();

            return transactions;
        }

        public Task<List<Transaction>> DetailedTransaction(string transID)
        {
            Uri requestUri = new(server, $"/transaction/transID?transID={transID}");
            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            HttpResponseMessage response;

            try
            {
                response = HttpClient.Send(request);
            }
            catch (HttpRequestException ex)
            {
                throw new("Network error", ex);
            }

            response.EnsureSuccessStatusCode();

            var transaction = response.Content.ReadFromJsonAsync<List<Transaction>>();

            return transaction;
        }
    }
}
