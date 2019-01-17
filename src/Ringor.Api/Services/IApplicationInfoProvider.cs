using Dalion.Ringor.Api.Models;

namespace Dalion.Ringor.Api.Services {
    public interface IApplicationInfoProvider {
        ApplicationInfo Provide();
    }
}