﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.SHARED.Exceptions
{
    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException(string message)
           : base(message, null, HttpStatusCode.Unauthorized)
        {
        }
    }
}
