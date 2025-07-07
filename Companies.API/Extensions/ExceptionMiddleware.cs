using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Companies.API.Extensions
{
    public static class ExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeatures = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeatures != null)
                    {
                        var problemDetailsFactory = app.Services.GetService<ProblemDetailsFactory>();
                        ArgumentNullException.ThrowIfNull(nameof(ProblemDetailsFactory));
                    }
                });
            });
        }
    }
}
                                                            