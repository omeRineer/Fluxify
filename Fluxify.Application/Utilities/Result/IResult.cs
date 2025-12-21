using AR = Fluxify.Application.Utilities.ApiResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxify.Application.Utilities.Result
{
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }
}
