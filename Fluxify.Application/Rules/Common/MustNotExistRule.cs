using Application.Rules;

namespace Fluxify.Application.Rules.Common
{
    public sealed class MustNotExistRule<T> : BusinessRule
    {
        private readonly T? _entity;

        public MustNotExistRule(T? entity, string? message = null)
        {
            _entity = entity;

            Code = "ENTITY_ALREADY_EXISTS";
            Message = message ?? $"{typeof(T).Name} already exists.";
        }

        public override Task<BusinessRuleResult> InvokeAsync()
        {
            return Task.FromResult(
                _entity is null
                    ? BusinessRuleResult.Success()
                    : BusinessRuleResult.Fail(Code, Message));
        }
    }
}
