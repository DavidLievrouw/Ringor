namespace Dalion.Ringor.Api.Models.Links {
    public interface IClaimLinksCreatorFactory {
        ILinksCreator<Claim> Create();
    }
}