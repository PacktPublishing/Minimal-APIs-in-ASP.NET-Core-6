using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Net.Http.Json;

namespace DotNetBenchmarkRunners
{
    [SimpleJob(RuntimeMoniker.Net60, baseline: true)]
    [JsonExporter]
    public class Performances
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
        public async Task Minimal_Json_Get() => await clientMinimal.GetAsync("/jsons");

        [Benchmark]
        public async Task Controller_Json_Get() => await clientControllers.GetAsync("/jsons");

        [Benchmark]
        public async Task Minimal_TextPlain_Get() => await clientMinimal.GetAsync("/text-plain");

        [Benchmark]
        public async Task Controller_TextPlain_Get() => await clientControllers.GetAsync("/text-plain");


        [Benchmark]
        public async Task Minimal_Validation_Post() => await clientMinimal.PostAsJsonAsync("/validations", data);

        [Benchmark]
        public async Task Controller_Validation_Post() => await clientControllers.PostAsJsonAsync("/validations", data);
    }

    public class ValidationData
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
