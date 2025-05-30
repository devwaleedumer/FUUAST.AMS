﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.WordReportingEngine.Abstractions
{

    /// <summary>
    /// Represents an interface for building command line arguments with the ability to add, remove, and clear arguments,
    /// and finally build the final string representation of the arguments.
    /// </summary>
    public interface ICommandLineArgumentsBuilder
    {
        /// <summary>
        /// Adds a new argument to the builder.
        /// </summary>
        /// <param name="argument">The argument to be added.</param>
        /// <returns>A reference to this instance of the <see cref="ICommandLineArgumentsBuilder"/> interface.</returns>
        ICommandLineArgumentsBuilder Add(string argument);

        /// <summary>
        /// Adds an argument and its value to the builder, with the option to wrap the value in quotes.
        /// </summary>
        /// <param name="argument">The argument to be added.</param>
        /// <param name="value">The value of the argument.</param>
        /// <param name="wrapValueInQuotes">Specifies whether to wrap the value in quotes. Defaults to <c>false</c>.</param>
        /// <returns>A reference to this instance of the <see cref="ICommandLineArgumentsBuilder"/> interface.</returns>
        ICommandLineArgumentsBuilder Add(string argument, string value, bool wrapValueInQuotes = false);

        /// <summary>
        /// Removes a previously added argument from the builder.
        /// </summary>
        /// <param name="argument">The argument to be removed.</param>
        /// <returns>A reference to this instance of the <see cref="ICommandLineArgumentsBuilder"/> interface.</returns>
        ICommandLineArgumentsBuilder Remove(string argument);

        /// <summary>
        /// Clears all the previously added arguments from the builder.
        /// </summary>
        /// <returns>A reference to this instance of the <see cref="ICommandLineArgumentsBuilder"/> interface.</returns>
        ICommandLineArgumentsBuilder Clear();

        /// <summary>
        /// Builds the final string representation of the command line arguments.
        /// </summary>
        /// <returns>The final string representation of the command line arguments.</returns>
        string Build();
    }
}
