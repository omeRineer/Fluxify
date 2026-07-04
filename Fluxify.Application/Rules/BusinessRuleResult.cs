namespace Application.Rules
{
    public class BusinessRuleResult
    {
        public bool IsSuccess { get; }
        public string? ErrorCode { get; }
        public string? Message { get; }

        private BusinessRuleResult(bool isSuccess, string? message, string? errorCode)
        {
            IsSuccess = isSuccess;
            ErrorCode = errorCode;
            Message = message;
        }

        public static BusinessRuleResult Success()
            => new(true, null, null);

        public static BusinessRuleResult Fail(string errorCode, string message)
            => new(false, message, errorCode);
    }
}
