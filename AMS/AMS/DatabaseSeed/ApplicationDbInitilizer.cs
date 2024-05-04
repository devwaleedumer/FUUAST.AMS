using AMS.DATA;
using Microsoft.EntityFrameworkCore;

namespace AMS.DatabaseSeed
{
    internal class ApplicationDbInitializer
    {
        private readonly AMSContext _dbContext;
        private readonly ApplicationDbSeeder _dbSeeder;
        private readonly ILogger<ApplicationDbInitializer> _logger;

        public ApplicationDbInitializer(AMSContext dbContext, ApplicationDbSeeder dbSeeder, ILogger<ApplicationDbInitializer> logger)
        {
            _dbContext = dbContext;
            _dbSeeder = dbSeeder;
            _logger = logger;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            if (_dbContext.Database.GetMigrations().Any())
            {
                if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    _logger.LogInformation("Applying Migrations ...");
                    await _dbContext.Database.MigrateAsync(cancellationToken);
                }

                if (await _dbContext.Database.CanConnectAsync(cancellationToken))
                {
                    _logger.LogInformation("Connection to Database Succeeded.");

                    await _dbSeeder.SeedDatabaseAsync(_dbContext, cancellationToken);
                }
            }
        }
    }
}
