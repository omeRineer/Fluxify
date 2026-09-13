using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Common.Pagination
{
    public class PagedData<TEntity>
    {
        public int Page { get; init; }
        public int Size { get; init; }
        public long TotalCount { get; init; }
        public List<TEntity> Data { get; set; }
    }

}
