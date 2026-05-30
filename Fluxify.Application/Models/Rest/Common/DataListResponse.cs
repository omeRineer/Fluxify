using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.Common
{
    public class DataListResponse<TData>
    {
        public int Count => Items.Count;
        public IReadOnlyList<TData> Items { get; private set; } = new List<TData>();

        public static DataListResponse<TData> Load(List<TData> items)
        {
            return new DataListResponse<TData>
            {
                Items = items.AsReadOnly()
            };
        }
    }
}
