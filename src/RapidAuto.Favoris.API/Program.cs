using RapidAuto.Favoris.API;
using RapidAuto.Favoris.API.Interface;
using RapidAuto.Favoris.API.Models;
using RapidAuto.Favoris.API.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Enregistrement du service de mise en cache memoire

builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 50;
});
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
//builder.Services.AddScoped<IRepository<Vehicule>, Repository<Vehicule>>();
builder.Services.AddScoped<IFavorisService, FavorisService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
