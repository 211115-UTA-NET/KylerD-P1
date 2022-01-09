using Microsoft.AspNetCore.WebUtilities;
using SpiceItUp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUp
{
    public class SpiceItUpService
    {
        private readonly HttpClient HttpClient = new();

        public SpiceItUpService(Uri serverUri)
        {
            HttpClient.BaseAddress = serverUri;
        }

        public Task<List<User>> GetUserFirstName(string firstName)
        {
            Dictionary<string, string> query = new() { ["firstName"] = firstName };
            string requestUri = QueryHelpers.AddQueryString("/user/FirstName", query);

            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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
            Dictionary<string, string> query = new() { ["lastName"] = lastName };
            string requestUri = QueryHelpers.AddQueryString("/user/LastName", query);

            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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
            string requestUri = "/user";
            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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
            string custID = customerID.ToString();

            Dictionary<string, string> query = new() { ["id"] = custID };
            string requestUri = QueryHelpers.AddQueryString("/transaction/customer", query);

            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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
            Dictionary<string, string> query = new() { ["transID"] = transID };
            string requestUri = QueryHelpers.AddQueryString("/transaction/transID", query);

            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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
            string requestUri = "/store";
            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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
            string id = storeID.ToString();

            Dictionary<string, string> query = new() { ["storeID"] = id };
            string requestUri = QueryHelpers.AddQueryString("/store/inventory", query);

            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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
            string id = storeID.ToString();

            Dictionary<string, string> query = new() { ["storeID"] = id };
            string requestUri = QueryHelpers.AddQueryString("/transaction/storeID", query);

            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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
            Dictionary<string, string> query = new() { ["username"] = username, ["password"] = password };
            string requestUri = QueryHelpers.AddQueryString("/user/Login", query);

            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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
            string newID = id.ToString();

            Dictionary<string, string> query = new() { ["id"] = newID };
            string requestUri = QueryHelpers.AddQueryString("/user/ID", query);

            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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

        public void PostUserInfo(string? user, string? pass, string? first, string? last, string? phone)
        {
            Dictionary<string, string?> query = new() { ["username"] = user, ["password"] = pass, ["firstName"] = first, ["lastName"] = last, ["phoneNumber"] = phone };
            string requestUri = QueryHelpers.AddQueryString("/newuser", query);

            HttpRequestMessage request = new(HttpMethod.Post, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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
            string newID = id.ToString();

            Dictionary<string, string> query = new() { ["storeID"] = newID };
            string requestUri = QueryHelpers.AddQueryString("/storeinfo", query);

            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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
            string newID = id.ToString();

            Dictionary<string, string> query = new() { ["storeID"] = newID };
            string requestUri = QueryHelpers.AddQueryString("/storecart", query);

            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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

        public void PostNewStoreInventory(int inStockListNew, int storeEntry, int itemIDListNew)
        {
            string inStock = inStockListNew.ToString();
            string store = storeEntry.ToString();
            string itemID = itemIDListNew.ToString();

            Dictionary<string, string> query = new() { ["inStockListNew"] = inStock, ["storeEntry"] = store, ["itemIDListNew"] = itemID };
            string requestUri = QueryHelpers.AddQueryString("/newinventory", query);

            HttpRequestMessage request = new(HttpMethod.Post, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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

        public void PostNewTransaction(string transID, int userID, int storeEntry)
        {
            string trans = transID.ToString();
            string user = userID.ToString();
            string store = storeEntry.ToString();

            Dictionary<string, string> query = new() { ["transID"] = trans, ["userID"] = user, ["storeEntry"] = store };
            string requestUri = QueryHelpers.AddQueryString("/newtransaction", query);

            HttpRequestMessage request = new(HttpMethod.Post, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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

        public void PostNewTransactionDetails(string transID, int customerItemIDNew, int customerQuantityNew, decimal customerPriceNew)
        {
            string itemID = customerItemIDNew.ToString();
            string quantity = customerQuantityNew.ToString();
            string price = customerPriceNew.ToString();

            Dictionary<string, string> query = new() { ["transID"] = transID, ["customerItemIDNew"] = itemID, ["customerQuantityNew"] = quantity, ["customerPriceNew"] = price };
            string requestUri = QueryHelpers.AddQueryString("/newtransactiondetails", query);

            HttpRequestMessage request = new(HttpMethod.Post, requestUri);
            request.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
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
    }
}
