using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HipicaFacilSQL.Data;
using static System.Formats.Asn1.AsnWriter;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<HipicaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HipicaContext") ?? throw new InvalidOperationException("Connection string 'HipicaContext' not found.")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<HipicaContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
