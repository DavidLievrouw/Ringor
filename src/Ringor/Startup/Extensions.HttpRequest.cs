using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Dalion.Ringor.Startup {
    internal static partial class Extensions {
        public static string GetRawSecurityToken(this HttpRequest request) {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var authorization = request.Headers["Authorization"];
            var firstBearerToken = authorization.FirstOrDefault(a => a.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase));

            return string.IsNullOrEmpty(firstBearerToken)
                ? null
                : firstBearerToken.Substring("Bearer ".Length).Trim();
        }
    }
}