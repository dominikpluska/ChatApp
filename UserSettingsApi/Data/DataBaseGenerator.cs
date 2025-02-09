namespace UserSettingsApi.Data
{
    public static class DataBaseGenerator
    {
        public static WebApplication Generate(this WebApplication app)
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
