using AMS.DATA;
using Newtonsoft.Json;
using System.Reflection;

namespace AMS.DatabaseSeed.Seeds.PreviousDegreeSeeds
{
    public class DegreeGroupSeeds : ICustomSeeder
    {
        private readonly AMSContext _db;
        private readonly ILogger<DegreeGroupSeeds> _logger;

        public DegreeGroupSeeds(ILogger<DegreeGroupSeeds> logger, AMSContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!_db.DegreeGroups.Any())
            {
                _logger.LogInformation("Started to Seed DegreeGroup.");
                //populating data from json file
                string previousDegreesData = await File.ReadAllTextAsync(path!.Split("bin")[0] + "/DatabaseSeed/Seeds/DegreeGroupSeeds/DegreeGroup.json");
                var previousDegrees = JsonConvert.DeserializeObject<List<AMS.DOMAIN.Entities.Lookups.DegreeGroup>>(previousDegreesData);
                if (previousDegrees is not null)
                {
                        await _db.DegreeGroups.AddRangeAsync(previousDegrees, cancellationToken);
                                   }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded DegreeGroup.");
            }
        }
    }
}
