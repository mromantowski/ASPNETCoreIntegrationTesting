﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using ProductionService.Model;

namespace ProductionService.IntegrationTests.TestHost
{
    public abstract class IntegrationTestsBase : IDisposable
    {
        private readonly TestServer testServer;
        private readonly HttpClient client;
        private readonly JsonSerializerOptions jsonOptions;

        private bool disposed = false;

        public IntegrationTestsBase()
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var testCutsPath = Path.Combine(basePath, "Files", "Cuts");

            testServer = new TestServer(
                new WebHostBuilder()
                .UseStartup<StartupBase>()
                .ConfigureServices((c,s) => s.AddCuts(testCutsPath)));

            client = testServer.CreateClient();

            jsonOptions = new JsonSerializerOptions();
            jsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }

        protected async Task<TResponse> GetAsync<TResponse>(string path, object query = null)
        {
            var response = await client.GetAsync(BuildUri(client.BaseAddress, path, CreateQuery(query)));
            Stream responseStream = null;
            try
            {
                responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<TResponse>(responseStream, jsonOptions);
            }
            finally
            {
                responseStream?.Dispose();
            }
        }
        
        private Uri BuildUri(Uri baseAddress, string path, string query = null)
        {
            var builder = new UriBuilder(baseAddress);
            builder.Path = baseAddress.AbsolutePath.TrimEnd('/') + '/' + path.TrimStart('/');
            
            if (query != null)
            {
                builder.Query = query;
            }

            return builder.Uri;
        }

        private string CreateQuery(object o)
        {
            if (o == null)
            {
                return string.Empty;
            }

            var parameters = o
                .GetType()
                .GetProperties()
                .Select(p => new { p.Name, Value = p.GetValue(o, null) })
                .Where(p => p.Value != null)
                .Select(p => $"{p.Name}={ HttpUtility.UrlEncode(p.Value.ToString())}")
                .ToArray();

            return string.Join('&', parameters);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                testServer.Dispose();
                client.Dispose();
            }

            disposed = true;
        }
    }
}
