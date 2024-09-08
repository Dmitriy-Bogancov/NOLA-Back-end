using NOLA_API;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

app.ConfigureMiddleware();

await app.RunAsync();
