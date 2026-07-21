using Proizvodi.Api;
using Proizvodi.Api.Data;
using Proizvodi.Api.Features.Proizvodi;
using Proizvodi.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDummyJsonClient(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var connectionString = builder.Configuration.GetConnectionString("Proizvodi")
    ?? throw new InvalidOperationException("Connection string 'Proizvodi' is not configured.");
builder.Services.AddSqlite<ProizvodiContext>(connectionString);

var app = builder.Build();

app.UseExceptionHandler();
app.MapProizvodiEndpoints();
app.MigrateDatabase();

app.Run();

public partial class Program;
