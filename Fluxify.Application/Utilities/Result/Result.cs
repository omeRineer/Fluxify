using AR = Fluxify.Application.Utilities.ApiResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxify.Application.Utilities.Result
{
    public class Result : IResult
    {
        public bool Success { get; }
        public string Message { get; }

        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }
        public Result(bool success)
        {
            Success = success;
        }
    }
}
