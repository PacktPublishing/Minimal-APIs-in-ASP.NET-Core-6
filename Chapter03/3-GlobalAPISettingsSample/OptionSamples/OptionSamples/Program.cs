using Microsoft.Extensions.Options;
using OptionSamples;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var startupConfig = builder.Configuration.GetSection(nameof(MyCustomStartupObject)).Get<MyCustomStartupObject>();
builder.Services.Configure<OptionBasic>(builder.Configuration.GetSection("OptionBasic"));
builder.Services.Configure<OptionMonitor>(builder.Configuration.GetSection("OptionMonitor"));
builder.Services.Configure<OptionSnapshot>(builder.Configuration.GetSection("OptionSnapshot"));
builder.Services.Configure<OptionCustomName>("CustomName1", builder.Configuration.GetSection("CustomName1"));
builder.Services.Configure<OptionCustomName>("CustomName2", builder.Configuration.GetSection("CustomName2"));
builder.Services.AddOptions<ConfigWithValidation>()
            .Bind(builder.Configuration.GetSection(nameof(ConfigWithValidation)))
            .ValidateDataAnnotations();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/read/options", (IOptions<OptionBasic> optionsBasic,
    IOptionsMonitor<OptionMonitor> optionsMonitor,
    IOptionsSnapshot<OptionSnapshot> optionsSnapshot,
    IOptionsFactory<OptionCustomName> optionsFactory,
    IOptions<ConfigWithValidation> optionsValidation) =>
{

    return Results.Ok(new
    {
        Basic = optionsBasic.Value,
        Monitor = optionsMonitor.CurrentValue,
        Snapshot = optionsSnapshot.Value,
        Custom1 = optionsFactory.Create("CustomName1"),
        Custom2 = optionsFactory.Create("CustomName2"),
        Validation = optionsValidation.Value
    });
})
.WithName("ReadOptions");

app.MapGet("/read/configurations", (IConfiguration configuration, IWebHostEnvironment environment) =>
{
    var priority = configuration.GetValue<string>("Priority");
    var customObject = configuration.GetSection(nameof(MyCustomObject)).Get<MyCustomObject>();
    return Results.Ok(new
    {
        MyCustomValue = configuration.GetValue<string>("MyCustomValue"),
        ConnectionString = configuration.GetConnectionString("Default"),
        CustomObject = customObject,
        StartupObject = startupConfig,
        Priority = $"Value: {priority} - from Enviroment: {environment.EnvironmentName}"
    });
})
.WithName("ReadConfigurations");

app.Run();

namespace OptionSamples
{
    public class OptionBasic
    {
        public string? Value { get; init; }
    }

    public class OptionSnapshot
    {
        public string? Value { get; init; }
    }

    public class OptionMonitor
    {
        public string? Value { get; init; }
    }

    public class OptionCustomName
    {
        public string? Value { get; init; }
    }

    public class MyCustomObject
    {
        public string? CustomProperty { get; init; }
    }
    public class MyCustomStartupObject
    {
        public string? CustomProperty { get; init; }
    }

    public class ConfigWithValidation
    {

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$")]
        public string? Email { get; set; }
        [Range(0, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int NumericRange { get; set; }
    }
}
