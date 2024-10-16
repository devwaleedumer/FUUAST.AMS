using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Identity.User
{
    public record class ApplicantUserResponse
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? ProfilePictureUrl { get; set; }
    }
}
