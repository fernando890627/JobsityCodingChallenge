using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using Chat.Api.Models;

namespace Chat.Utils
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
        : base(new ValidationResultModel(modelState))
        {
            this.StatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        public ValidationFailedResult(Exception ex)
        : base(new ValidationResultModel(ex))
        {
            this.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
