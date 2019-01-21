using System;

namespace Dalion.Ringor.Api.Models.Links {
    public interface IApplicationUriResolver {
        Uri Resolve();
    }
}