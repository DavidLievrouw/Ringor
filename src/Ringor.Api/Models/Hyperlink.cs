using System;
using System.Net.Http;

namespace Dalion.Ringor.Api.Models {
    public class Hyperlink<TRel> : IEquatable<Hyperlink<TRel>>
        where TRel : struct, IConvertible {

        public Hyperlink(HttpMethod method, string href, TRel rel) {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!typeof(TRel).IsEnum) throw new ArgumentException("TRel must be an enum type.");
            if (!Enum.IsDefined(typeof(TRel), rel)) throw new ArgumentOutOfRangeException(nameof(rel));
            Method = method.Method;
            Href = string.IsNullOrWhiteSpace(href)
                ? null
                : href;
            Rel = string.IsNullOrWhiteSpace(href)
                ? default(TRel)
                : rel;
        }
        
        public string Method { get; }

        public string Href { get; }

        public TRel Rel { get; }

        public bool Equals(Hyperlink<TRel> other) {
            return !ReferenceEquals(null, other) && Rel.Equals(other.Rel);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Hyperlink<TRel> hyperlink && Equals(hyperlink);
        }

        public override int GetHashCode() {
            return Rel.GetHashCode();
        }

        public static bool operator ==(Hyperlink<TRel> left, Hyperlink<TRel> right) {
            return Equals(left, right);
        }

        public static bool operator !=(Hyperlink<TRel> left, Hyperlink<TRel> right) {
            return !Equals(left, right);
        }
    }
}