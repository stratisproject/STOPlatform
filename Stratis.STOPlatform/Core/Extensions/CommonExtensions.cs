using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Extensions
{
    public static class CommonExtensions
    {
        public static T As<T>(this IConvertible value, IFormatProvider format = null)
        {
            var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            if (value == null || value.Equals(""))
                return default(T);

            return (T)value.ToType(type, format ?? CultureInfo.CurrentCulture);
        }

        public static bool IsAjaxRequest(this IDictionary<string, StringValues> headers)
        {
            headers.TryGetValue("x-requested-with", out StringValues value);

            return value == "XMLHttpRequest";
        }

        public static string GetFullName(this ClaimsPrincipal user)
        {
            var fullname = user.FindFirst(ClaimTypes.Name)?.Value;
            if (fullname == null)
                return null;

            return CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(fullname);
        }
    }
}
