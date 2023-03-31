using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using System.Net;
using RapidAuto.Utilisateurs.API.Data;
using RapidAuto.Utilisateurs.API.Interfaces;
using RapidAuto.Utilisateurs.API.Services;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RapidAuto.Utilisateurs.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();
builder.Services.AddDbContext<UtilisateurContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers()
    //Modifie le comportement de ASP.NET Core Web API pour supprimer la validation du modelstate afin qu'elle puisse se faire 
    //dans le controleur
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de gestion de comptes de la compagnie RapidAuto",
        Version = "v1",
        Description = "Système permettant de faire des demandes et consultations  des comptes des utilisateurs",
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("http://www.apache.org")
        },
        Contact = new OpenApiContact
        {
            Name = "Emilio,Mohamed et Olena"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Utilisateurs.API v1"));

}
else
{
    app.UseExceptionHandler(appError =>
    {
        appError.Run(async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlerPathFeature?.Error is ArgumentException)
            {
                await context.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Aucune reponse pour cette requête"
                }.ToString());
            }
            else
            {
                await context.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error."
                }.ToString());
            }
        });
    });
}
    app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
