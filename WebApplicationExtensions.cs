using NOLA_API.Middleware;

namespace NOLA_API;

public static class WebApplicationExtensions
{
    public static void ConfigureMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI();
            
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}