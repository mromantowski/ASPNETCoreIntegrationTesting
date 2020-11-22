using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductionService.Client
{
    public class ProductionServiceClient : IProductionServiceClient
    {
        private readonly IRestClient client;

        public ProductionServiceClient(string url)
        {
            this.client = new RestClient(url);
            client.UseNewtonsoftJson();
        }
        public async Task<GetActionsResponse> GetActions(string machine, DateTime day)
            => await GetActions(machine, day, CancellationToken.None);

        public async Task<GetActionsResponse> GetActions(string machine, DateTime day, CancellationToken cancellationToken)
        {
            var queryParameters = new Dictionary<string, string>
            {
                {  "Machine", machine },
                { "Day", day.ToString("yyyy-MM-dd") }
            };
            
            return await GetAsync<GetActionsResponse>("/api/actions", cancellationToken, queryParameters);
        }

        private async Task<TResponse> GetAsync<TResponse>(string path, CancellationToken cancellationToken, IDictionary<string, string> queryParameters = null) where TResponse : Response, new()
        {
            try
            {
                var restRequest = new RestRequest(path, Method.GET);
                if (queryParameters != null)
                {
                    foreach (var key in queryParameters.Keys)
                    {
                        restRequest.AddQueryParameter(key, queryParameters[key]);
                    }
                }
                var restResponse = await client.ExecuteAsync<TResponse>(restRequest, cancellationToken);

                return restResponse.IsSuccessful
                    ? restResponse.Data
                    : new TResponse { IsSuccessful = false, ErrorMessage = restResponse.ErrorMessage };
            } 
            catch(Exception e)
            {
                return new TResponse { IsSuccessful = false, ErrorMessage = e.Message };
            }
        }
    }
}
