using System;
using System.IO;

namespace FileCabinetApp.Writers
{
    /// <summary>
    /// Logs writer.
    /// </summary>
    public class LogsWriter
    {
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsWriter"/> class.
        /// </summary>
        /// <param name="writer">File for writing.</param>
        public LogsWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Write<TIn, TOut>(Func<TIn, TOut> method, TIn parameter, out TOut methodsOut, string name)
        {
            if (method is null)
            {
                throw new ArgumentNullException(name);
            }

            this.writer.WriteLine($"{DateTime.Now} - Calling {name} with {parameter}");
            methodsOut = method.Invoke(parameter);
            this.writer.WriteLine($"{DateTime.Now} - {name} returned {methodsOut}");
            this.writer.Flush();
        }

        public void Write<T1In, T2In, TOut>(Func<T1In, T2In, TOut> method, T1In parameter1, T2In parameter2, out TOut methodsOut, string name)
        {
            if (method is null)
            {
                throw new ArgumentNullException(name);
            }

            this.writer.WriteLine($"{DateTime.Now} - Calling {name} with {parameter1} and {parameter2}");
            methodsOut = method.Invoke(parameter1, parameter2);
            this.writer.WriteLine($"{DateTime.Now} - {name} returned {methodsOut}");
            this.writer.Flush();
        }

        public void Write<T>(Func<T> method, out T methodsOut, string name)
        {
            if (method is null)
            {
                throw new ArgumentNullException(name);
            }

            this.writer.WriteLine($"{DateTime.Now} - Calling {name}");
            methodsOut = method.Invoke();
            this.writer.WriteLine($"{DateTime.Now} - {name} returned {methodsOut}");
            this.writer.Flush();
        }

        public void Write<T>(Action<T> method, T parameter, string name)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            this.writer.WriteLine($"{DateTime.Now} - Calling {name} with {parameter}");
            method.Invoke(parameter);
            this.writer.WriteLine($"{DateTime.Now} - {name} executed");
            this.writer.Flush();
        }
    }
}
