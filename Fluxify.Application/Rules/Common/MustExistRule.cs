using Application.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Rules.Common
{
    public sealed class MustExistRule<T> : BusinessRule
    {
        private readonly T? _entity;

        public override string Code => "ENTITY_NOT_FOUND";

        public MustExistRule(T? entity, string? message = null)
        {
            _entity = entity;
            Message = message ?? $"{typeof(T).Name} not found.";
        }

        public override Task<BusinessRuleResult> InvokeAsync()
        {
            return Task.FromResult(
                _entity is null
                    ? BusinessRuleResult.Fail(Code, Message)
                    : BusinessRuleResult.Success());
        }
    }
}
