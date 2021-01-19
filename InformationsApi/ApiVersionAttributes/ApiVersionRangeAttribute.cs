using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System;
using System.Collections.Generic;

namespace InformationsApi.ApiVersionAtributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ApiVersionRangeAttribute : ApiVersionsBaseAttribute, IApiVersionProvider
    {
        public ApiVersionRangeAttribute(int majorStart, int majorEnd)
            : base(GenerateRange(majorStart, majorEnd))
        { }

        public ApiVersionRangeAttribute(int major, int minorStart, int minorEnd)
            : base(GenerateRange(major, minorStart, minorEnd))
        { }

        public ApiVersionProviderOptions Options => ApiVersionProviderOptions.None;


        static ApiVersion[] GenerateRange(int majorStart, int majorEnd)
        {
            var versions = new List<ApiVersion>();
            for (var major = majorStart; major <= majorEnd; major++)
            {
                versions.Add(new ApiVersion(major, 0));
            }
            return versions.ToArray();
        }
        static ApiVersion[] GenerateRange(int major, int minorStart, int minorEnd)
        {
            var versions = new List<ApiVersion>();
            for (var minor = minorStart; minor <= minorEnd; minor++)
            {
                versions.Add(new ApiVersion(major, minor));
            }
            return versions.ToArray();
        }
    }
}
