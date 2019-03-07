namespace Dalion.Ringor.Utils {
    public static partial class Extensions {
        public static string DefaultIf(this string value, string toReplace = null, string replacement = null) {
            return Equals(toReplace, value)
                ? replacement
                : value;
        }
    }
}