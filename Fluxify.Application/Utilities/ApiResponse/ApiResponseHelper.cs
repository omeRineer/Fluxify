namespace Fluxify.Application.Utilities.ApiResponse
{
    public static class ApiResponseHelper
    {
        public static ApiResponse Success(string? message = null)
            => new ApiResponse(true, message);
        public static ApiDataResponse<TData> Success<TData>(TData? data = default,
                                                            string ? message = null)
            => new ApiDataResponse<TData>(true, data, message);


        public static ApiResponse Error(string? message = null,
                                        List<string>? errors = null)
            => new ApiResponse(false, message, errors);
        public static ApiDataResponse<TData> Error<TData>(TData? data = default,
                                                          string? message = null,
                                                          List<string>? errors = null)
            => new ApiDataResponse<TData>(false, data, message, errors);
    }
}
