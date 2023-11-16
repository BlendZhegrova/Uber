using Uber.MiddleWares;
namespace Uber.Configurations;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleWare>();
    }
}