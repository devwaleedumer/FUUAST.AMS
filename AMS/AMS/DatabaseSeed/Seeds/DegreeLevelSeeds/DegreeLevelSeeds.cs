using AMS.DATA;
using Newtonsoft.Json;
using System.Reflection;

namespace AMS.DatabaseSeed.Seeds.DegreeLevelSeeds
{
    public class DegreeLevelSeeds : ICustomSeeder
    {
        private readonly AMSContext _db;
        private readonly ILogger<DegreeLevelSeeds> _logger;

        public DegreeLevelSeeds(ILogger<DegreeLevelSeeds> logger, AMSContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!_db.DegreeLevels.Any())
            {
                _logger.LogInformation("Started to Seed DegreeLevels.");
                //populating data from json file
                string degreeLevelData = await File.ReadAllTextAsync(path!.Split("bin")[0] + "/DatabaseSeed/Seeds/DegreeLevelSeeds/DegreeLevels.json");
                var degreeLevels = JsonConvert.DeserializeObject<List<AMS.DOMAIN.Entities.Lookups.DegreeLevel>>(degreeLevelData);
                if (degreeLevels is not null)
                {
                    foreach (var degreeLevel in degreeLevels)
                    {
                        await _db.DegreeLevels.AddAsync(degreeLevel, cancellationToken);
                    }
                }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded DegreeLevels.");
            }
        }
    }
}
