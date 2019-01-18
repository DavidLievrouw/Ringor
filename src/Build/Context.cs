using Cake.Core;
using Cake.Frosting;
using Dalion.Ringor.Build.Properties;

namespace Dalion.Ringor.Build {
    public class Context : FrostingContext {
        public Context(ICakeContext context) : base(context) {
            Ringor = new RingorProperties(context);
        }

        public RingorProperties Ringor { get; }
    }
}