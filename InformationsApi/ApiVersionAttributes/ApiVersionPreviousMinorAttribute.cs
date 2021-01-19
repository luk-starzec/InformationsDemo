using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System;
using System.Reflection;

namespace InformationsApi.ApiVersionAttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ApiVersionPreviousMinorAttribute : ApiVersionsBaseAttribute, IApiVersionProvider
    {
        public ApiVersionPreviousMinorAttribute()
            : base(GenerateCurrentVersion())
        { }

        public ApiVersionProviderOptions Options => ApiVersionProviderOptions.None;


        static ApiVersion[] GenerateCurrentVersion()
        {
            var versionText = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            var version = versionText.Split(".");
            var major = int.Parse(version[0]);
            var minor = int.Parse(version[1]);
            var previousMinor = minor > 0 ? minor-- : 0;

            return new ApiVersion[] { new ApiVersion(major, previousMinor) };
        }
    }

}
