using System.Text.Json.Serialization;
using OrderApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSingleton<IOrderService, InMemoryOrderService>();

var corsPolicy = "AllowFrontend";

builder.Services.AddCors(o =>
{
    o.AddPolicy(corsPolicy, policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors(corsPolicy);

app.MapControllers();

app.Run();
