using BenchmarkDotNet.Running;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Benchmarks m = new Benchmarks();
            //m.Setup();
            //m.Benchmark_BXmlDocument();

            BenchmarkRunner.Run<Benchmarks>();
        }
    }
}