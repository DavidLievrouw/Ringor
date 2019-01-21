namespace Dalion.Ringor.Api.Models.Links {
    public interface IApiHomeResponseLinksCreatorFactory {
        ILinksCreator<ApiHomeResponse> Create();
    }
}