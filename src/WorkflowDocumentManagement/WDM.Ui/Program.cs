using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WDM.Applicatiion.Interfaces;
using WDM.Applicatiion.Services;
using WDM.Domain.Repositories;
using WDM.Domain.Services;
using WDM.Infrastructure;
using WDM.Infrastructure.Repositories;
using WDM.Infrastructure.Security;
using WDM.Ui.Middlewares;
using WDM.Ui.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Configuration
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));
// In your Razor Pages project (Program.cs or Startup.cs)
builder.Services.AddHttpClient<IUserService, UserService>(client => {
    client.BaseAddress = new Uri("https://localhost:7194/api/"); // Your API base URL
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSession();
app.UseRouting();
app.UseMiddleware<NoCacheMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/Login"));

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
