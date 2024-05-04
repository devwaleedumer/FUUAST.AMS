using AMS.DATA;
using Newtonsoft.Json;
using System.Reflection;

namespace AMS.DatabaseSeed.Seeds.DepartmentSeeds
{
    public class DepartmentSeeds : ICustomSeeder
    {
        private readonly AMSContext _db;
        private readonly ILogger<DepartmentSeeds> _logger;

        public DepartmentSeeds(ILogger<DepartmentSeeds> logger, AMSContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!_db.Departments.Any())
            {
                _logger.LogInformation("Started to Seed Departments.");
                //populating data from json file
                string departmentData = await File.ReadAllTextAsync(path!.Split("bin")[0] + "/DatabaseSeed/Seeds/DepartmentSeeds/Departments.json");
                var departments = JsonConvert.DeserializeObject<List<AMS.DOMAIN.Entities.Lookups.Department>>(departmentData);
                if (departments is not null)
                {
                    foreach (var department in departments)
                    {
                        await _db.Departments.AddAsync(department, cancellationToken);
                    }
                }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded Departments.");
            }
        }
    }
}
