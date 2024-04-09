using AMS.DOMAIN.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.DOMAIN.Entities.Lookups
{
    public class PreviousDegreeDetail : IBaseEntity
    {
        public int Id { get; set; }
        public required int DegreeLevelId { get; set; }
        public required int DegreeName { get; set; }
        public string? Description { get; set; }
        public virtual DegreeLevel? DegreeLevel { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
