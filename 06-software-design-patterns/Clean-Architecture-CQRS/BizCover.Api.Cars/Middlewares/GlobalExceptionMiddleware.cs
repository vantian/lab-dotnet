using BizCover.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using System.Threading.Tasks;

namespace BizCover.Api.Cars.Middlewares {
    public class GlobalExceptionMiddleware : IMiddleware {

        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger) {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            try {
                await next(context);
            }
            catch (ValidationException vex) {
                _logger.LogError(vex, "[422] Validation error occurred.");

                // get error details
                var errors = vex.Errors != null ? 
                    vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}") 
                    : new[] { vex.Message };

                context.Response.StatusCode = 422;
                await context.Response.WriteAsJsonAsync(Result<IEnumerable<string>>.NotValid("error on validation.", errors));
            }
            catch (ApplicationException aex) {
                _logger.LogError(aex, "[400] An application error occurred.");
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("An error occurred on the application, please check log.");
            }
            catch (Exception ex) {
                _logger.LogError(ex, "[500] An unexpected error occurred.");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An unexpected error occurred.");
            }
        }
    }
}
