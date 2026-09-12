using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Common.Pagination
{
    public abstract class QueryOptions
    {
        public int Page { get; set; } = 0;

        [Range(10, 50)]
        public int Size { get; set; } = 20;

        public string? Search { get; set; }

    }
}
