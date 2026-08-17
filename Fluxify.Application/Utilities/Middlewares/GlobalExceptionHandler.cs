using Fluxify.Application.Rules;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Utilities.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, title, detail, metaData) = exception switch
            {
                BusinessException ex => (
                    StatusCodes.Status400BadRequest,
                    ex.Code,
                    ex.Message,
                    ex.Metadata
                ),


                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "An unexpected error occurred on the server.",
                    null
                )
            };

            var problemDetails = new
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };


            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
