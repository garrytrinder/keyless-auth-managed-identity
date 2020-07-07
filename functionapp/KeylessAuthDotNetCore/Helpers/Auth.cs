namespace KeylessAuthDotNetCore.Helpers
{
    using Microsoft.Azure.Services.AppAuthentication;

    internal static class Auth
    {
        public static AzureServiceTokenProvider GetProvider()
        {
            var connectionString = "RunAs=App";
#if DEBUG
            connectionString = $"RunAs=App;AppId={Configuration.LocalDevAppId};TenantId={Configuration.LocalDevTenantId};CertificateThumbprint={Configuration.LocalDevThumbprint};CertificateStoreLocation=CurrentUser";
#endif
            return new AzureServiceTokenProvider(connectionString);
        }
    }
}
