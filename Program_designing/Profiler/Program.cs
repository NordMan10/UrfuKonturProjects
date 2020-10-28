using System;
using System.Collections.Generic;
using System.Diagnostics;
using MyPhotoshop;
using MyPhotoshop.Filters;

namespace Profiler
{
    class Program
    {
        static void Test(Func<double[], LightningParameters> func, int N)
        {
            var args = new double[] { 0 };
            func(args);

            var watch = new Stopwatch();
            watch.Start();

            for (var i = 0; i < N; i++)
            {
                func(args);
            }

            watch.Stop();

            Console.WriteLine((double)watch.ElapsedMilliseconds * 1000 / N);
        }

        static void Main(string[] args)
        {
            var simpleHandler = new StaticParametersHandler<LightningParameters>();

            Test((values) => simpleHandler.CreateParameters(values), 1000000);

            var expressionHandler = new ExpressionParametersHandler<LightningParameters>();

            Test((values) => expressionHandler.CreateParameters(values), 1000000);

            Test((values) => new LightningParameters() { Coefficient = values[0] }, 1000000);
        }
    }
}
