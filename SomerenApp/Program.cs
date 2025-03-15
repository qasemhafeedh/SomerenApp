using Microsoft.EntityFrameworkCore;
using SomerenApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Ensure the connection string is being read from appsettings.json
var connectionString = "Server=tcp:qasemhafeedh.database.windows.net,1433;Initial Catalog=qasemhafeedh;Persist Security Info=False;User ID=qasemhafeedh;Password=QaSeM*7/6.5;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";


if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("❌ ERROR: Database connection string is missing in appsettings.json");
}

// Use SQL Server with Entity Framework
builder.Services.AddDbContext<SomerenDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


