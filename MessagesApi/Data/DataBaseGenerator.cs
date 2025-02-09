namespace MessagesApi.Data
{
    internal static class DataBaseGenerator
    {
        internal static WebApplication GenerateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
            }
            return app;
        }
    }
}
