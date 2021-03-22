

using System.ComponentModel;

namespace KeylessAuthDotNetCore.Helpers
{
    using System;

    internal static class Configuration
    {
        public static string LocalDevAppId => GetProperty<string>("LocalDevAppId");
        public static string LocalDevTenantId => GetProperty<string>("LocalDevTenantId");
        public static string LocalDevThumbprint => GetProperty<string>("LocalDevThumbprint");
        public static string LocalDevCertName => GetProperty<string>("LocalDevCertName"); //CN=KeylessAuthLocalDev

        private static T GetProperty<T>(string key)
        {
            var appSetting = Environment.GetEnvironmentVariable(key);

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)(converter.ConvertFromInvariantString(appSetting));
        }
    }
}
