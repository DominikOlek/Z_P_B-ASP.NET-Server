using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;
using ApiP.Exceptions;

namespace ApiP.Middleware
{
    public class ErrorMiddleware : IMiddleware {
        private readonly ILogger<ErrorMiddleware> _logger;

        public ErrorMiddleware(ILogger<ErrorMiddleware> Logger)
        {
            _logger = Logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundExceptions notfound)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notfound.Message);
            }
            catch (ApiP.Exceptions.OverflowException over) {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsync(over.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Coś poszło nie tak");
            }
        }
    }

}
