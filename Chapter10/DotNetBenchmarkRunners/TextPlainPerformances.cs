using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarkRunners
{
    [SimpleJob(RuntimeMoniker.Net60, baseline: true)]
    //[RPlotExporter]
    [JsonExporter]
    public class TextPlainPerformances
    {
        private readonly HttpClient clientMinimal = new HttpClient();
        private readonly HttpClient clientControllers = new HttpClient();

        [GlobalSetup]
        public void Setup()
        {
            clientMinimal.BaseAddress = new Uri("https://localhost:7059");
            clientControllers.BaseAddress = new Uri("https://localhost:7149");
        }

        [Benchmark]
        public async Task CallTextPlainMinimal() => await clientMinimal.GetAsync("/text-plain");

        [Benchmark]
        public async Task CallTextPlainController() => await clientControllers.GetAsync("/text-plain");
    }
}
