using Azure.Core;
using Azure.Identity;
using KeylessAuthDotNetCore.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KeylessAuthDotNetCore
{
    public static class GroupsAzureIdentity
    {
        [FunctionName("GroupsAzureIdentity")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get Groups using Azure.Identity...");

#if DEBUG
            log.LogInformation("Using client credentials");
            X509Certificate2 cert = Certs.GetCertificateFromStore(Configuration.LocalDevCertName);
            var credential = new ClientCertificateCredential(Configuration.LocalDevTenantId, Configuration.LocalDevAppId, cert);
#else
            log.LogInformation("Using Managed identity credentials");
            var credential = new ManagedIdentityCredential();
#endif

            try
            {
                var accessTokenRequest = await credential.GetTokenAsync(
                    new TokenRequestContext(scopes: new string[] { "https://graph.microsoft.com/.default" }) { }
                    );

                var accessToken = accessTokenRequest.Token;

                log.LogInformation(accessToken);

                var httpClient = new HttpClient()
                {
                    DefaultRequestHeaders = {
                    Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
                }
                };

                var result = await httpClient.GetStringAsync("https://graph.microsoft.com/v1.0/groups");

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(result, Encoding.UTF8, "application/json")
                };
            }
            catch (AuthenticationFailedException ex)
            {
                log.LogError($"Authentication Failed. {ex.Message}");
                return null;
            }
        }
    }
}

