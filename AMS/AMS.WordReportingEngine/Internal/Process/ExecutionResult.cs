using AMS.WordReportingEngine.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.WordReportingEngine.Internal.Process
{
    internal class ExecutionResult : IExecutionResult
    {
        /// <inheritdoc/>
        public int ExitCode { get; }

        /// <inheritdoc/>
        public string Output { get; }

        /// <inheritdoc/>
        public string CommandLineArguments { get; }

        /// <inheritdoc/>
        public string? ErrorMessage { get; }

        /// <inheritdoc/>
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionResult"/> class with the specified exit code and output.
        /// </summary>
        /// <param name="exitCode">The exit code returned by the executed process.</param>
        /// <param name="output">The output of the executed process.</param>
        /// <param name="commandLineArguments">The command line arguments used to execute the process.</param>
        /// <param name="errorMessage">The error message, if any, encountered during the execution of the process.</param>
        public ExecutionResult(int exitCode, string output, string commandLineArguments, string? errorMessage)
        {
            ExitCode = exitCode;
            Output = output;
            CommandLineArguments = commandLineArguments;
            ErrorMessage = errorMessage;
        }
    }
}
