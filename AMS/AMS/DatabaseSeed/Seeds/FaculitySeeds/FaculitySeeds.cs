using AMS.DATA;
using Newtonsoft.Json;
using System.Reflection;

namespace AMS.DatabaseSeed.Seeds.FaculitSeeds
{
    public class FaculitySeeds : ICustomSeeder
    {
        private readonly AMSContext _db;
        private readonly ILogger<FaculitySeeds> _logger;

        public FaculitySeeds(ILogger<FaculitySeeds> logger, AMSContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath);
            if (!_db.Faculties.Any())
            {
                _logger.LogInformation("Started to Seed Faculities.");
               // populating data from json file
                string faculityData = await File.ReadAllTextAsync(path!.Split("bin")[0] + "/DatabaseSeed/Seeds/FaculitySeeds/Faculities.json");
                var faculities = JsonConvert.DeserializeObject<List<AMS.DOMAIN.Entities.Lookups.Faculity>>(faculityData);
                if (faculities is not null)
                {
                    foreach (var faculity in faculities)
                    {
                        await _db.Faculties.AddAsync(faculity, cancellationToken);
                    }
                }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded Faculities.");
            }
        }
    }
}
