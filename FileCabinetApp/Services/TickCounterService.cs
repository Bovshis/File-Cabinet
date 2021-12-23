using System;
using System.Diagnostics;

namespace FileCabinetApp.Services
{
    public static class TickCounterService
    {
        private static readonly Stopwatch Stopwatch = new ();

        public static long GetTicks<TIn, TOut>(Func<TIn, TOut> method, TIn parameter, out TOut methodsOut)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            Stopwatch.Start();
            methodsOut = method.Invoke(parameter);
            Stopwatch.Stop();
            var ticks = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();

            return ticks;
        }

        public static long GetTicks<T1In, T2In, TOut>(Func<T1In, T2In, TOut> method, T1In parameter1, T2In parameter2, out TOut methodsOut)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            Stopwatch.Start();
            methodsOut = method.Invoke(parameter1, parameter2);
            Stopwatch.Stop();
            var ticks = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();

            return ticks;
        }

        public static long GetTicks<T>(Func<T> method, out T methodsOut)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            Stopwatch.Start();
            methodsOut = method.Invoke();
            Stopwatch.Stop();
            var ticks = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();

            return ticks;
        }

        public static long GetTicks<T>(Action<T> method, T parameter)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            Stopwatch.Start();
            method.Invoke(parameter);
            Stopwatch.Stop();
            var ticks = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();

            return ticks;
        }
    }
}
