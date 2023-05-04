using Microsoft.EntityFrameworkCore;
using System.Globalization;
using demo.Models;
using demo.Models.Interfaces;
using demo.Models.Mocks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IDbUpload, DbUploadMock>();
builder.Services.AddTransient<ICharts, ChartsMock>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DbUpload}/{action=UploadFile}/{id?}");

app.Run();