namespace KeylessAuthDotNetCore
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Helpers;

    public static class Group
    {
        [FunctionName("Group")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var provider = Auth.GetProvider();
            var accessToken = await provider.GetAccessTokenAsync("https://graph.microsoft.com");

            var httpClient = new HttpClient()
            {
                DefaultRequestHeaders = {
                    Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
                }
            };

            var result = await httpClient.GetStringAsync("https://graph.microsoft.com/v1.0/groups");

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(result,Encoding.UTF8,"application/json")
            };
        }
    }
}
