using System;
using System.IO;

namespace FileCabinetApp.Writers
{
    public class LogsWriter
    {
        private readonly TextWriter writer;

        public LogsWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Write<TIn, TOut>(Func<TIn, TOut> method, TIn parameter, out TOut methodsOut)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            this.writer.WriteLine($"{DateTime.Now} - Calling {nameof(method)} with {parameter}");
            methodsOut = method.Invoke(parameter);
            this.writer.WriteLine($"{DateTime.Now} - {nameof(method)} returned {methodsOut}");
            this.writer.Flush();
        }

        public void Write<T1In, T2In, TOut>(Func<T1In, T2In, TOut> method, T1In parameter1, T2In parameter2, out TOut methodsOut)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            this.writer.WriteLine($"{DateTime.Now} - Calling {nameof(method)} with {parameter1} and {parameter2}");
            methodsOut = method.Invoke(parameter1, parameter2);
            this.writer.WriteLine($"{DateTime.Now} - {nameof(method)} returned {methodsOut}");
            this.writer.Flush();
        }

        public void Write<T>(Func<T> method, out T methodsOut)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            this.writer.WriteLine($"{DateTime.Now} - Calling {nameof(method)}");
            methodsOut = method.Invoke();
            this.writer.WriteLine($"{DateTime.Now} - {nameof(method)} returned {methodsOut}");
            this.writer.Flush();
        }

        public void Write<T>(Action<T> method, T parameter)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            this.writer.WriteLine($"{DateTime.Now} - Calling {nameof(method)} with {parameter}");
            method.Invoke(parameter);
            this.writer.WriteLine($"{DateTime.Now} - {nameof(method)} executed");
            this.writer.Flush();
        }
    }
}
