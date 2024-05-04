using AMS.DATA;
using Newtonsoft.Json;
using System.Reflection;

namespace AMS.DatabaseSeed.Seeds.ProgramTypeSeeds
{
    public class ProgramTypeSeeds : ICustomSeeder
    {
        private readonly AMSContext _db;
        private readonly ILogger<ProgramTypeSeeds> _logger;

        public ProgramTypeSeeds(ILogger<ProgramTypeSeeds> logger, AMSContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!_db.ProgramTypes.Any())
            {
                _logger.LogInformation("Started to Seed ProgramTypes.");
                //populating data from json file
                string programTypeData = await File.ReadAllTextAsync(path!.Split("bin")[0] + "/DatabaseSeed/Seeds/ProgramTypeSeeds/ProgramTypes.json");
                var programTypes = JsonConvert.DeserializeObject<List<AMS.DOMAIN.Entities.Lookups.ProgramType>>(programTypeData);
                if (programTypes is not null)
                {
                    foreach (var type in programTypes)
                    {
                        await _db.ProgramTypes.AddAsync(type, cancellationToken);
                    }
                }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded ProgramTypes.");
            }
        }
    }
}
