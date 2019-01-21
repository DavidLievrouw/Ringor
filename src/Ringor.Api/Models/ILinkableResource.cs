using System;

namespace Dalion.Ringor.Api.Models {
    public interface ILinkableResource<TRel> where TRel : struct, IConvertible {
        Hyperlink<TRel>[] Links { get; set; }
    }
}