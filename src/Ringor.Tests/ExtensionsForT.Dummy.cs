using FakeItEasy;

namespace Dalion.Ringor {
    public static partial class ExtensionsForT {
        public static T Dummy<T>(this T reference) {
            return A.Dummy<T>();
        }
    }
}