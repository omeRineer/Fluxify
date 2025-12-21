using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Rules
{
    public interface IBusinessRule
    {
        public string Code { get; }
        public string? Message { get; }
        Task<BusinessRuleResult> InvokeAsync();
    }
}
