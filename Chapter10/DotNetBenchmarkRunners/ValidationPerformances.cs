using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Net.Http.Json;

namespace DotNetBenchmarkRunners
{
    [SimpleJob(RuntimeMoniker.Net60, baseline: true)]
    //[RPlotExporter]
    [JsonExporter]
    public class ValidationPerformances
    {
        private readonly HttpClient clientMinimal = new HttpClient();
        private readonly HttpClient clientControllers = new HttpClient();
        private readonly ValidationData data = new ValidationData()
        {
            Id = 1,
            Description = "Performance"
        };

        [GlobalSetup]
        public void Setup()
        {
            clientMinimal.BaseAddress = new Uri("https://localhost:7059");
            clientControllers.BaseAddress = new Uri("https://localhost:7149");

        }

        [Benchmark]
        public async Task CallValidationPostMinimal() => await clientMinimal.PostAsJsonAsync("/validations", data);

        [Benchmark]
        public async Task CallValidationPostController() => await clientControllers.PostAsJsonAsync("/validations", data);
    }

    public class ValidationData
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
