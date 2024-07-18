using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<AppDbContext>(options => {
//    options.UseMongoDB()
//});

var mongoClient = new MongoClient("<Your MongoDB Connection URI>");
var dbContextOptions =
    new DbContextOptionsBuilder<AppDbContext>().UseMongoDB(mongoClient, "<Database Name>");

var app = builder.Build();

// create scope to create dbcontext
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.Patient.Any())
    {
        db.Patient.AddRange(new[] {
            new Patient { Name = "Google", OrderId = 1 },
            new Patient { Name = "Meta", OrderId = 2 },
            new Patient { Name = "YouTube", OrderId = 3 },
            new Patient { Name = "Tesla", OrderId = 4 },
            new Patient { Name = "Microsoft", OrderId = 5 },
        });
        db.SaveChanges();
    }
}

app.UseFileServer(new FileServerOptions
{
    RequestPath = "",
    FileProvider = new Microsoft.Extensions.FileProviders
                    .ManifestEmbeddedFileProvider(
        typeof(Program).Assembly, "ui"
    )
});

app.RegisterCrudEndPoints();

app.Run();
