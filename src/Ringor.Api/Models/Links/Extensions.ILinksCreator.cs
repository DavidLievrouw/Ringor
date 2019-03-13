using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dalion.Ringor.Utils;

namespace Dalion.Ringor.Api.Models.Links {
    public static partial class Extensions {
        public static async Task CreateLinksFor<TModel>(this ILinksCreator<TModel> linksCreator, IEnumerable<TModel> models) {
            if (models == null) throw new ArgumentNullException(nameof(models));
            await models.ForEach(async model => await linksCreator.CreateLinksFor(model));
        }
    }
}