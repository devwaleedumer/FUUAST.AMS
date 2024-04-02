using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.SHARED.Constants
{
    public static class StatusCodes
    {
        public const int Success = 200;
        public const int Error = 400;
        public const int Warning = 300;
        public const int NotFound = 404;
        public const int Forbidden = 403;
        public const int RequestEntityError = 413;
        public const int InternalServerError = 500;
    }
}
