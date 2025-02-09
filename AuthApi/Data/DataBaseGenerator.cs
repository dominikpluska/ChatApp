using Microsoft.EntityFrameworkCore;

namespace AuthApi.Data
{
    internal static class DataBaseGenerator
    {
        internal static WebApplication GenerateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }

                dbContext.Database.CloseConnection();
            }
            return app;
        }
    }
}
