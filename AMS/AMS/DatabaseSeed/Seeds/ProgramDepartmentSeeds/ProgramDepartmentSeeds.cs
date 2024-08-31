using AMS.DATA;
using AMS.DOMAIN.Entities.Lookups;
using Newtonsoft.Json;
using System.Reflection;

namespace AMS.DatabaseSeed.Seeds.ProgramDepartmentSeeds
{
    // initializing Academic year along Shifts
    public class ProgramDepartmentSeeds : ICustomSeeder
    {
        private readonly AMSContext _db;
        private readonly ILogger<ProgramDepartmentSeeds> _logger;

        public ProgramDepartmentSeeds(ILogger<ProgramDepartmentSeeds> logger, AMSContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!_db.ProgramDepartments.Any())
            {
                _logger.LogInformation("Started to Seed ProgramDepartment.");
                //populating data from json file
                // todo path is not valid jogaar
                string programDepartmentSeedsData = await File.ReadAllTextAsync(path!.Split("bin")[0] + "/DatabaseSeed/Seeds/ProgramDepartmentSeeds/ProgramDepartments.json");
                var programDepartmentSeeds = JsonConvert.DeserializeObject<List<DOMAIN.Entities.Lookups.ProgramDepartment>>(programDepartmentSeedsData);
                if (programDepartmentSeeds is not null)
                {
                    
                        await _db.ProgramDepartments.AddRangeAsync(programDepartmentSeeds, cancellationToken);
                    
                }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded ProgramDepartment.");
            }
        }
    }
}