using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.WordReportingEngine.Abstractions
{
    /// <summary>
    /// Represents the result of executing a process.
    /// </summary>
    public interface IExecutionResult
    {
        /// <summary>
        /// The exit code returned by the executed process.
        /// </summary>
        int ExitCode { get; }

        /// <summary>
        /// The output of the executed process.
        /// </summary>
        string Output { get; }

        /// <summary>
        /// The command line arguments used to execute the process.
        /// </summary>
        string CommandLineArguments { get; }

        /// <summary>
        /// The error message, if any, encountered during the execution of the process.
        /// </summary>
        string? ErrorMessage { get; }

        /// <summary>
        /// Indicates whether the executed process encountered an error.
        /// </summary>
        bool HasError { get; }
    }
}
