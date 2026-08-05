using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Rules
{
    public abstract class BusinessRule
    {
        public abstract string Code { get; }
        public string? Message { get; protected set; }
        public abstract Task<BusinessRuleResult> InvokeAsync();
    }
}
