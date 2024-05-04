using AMS.DATA;
using Newtonsoft.Json;
using System.Reflection;

namespace AMS.DatabaseSeed.Seeds.TimeShiftSeeds
{
    public class TimeShiftSeeds : ICustomSeeder
    {
        private readonly AMSContext _db;
        private readonly ILogger<TimeShiftSeeds> _logger;

        public TimeShiftSeeds(ILogger<TimeShiftSeeds> logger, AMSContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!_db.TimeShifts.Any())
            {
                _logger.LogInformation("Started to Seed TimeShift.");
                //populating data from json file
                string timeShiftData = await File.ReadAllTextAsync(path!.Split("bin")[0] + "/DatabaseSeed/Seeds/TimeShiftSeeds/TimeShifts.json");
                var timeShifts = JsonConvert.DeserializeObject<List<AMS.DOMAIN.Entities.Lookups.TimeShift>>(timeShiftData);
                if (timeShifts is not null)
                {
                    foreach (var timeShift in timeShifts)
                    {
                        await _db.TimeShifts.AddAsync(timeShift, cancellationToken);
                    }
                }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded TimeShiftSeeds.");
            }
        }
    }

}
