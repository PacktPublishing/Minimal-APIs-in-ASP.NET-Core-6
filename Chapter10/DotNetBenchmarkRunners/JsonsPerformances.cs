using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace DotNetBenchmarkRunners
{
    [SimpleJob(RuntimeMoniker.Net60, baseline: true)]
    //[RPlotExporter]
    [JsonExporter]
    public class JsonsPerformances
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
        public async Task CallJsonGetMinimal() => await clientMinimal.GetAsync("/jsons");

        [Benchmark]
        public async Task CallJsonGetController() => await clientControllers.GetAsync("/jsons");
    }
}
