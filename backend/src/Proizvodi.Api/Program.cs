using Proizvodi.Api;
using Proizvodi.Api.Data;
using Proizvodi.Api.Features.Proizvodi;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("nesto", c => c.BaseAddress = new Uri("https://dummyjson.com"));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var connString="Data Source=Proizvodi.db";
builder.Services.AddSqlite<ProizvodiContext>(connString);

var app = builder.Build();

app.UseExceptionHandler();
app.MapProizvodiEndpoints();
app.MigrateDd();

app.Run();
