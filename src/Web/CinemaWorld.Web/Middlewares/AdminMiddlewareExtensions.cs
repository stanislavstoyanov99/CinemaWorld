namespace CinemaWorld.Web.Middlewares
{
    using Microsoft.AspNetCore.Builder;

    public static class AdminMiddlewareExtensions
    {
        public static IApplicationBuilder UseAdminMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminMiddleware>();
        }
    }
}
