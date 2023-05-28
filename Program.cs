using api_ejemplar;
using api_ejemplar.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
// Add services to the container.


// Configure the HTTP request pipeline.
// Agregar la configuración de servicios
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Configure the HTTP request pipeline.

//builder.Services.AddScoped<MIServicio>();


builder.Services.AddControllers();

//pasarle la config de los servicios

startup.ConfigureServicies(builder.Services);
var app = builder.Build();
startup.Configure(app, app.Lifetime);


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//app.UseHttpsRedirection();

//app.MapControllers();

//app.UseRouting();

app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//     var a = new ValuesController();
//    return a.Get();
//});
//AGREGAR ESTO PARA ENRUTAR

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}