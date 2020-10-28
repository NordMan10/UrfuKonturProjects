using MyPhotoshop;
using MyPhotoshop.Filters;
using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    class Program
    {
        static void Test(Action<double[], LightningParameters> action, int N)
        {
            var args = new double[] { 0 };
            var obj = new LightningParameters();
            action(args, obj);

            var watch = new Stopwatch();
            watch.Start();

            for (var i = 0; i < N; i++)
            {
                action(args, obj);
            }

            watch.Stop();

            Console.WriteLine((double)watch.ElapsedMilliseconds * 1000 / N);
        }

        static void Main(string[] args)
        {
            Test((values, parameters) => parameters.SetValues(values), 1000000);

            Test((values, paremeters) => paremeters.Coefficient = values[0], 1000000);
        }
    }
}
