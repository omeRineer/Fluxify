using Application.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Rules.Common
{
    public class DataMustBeFoundRule : IBusinessRule
    {
        public string Code => "DATA_NOT_FOUND";
        public string? Message { get; }

        readonly object? Data;

        public DataMustBeFoundRule(object? data, string? message = "Data not found")
        {
            Message = message;
            Data = data;
        }

        public Task<BusinessRuleResult> InvokeAsync()
        {
            if (Data is null)
                return Task.FromResult(BusinessRuleResult.Fail(Code, Message));

            return Task.FromResult(BusinessRuleResult.Success());
        }
    }
}
