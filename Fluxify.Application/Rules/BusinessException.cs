using Application.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Rules
{
    public class BusinessException : Exception
    {
        public string Code { get; }
        public IReadOnlyDictionary<string, object>? Metadata { get; }

        public BusinessException(BusinessRuleResult result)
            : base(result.Message)
        {
            Code = result.Code!;
            Metadata = result.Metadata;
        }
    }
}
