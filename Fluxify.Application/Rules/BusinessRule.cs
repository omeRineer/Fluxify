using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Rules
{
    public abstract class BusinessRule
    {
        public string Code { get; protected set; }
        public string? Message { get; protected set; }
        public abstract Task<BusinessRuleResult> InvokeAsync();
    }
}
