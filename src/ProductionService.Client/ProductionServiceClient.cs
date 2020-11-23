using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
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

        public async Task<GetCutTaskDetailsResponse> GetCutTaskDetails(string taskId)
            => await GetCutTaskDetails(taskId, CancellationToken.None);

        public async Task<GetCutTaskDetailsResponse> GetCutTaskDetails(string taskId, CancellationToken cancellationToken)
            => await GetAsync<GetCutTaskDetailsResponse>($"api/cuts/{taskId}", cancellationToken);

        public async Task<Response> MarkTaskCompleted(string taskId)
            => await MarkTaskCompleted(taskId, CancellationToken.None);

        public async Task<Response> MarkTaskCompleted(string taskId, CancellationToken cancellationToken)
            => await PostAsync<Response>("/api/cuts/completed", new { TaskId = taskId }, cancellationToken);

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

        private async Task<TResponse> PostAsync<TResponse>(string path, object request, CancellationToken cancellationToken) where TResponse : Response, new()
        {
            try
            {
                var restRequest = new RestRequest(path, Method.POST);
                restRequest.AddJsonBody(request);
                var restResponse = await client.ExecuteAsync<TResponse>(restRequest, cancellationToken);

                return restResponse.IsSuccessful
                    ? restResponse.Data
                    : new TResponse { IsSuccessful = false, ErrorMessage = restResponse.ErrorMessage };
            }
            catch (Exception e)
            {
                return new TResponse { IsSuccessful = false, ErrorMessage = e.Message };
            }
        }
    }
}
