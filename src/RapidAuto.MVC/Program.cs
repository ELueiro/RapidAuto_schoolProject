using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RapidAuto.MVC.Interface;
using RapidAuto.MVC.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IVehiculeMVCService, VehiculeMVCServiceProxy>(client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlVehiculeAPI")));
builder.Services.AddHttpClient<IUtilisateurMVCService, UtilisateurMVCServiceProxy>(client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPI")));
builder.Services.AddHttpClient<ICommandeMVCService, CommanderMVCServiceProxy>(client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlCommandeAPI")));
builder.Services.AddHttpClient<IFavorisMVCService, FavorisMVCServiceProxy>(client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlFavorisAPI"))); 
builder.Services.AddHttpClient<IFichierMVCService, FichierMVCServiceProxy>(client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlFichierAPI")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}else
{
    // app.UseExceptionHandler("/Home/Error");
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; ;
            context.Response.ContentType = "text/html";

            await context.Response.WriteAsync("<html lang=\"fr\"><body>\r\n");
            await context.Response.WriteAsync("Erreur!<br><br>\r\n");

            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is MethodAccessException)
            {
                await context.Response.WriteAsync(
                                          "Accès interdit!<br><br>\r\n");
            }

            await context.Response.WriteAsync(
                                          "<a href=\"/\">Accueil</a><br>\r\n");
            await context.Response.WriteAsync("</body></html>\r\n");
            await context.Response.WriteAsync(new string(' ', 512));
        });
    });
    app.UseHsts();
}
//app.UseStatusCodePages();

app.UseStatusCodePagesWithRedirects("/Home/CodeStatus?code={0}");

//app.UseStatusCodePagesWithRedirects("/Home/CodeStatus?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
