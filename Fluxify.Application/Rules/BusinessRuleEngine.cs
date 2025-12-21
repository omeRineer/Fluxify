using Application.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Rules
{
    public static class BusinessRuleEngine
    {
        public static async Task<BusinessRuleResult> Run(params IBusinessRule[] rules)
        {
            foreach (var rule in rules)
            {
                var result = await rule.InvokeAsync();
                if (!result.IsSuccess)
                    return result;
            }

            return BusinessRuleResult.Success();
        }
    }
}
