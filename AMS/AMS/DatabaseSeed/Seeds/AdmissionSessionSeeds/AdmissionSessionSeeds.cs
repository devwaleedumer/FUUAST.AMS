using AMS.DATA;
using Newtonsoft.Json;
using System.Reflection;

namespace AMS.DatabaseSeed.Seeds.AdmissionSessionSeeds
{
    public class AdmissionSessionSeeds : ICustomSeeder
    {
        private readonly AMSContext _db;
        private readonly ILogger<AdmissionSessionSeeds> _logger;

        public AdmissionSessionSeeds(ILogger<AdmissionSessionSeeds> logger, AMSContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!_db.Sessions.Any())
            {
                _logger.LogInformation("Started to Seed AdmissionSession.");
                //populating data from json file
                string admissionSessionData = await File.ReadAllTextAsync(path!.Split("bin")[0] + "/DatabaseSeed/Seeds/AdmissionSessionSeeds/AdmissionSessions.json");
                var admissionSessions = JsonConvert.DeserializeObject<List<AMS.DOMAIN.Entities.Lookups.AdmissionSession>>(admissionSessionData);
                if (admissionSessions is not null)
                {
                    foreach (var admissionSession in admissionSessions)
                    {
                        await _db.Sessions.AddAsync(admissionSession, cancellationToken);
                    }
                }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded AdmissionSession.");
            }
        }
    }
}
