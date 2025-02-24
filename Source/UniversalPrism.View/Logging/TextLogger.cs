﻿using System;
using System.Globalization;
using System.IO;

namespace UniversalPrism.View.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILoggerFacade"/> that logs into a <see cref="TextWriter"/>.
    /// </summary>
    public class TextLogger : ILoggerFacade, IDisposable
    {
        public TextWriter Writer { get; set; } = Console.Out;

        /// <summary>
        /// Initializes a new instance of <see cref="TextLogger"/> that writes to
        /// the console output.
        /// </summary>
        public TextLogger() { }

        /// <summary>
        /// Write a new log entry with the specified category and priority.
        /// </summary>
        /// <param name="message">Message body to log.</param>
        /// <param name="category">Category of the entry.</param>
        /// <param name="priority">The priority of the entry.</param>
        public void Log(string message, Category category, Priority priority)
        {
            string messageToLog = string.Format(CultureInfo.InvariantCulture, "{1}: {2}. Priority: {3}. Timestamp:{0:u}.", DateTime.Now,
                                                category.ToString().ToUpper(CultureInfo.InvariantCulture), message, priority.ToString());

            Writer.WriteLine(messageToLog);
        }

        /// <summary>
        /// Disposes the associated <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="disposing">When <see langword="true"/>, disposes the associated <see cref="TextWriter"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Writer?.Dispose();
            }
        }

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        /// <remarks>Calls <see cref="Dispose(bool)"/></remarks>.
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
