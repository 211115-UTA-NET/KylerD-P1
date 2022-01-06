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

        public Task<List<Store>> GetStoreList()
        {
            Uri requestUri = new(server, $"/store");
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

            var stores = response.Content.ReadFromJsonAsync<List<Store>>();

            return stores;
        }

        public Task<List<Store>> GetStoreInventory(int storeID)
        {
            Uri requestUri = new(server, $"/store/inventory?storeID={storeID}");
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

            var inventory = response.Content.ReadFromJsonAsync<List<Store>>();

            return inventory;
        }

        public Task<List<Transaction>> GetStoreTransactionList(int storeID)
        {
            Uri requestUri = new(server, $"/transaction/storeID?storeID={storeID}");
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

        public Task<List<User>> GetLoginInfo(string username, string password)
        {
            Uri requestUri = new(server, $"/user/Login?username={username}&password={password}");
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

            var login = response.Content.ReadFromJsonAsync<List<User>>();

            return login;
        }

        public Task<List<User>> GetUserInfo(int id)
        {
            Uri requestUri = new(server, $"/user/ID?id={id}");
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

            var user = response.Content.ReadFromJsonAsync<List<User>>();

            return user;
        }

        public void PostUserInfo(string user, string pass, string first, string last, string phone)
        {
            Uri requestUri = new(server, $"/newuser?username={user}&password={pass}&firstName={first}&lastName={last}&phoneNumber={phone}");
            HttpRequestMessage request = new(HttpMethod.Post, requestUri);
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
        }

        public Task<List<Store>> GetStoreName(int id)
        {
            Uri requestUri = new(server, $"/storeinfo?storeID={id}");
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

            var user = response.Content.ReadFromJsonAsync<List<Store>>();

            return user;
        }

        public Task<List<Store>> GetCartStoreInventory(int id)
        {
            Uri requestUri = new(server, $"/storecart?storeID={id}");
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

            var user = response.Content.ReadFromJsonAsync<List<Store>>();

            return user;
        }
    }
}
