using Microsoft.AspNetCore.HttpOverrides;
using Scalar.AspNetCore;
using Serilog;
using SpaceFlow.Api.Middleware;

namespace SpaceFlow.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseApiFoundation(this WebApplication app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestPath", httpContext.Request.Path.Value ?? string.Empty);
                diagnosticContext.Set("StatusCode", httpContext.Response.StatusCode);
                diagnosticContext.Set("User", httpContext.User.Identity?.Name ?? "anonymous");
                diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier);
            };

            options.GetLevel = (_, elapsed, ex) =>
            {
                if (ex is not null || elapsed > 1000)
                {
                    return Serilog.Events.LogEventLevel.Warning;
                }

                return Serilog.Events.LogEventLevel.Information;
            };
        });

        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AngularClient");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/health");

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        return app;
    }
}