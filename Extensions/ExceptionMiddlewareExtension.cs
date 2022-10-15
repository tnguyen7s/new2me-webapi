using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;

namespace new2me_api.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this WebApplication app){
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // return limited message replacing for the full error stack
                app.UseExceptionHandler(
                    options => 
                    {
                        options.Run(
                            async context => {
                            var ex = context.Features.Get<IExceptionHandlerFeature>();

                            if (ex!=null){
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                await context.Response.WriteAsync(ex.Error.Message);  
                            }   
                            }
                        );
                    }   
                );
            }
        }
    }
}