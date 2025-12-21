using Application.Rules;

namespace Fluxify.Application.Rules.Common
{
    public class DataMustBeAvaibleRule : IBusinessRule
    {
        public string Code => "DATA_ALREADY_EXISTS";
        public string? Message { get; }

        readonly object? Data;

        public DataMustBeAvaibleRule(object? data, string? message = "Data already exist")
        {
            Data = data;
            Message = message;
        }

        public Task<BusinessRuleResult> InvokeAsync()
        {
            if (Data is not null)
                return Task.FromResult(BusinessRuleResult.Fail(Code, Message));

            return Task.FromResult(BusinessRuleResult.Success());
        }
    }
}
