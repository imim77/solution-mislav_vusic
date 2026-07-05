using Microsoft.EntityFrameworkCore;

namespace Proizvodi.Api.Data;

public static class DataExtensions
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProizvodiContext>();
        dbContext.Database.Migrate();
    }

    [Obsolete("Use MigrateDatabase instead.")]
    public static void MigrateDd(this WebApplication app)
    {
        app.MigrateDatabase();
    }
}
