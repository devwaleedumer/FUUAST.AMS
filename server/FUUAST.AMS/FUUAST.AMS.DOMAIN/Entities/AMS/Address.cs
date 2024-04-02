using FUUAST.AMS.DOMAIN.Entities.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.DOMAIN.Entities.AMS
{
    public class Address
    {
        public int Id { get; set; }
        public string StreetAddress { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public int PostalCode { get; set; }
        public int AddressTypeEid { get; set; }

        public int ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }
    }
}