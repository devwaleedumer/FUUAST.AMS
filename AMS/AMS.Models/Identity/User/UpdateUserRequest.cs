using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Identity.User
{
    public class UpdateUserRequest
    {
        public string Id { get; set; } = default!;
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        //public FileUploadRequest? Image { get; set; }
        public bool DeleteCurrentImage { get; set; } = false;
    }
}
