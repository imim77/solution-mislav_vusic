using Proizvodi.Api.Features.Proizvodi;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddHttpClient("nesto", c => c.BaseAddress = new Uri("https://dummyjson.com"));
var app = builder.Build();

app.MapProizvodiEndpoints();

app.Run();
