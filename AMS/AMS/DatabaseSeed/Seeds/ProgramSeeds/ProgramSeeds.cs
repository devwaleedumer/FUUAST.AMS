
using AMS.DATA;
using Newtonsoft.Json;
using System.Reflection;

namespace AMS.DatabaseSeed.Seeds.ProgramSeeds
{
    public class ProgramSeeds : ICustomSeeder
    {
        private readonly AMSContext _db;
        private readonly ILogger<ProgramSeeds> _logger;

        public ProgramSeeds(ILogger<ProgramSeeds> logger, AMSContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!_db.Programs.Any())
            {
                _logger.LogInformation("Started to Seed Programs.");
                //populating data from json file
                string programData = await File.ReadAllTextAsync(path!.Split("bin")[0] + "/DatabaseSeed/Seeds/ProgramSeeds/Programs.json");
                var programs = JsonConvert.DeserializeObject<List<AMS.DOMAIN.Entities.Lookups.Program>>(programData);
                if (programs is not null)
                {
                    foreach (var program in programs)
                    {
                        await _db.Programs.AddAsync(program, cancellationToken);
                    }
                }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded Programs.");
            }
        }
    }
}
