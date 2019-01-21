using System.Collections.Generic;

namespace Dalion.Ringor.ViewModels {
    public class IndexViewModel {
        public IEnumerable<string> Styles { get; set; }
        public IEnumerable<string> Scripts { get; set; }
    }
}