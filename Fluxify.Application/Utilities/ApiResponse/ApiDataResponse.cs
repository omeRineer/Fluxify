namespace Fluxify.Application.Utilities.ApiResponse
{
    public class ApiDataResponse<TData> : ApiResponse
    {
        public TData? Data { get; set; }

        public ApiDataResponse(bool success, TData? data) : base(success)
        {
            Data = data;
        }
        public ApiDataResponse(bool success, TData? data, string? message) : base(success, message)
        {
            Data = data;
        }

        public ApiDataResponse(bool success, TData? data, string? message, List<string>? errors) : base(success, message, errors)
        {
            Data = data;
        }
    }
}
