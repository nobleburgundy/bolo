using Models;
using Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepository<Game>, GamesRepository>();
builder.Services.AddScoped<IRepository<Player>, PlayersRepository>();
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

// app.MapFallbackToFile("index.html"); ;

// Specify the port here
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
// app.Run($"http://localhost:{port}");
app.Run();
