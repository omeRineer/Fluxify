using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Common
{
    public class CollectionResult<TData>
    {
        public int Count => List.Count;

        public IReadOnlyList<TData> List { get; set; }

    }
}
