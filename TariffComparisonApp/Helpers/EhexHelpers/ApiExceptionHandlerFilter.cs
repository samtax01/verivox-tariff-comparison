using System;
using Microsoft.AspNetCore.Mvc.Filters;

// ReSharper disable once CheckNamespace
#pragma warning disable 1570
namespace Ehex.Helpers
{
    /// <summary>
    /// Helper Class
    /// @version: 1.0
    /// @repo: https://github.com/samtax01/ehex-dotnet-helper
    /// @adviseFrom: https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-5.0#use-exceptions-to-modify-the-response
    ///
    ///     Global Exception Handler
    ///     To handle all controllers exceptions.
    /// 
    ///        USAGE
    ///          services.AddControllers(options => options.Filters.Add(new ApiExceptionHandlerFilter()));
    /// </summary>
    public class ApiExceptionHandlerFilter : IActionFilter, IOrderedFilter
    {
        // The preceding filter specifies an Order of the maximum integer value minus 10. This allows other filters to run at the end of the pipeline.
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Exception is null)
                return;
            
            var response = ApiResponse.FailureMessage(context.Exception.Message);
            response.StatusCode = context.Exception switch
            {
                ApiException exception => exception.StatusCode,
                ArgumentException => 422,
                _ => 500
            };
            context.Result = response;
            context.ExceptionHandled = true;
            
        }
    }
    
    

    public class ApiException: Exception
    {
        public ApiException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
        public int StatusCode { set; get; }
    }
    
    
}