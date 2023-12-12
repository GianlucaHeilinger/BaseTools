using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTools.Trace
{
    [Flags]
    public enum LineType
    {
        Default = 0,
        Start = 1,
        End = 2,
    }

    public class TraceWriter
    {
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
