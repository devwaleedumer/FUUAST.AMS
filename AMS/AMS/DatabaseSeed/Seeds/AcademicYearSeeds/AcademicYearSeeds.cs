using AMS.DATA;
using AMS.DOMAIN.Entities.Lookups;
using Newtonsoft.Json;
using System.Reflection;

namespace AMS.DatabaseSeed.Seeds.AcademicYearSeeds
{
    // initializing Academic year along Shifts
    public class AcademicYearSeeds : ICustomSeeder
    {
        private readonly AMSContext _db;
        private readonly ILogger<AcademicYearSeeds> _logger;

        public AcademicYearSeeds(ILogger<AcademicYearSeeds> logger, AMSContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!_db.AcademicYears.Any())
            {
                _logger.LogInformation("Started to Seed AcademicYear and Shifts.");
                //populating data from json file
                // todo path is not valid jogaar
                string academicYeardata = await File.ReadAllTextAsync(path!.Split("bin")[0] + "/DatabaseSeed/Seeds/AcademicYearSeeds/AcademicYears.json");
                var academicYears = JsonConvert.DeserializeObject<List<DOMAIN.Entities.Lookups.AcademicYear>>(academicYeardata);
                if (academicYears is not null)
                {
                        await _db.AcademicYears.AddRangeAsync(academicYears, cancellationToken);
                    }
                }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded AcademicYear andShifts.");
            }
        }
    }