using Microsoft.AspNetCore.Builder;

namespace IpShieldMiddleware.Extensions;

public static class IpShieldMiddlewareExtensions
{
    public static IApplicationBuilder UseIpFiltering(this IApplicationBuilder app)
    {
        return app.UseMiddleware<Middlewares.IpShieldMiddleware>();
    }
}
