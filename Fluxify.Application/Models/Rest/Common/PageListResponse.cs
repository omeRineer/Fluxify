using Fluxify.Application.Models.Common.Pagination;

namespace Fluxify.Application.Models.Rest.Common
{
    public class PageListResponse<TData>
    {
        public long TotalCount { get; set; }
        public List<TData> Data { get; set; }
    }
}
