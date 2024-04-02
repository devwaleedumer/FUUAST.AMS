using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.DOMAIN.Entities.AMS
{
    public class Guardian
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Occupation { get; set; }
        public decimal TotalPerMonthIncome { get; set; }
        public decimal TotalPerMonthExpenses { get; set; }
        public string PhoneNo { get; set; }
        public int ApplicantId { get; set; }
        public virtual Applicant? Applicant { get; set; }

    }
}
