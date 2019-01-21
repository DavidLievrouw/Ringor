using System;

namespace Dalion.Ringor.Api {
    public static partial class Extensions {
        public static Uri WithRelativePath(this Uri baseUri, string relativePath) {
            if (baseUri == null) throw new ArgumentNullException(nameof(baseUri));
            if (relativePath == null) relativePath = string.Empty;
            if (!baseUri.IsAbsoluteUri) throw new InvalidOperationException("The base uri should be an absolute uri.");
            relativePath = relativePath.StartsWith("/")
                ? relativePath.Substring(1)
                : relativePath;
            return new Uri(baseUri, baseUri.AbsolutePath + (baseUri.AbsolutePath.EndsWith("/")
                                        ? string.Empty
                                        : "/") + relativePath);
        }

        public static Uri WithRelativePath(this Uri baseUri, Uri relativeUri) {
            if (baseUri == null) throw new ArgumentNullException(nameof(baseUri));
            if (relativeUri == null) relativeUri = new Uri(string.Empty, UriKind.Relative);
            if (!baseUri.IsAbsoluteUri) throw new InvalidOperationException("The base uri should be an absolute uri.");
            if (relativeUri.IsAbsoluteUri) throw new InvalidOperationException("The relative uri should be a relative uri, not an absolute uri.");
            baseUri = new Uri(baseUri.OriginalString.EndsWith("/")
                ? baseUri.OriginalString
                : baseUri.OriginalString + "/", UriKind.Absolute);
            relativeUri = new Uri(relativeUri.OriginalString.StartsWith("/")
                ? relativeUri.OriginalString.Substring(1)
                : relativeUri.OriginalString, UriKind.Relative);
            return new Uri(baseUri, relativeUri);
        }
    }
}