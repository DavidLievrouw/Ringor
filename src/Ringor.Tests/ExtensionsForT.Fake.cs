using FakeItEasy;

namespace Dalion.Ringor {
    public static partial class ExtensionsForT {
        public static T Fake<T>(this T reference) where T : class {
            return A.Fake<T>();
        }
    }
}