using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using new2me_api.Errors;

namespace new2me_api.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate next;
        public ILogger<CustomExceptionMiddleware> Logger { get; }
        private readonly IHostEnvironment env;

        public CustomExceptionMiddleware(RequestDelegate next, 
                                            ILogger<CustomExceptionMiddleware> logger,
                                            IHostEnvironment env){
            this.env = env;
            this.Logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context){
            try{
                //white pass control to the next middleware, catch any exception
                await next(context);
            }
            catch (Exception ex){
                ApiError errorResponse;

                // exception types
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError; // default
                var message = "Some unknown error occurred"; // default

                var exceptionType = ex.GetType();

                if (exceptionType==typeof(UnauthorizedAccessException)){
                    statusCode = HttpStatusCode.Forbidden;
                    message = "You are not authorized.";
                }


                // depend on whether it is dev or production, no stacktrace returned if production
                if (env.IsDevelopment()){
                    errorResponse = new ApiError((int)statusCode, ex.Message, ex.StackTrace.ToString());
                }
                else{
                    errorResponse = new ApiError((int)statusCode, message);
                }

                // log to the development console
                this.Logger.LogError(ex, ex.Message);

                // draft the response to send back to the client
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(errorResponse.ToString());
            }
        }
    }
}