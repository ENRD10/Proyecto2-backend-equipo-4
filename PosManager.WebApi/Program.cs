using PosManager.Business;
using PosManager.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Register BancoData and BancoBusiness for DI
var connectionString = builder.Configuration.GetConnectionString("Lenguajes_Equipo4")
                       ?? throw new InvalidOperationException("Connection string 'Lenguajes_Equipo4' not found.");
builder.Services.AddScoped(_ => new BancoData(connectionString));
builder.Services.AddScoped<BancoBusiness>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
