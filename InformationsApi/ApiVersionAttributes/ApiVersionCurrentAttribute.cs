using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System;
using System.Reflection;

namespace InformationsApi.ApiVersionAttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ApiVersionCurrentAttribute : ApiVersionsBaseAttribute, IApiVersionProvider
    {
        public ApiVersionCurrentAttribute()
            : base(GenerateCurrentVersion())
        { }

        public ApiVersionProviderOptions Options => ApiVersionProviderOptions.None;


        static ApiVersion[] GenerateCurrentVersion()
        {
            var versionText = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            var version = versionText.Split(".");
            var major = int.Parse(version[0]);
            var minor = int.Parse(version[1]);

            return new ApiVersion[] { new ApiVersion(major, minor) };
        }
    }
}
