using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.DOMAIN.Entities.AMS
{
    public class Address : IBaseEntity
    {
        public int Id { get; set; }
        public required string StreetAddress { get; set; }
        public required string Province { get; set; }
        public required string District { get; set; }
        public int PostalCode { get; set; }
        public int AddressTypeEid { get; set; }
        public int ApplicantId { get; set; }
        public virtual Applicant? Applicant { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}