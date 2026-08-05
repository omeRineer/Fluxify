using Application.Rules;

namespace Fluxify.Application.Rules.Common
{
    public sealed class MustNotExistRule<T> : BusinessRule
    {
        private readonly T? _entity;

        public override string Code => "ENTITY_ALREADY_EXISTS";

        public MustNotExistRule(T? entity, string? message = null)
        {
            _entity = entity;
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
