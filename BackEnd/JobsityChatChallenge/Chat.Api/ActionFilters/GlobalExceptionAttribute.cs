using Chat.Api.Models;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Chat.Api.ActionFilters
{
    public class GlobalExceptionAttribute : IExceptionFilter
    {

        private readonly ILogger _logger;
        public GlobalExceptionAttribute(ILogger<GlobalExceptionAttribute> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            var typeException = context.Exception.GetType().ToString();
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionName = descriptor.ActionName;
            var ctrlName = descriptor.ControllerName;

            string exceptionMsg = context.Exception.Message + string.Format(":  Controller: {0}  Action: {1}", ctrlName, actionName);
            var exception = new Exception(exceptionMsg, context.Exception);

            TelemetryClient telemetryClient = new TelemetryClient();
            telemetryClient.TrackException(exception);

            var result = new ObjectResult(new ValidationResultModel(context))
            {
                StatusCode = HandleCustomException(typeException)
            };
            context.Result = result;
            _logger.LogError(exception, exceptionMsg, null);

        }
        private static int HandleCustomException(string typeException)
        {
            switch (typeException)
            {
                case "System.ArgumentException":
                    return (int)HttpStatusCode.BadRequest;   
                    
                case "System.ArgumentNullException":
                    return (int)HttpStatusCode.NotFound;

                case "System.DivideByZeroException":
                   return(int)HttpStatusCode.BadRequest;    
               
                case "System.FormatException":
                   return(int)HttpStatusCode.Forbidden;
                   
                case "System.IndexOutOfRangeException":
                   return(int)HttpStatusCode.BadRequest;
                   
                case "System.InvalidOperationException":
                   return(int)HttpStatusCode.BadRequest;
                   
                case "System.UnauthorizedAccessException":
                   return(int)HttpStatusCode.Unauthorized;
              
                default:
                   return(int)HttpStatusCode.InternalServerError;
                   
            }

        }
    }
}
