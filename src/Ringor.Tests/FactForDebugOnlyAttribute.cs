using System.Diagnostics;
using Xunit;

namespace Dalion.Ringor {
    public sealed class FactForDebugOnlyAttribute : FactAttribute {
        public FactForDebugOnlyAttribute() {
            if (!Debugger.IsAttached) {
                Skip = "Only running in interactive mode.";
            }
        }
    }
}