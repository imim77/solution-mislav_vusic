using Proizvodi.Api;
using Proizvodi.Api.Features.Proizvodi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("nesto", c => c.BaseAddress = new Uri("https://dummyjson.com"));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
var app = builder.Build();

app.UseExceptionHandler();
app.MapProizvodiEndpoints();

app.Run();
