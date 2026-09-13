using Fluxify.Application.Models.Common.Pagination;

namespace Fluxify.Application.Models.Rest.Common
{
    public class PagedDataResponse<TData>
    {
        public long TotalCount { get; set; }
        public IEnumerable<TData> Data { get; set; }

        public static PagedDataResponse<TData> Load(IEnumerable<TData> data, long totalCount)
            => new PagedDataResponse<TData>
            {
                TotalCount = totalCount,
                Data = data
            };
    }
}
