using System;

namespace BaseTools.Trace
{
    /// <summary>
    /// Specifies the type of line to write.
    /// </summary>
    [Flags]
    public enum LineType
    {
        /// <summary>
        /// Default line type.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Indicates the start of a section.
        /// </summary>
        Start = 1,

        /// <summary>
        /// Indicates the end of a section.
        /// </summary>
        End = 2,
    }

    /// <summary>
    /// Provides methods to write trace messages to the console.
    /// </summary>
    public static class TraceWriter
    {
        /// <summary>
        /// Writes a message to the console with optional start and end lines.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="lineType">The type of line to write.</param>
        public static void WriteLine(string message, LineType lineType = LineType.Default)
        {
            if (lineType.HasFlag(LineType.Start))
            {
                Console.WriteLine("--------------------------------------------------");
            }

            Console.WriteLine(message);

            if (lineType.HasFlag(LineType.End))
            {
                Console.WriteLine("--------------------------------------------------");
            }
        }
    }
}