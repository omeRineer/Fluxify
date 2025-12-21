using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxify.Application.Utilities.Result
{
    public class SuccessResult : Result, IResult
    {
        public SuccessResult(string message) : base(true, message)
        {

        }

        public SuccessResult() : base(true)
        {

        }
    }
}
