using CategoryTree.Helpers;
using CategoryTree.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Load connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register DbContext
builder.Services.AddDbContext<CategoryDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Resolve DbContext and use it
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CategoryDbContext>();

    var allCategories = await db.Categories
                       .OrderBy(c => c.ParentId)
                       .ThenBy(c => c.Id)
                       .ToListAsync();

    CategoryTreeHelper.GetCategoryTreeWithEF(allCategories);
}

await CategoryTreeHelper.GetCategoryTreeWithSP(connectionString);

app.Run();