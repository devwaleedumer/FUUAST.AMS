using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SHARED.Exceptions
{
    public class AMSException : CustomException
    {
        public AMSException(string message, List<string>? errors = default)
            : base(message, errors, HttpStatusCode.InternalServerError)
        {
        }
    }
}
