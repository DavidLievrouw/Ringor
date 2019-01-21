using System.Threading.Tasks;

namespace Dalion.Ringor.Api.Models.Links {
    public interface ILinksCreator<in TModel> {
        Task CreateLinksFor(TModel model);
    }
}