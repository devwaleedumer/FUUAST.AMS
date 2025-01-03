using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.Reporting.IService
{
    public interface IWordReportGenerator
    {
        Task<byte[]> ConvertWordToPdfAsync(string input);

    }
}
