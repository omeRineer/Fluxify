namespace Application.Rules
{
    public class BusinessRuleResult
    {
        public bool IsSuccess { get; }
        public string? Code { get; }
        public string? Message { get; }
        public IReadOnlyDictionary<string, object>? Metadata { get; }

        private BusinessRuleResult(bool isSuccess, string? code, string? message, IReadOnlyDictionary<string, object>? metadata = null)
        {
            IsSuccess = isSuccess;
            Code = code;
            Message = message;
            Metadata = metadata;
        }

        public static BusinessRuleResult Success()
            => new(true, null, null);

        public static BusinessRuleResult Fail(string code, string message, IReadOnlyDictionary<string, object>? metadata = null)
            => new(false, code, message, metadata);
    }
}
