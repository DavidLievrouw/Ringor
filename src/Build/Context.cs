using Cake.Core;
using Cake.Frosting;
using Dalion.Ringor.Build.Configuration;

namespace Dalion.Ringor.Build {
    public class Context : FrostingContext {
        public Context(ICakeContext context) : base(context) {
            App = new AppProperties(context);
        }

        public AppProperties App { get; }
    }
}