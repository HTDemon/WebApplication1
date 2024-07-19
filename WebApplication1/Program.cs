using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using WebApplication1;


var builder = WebApplication.CreateBuilder(args);

//var mongoClient = new MongoClient("<Your MongoDB Connection URI>");
var mongoClient = new MongoClient("mongodb://10.24.20.181:32768");

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseMongoDB(mongoClient, "Main");
});

var app = builder.Build();

//app.UseFileServer(new FileServerOptions
//{
//    RequestPath = "",
//    FileProvider = new Microsoft.Extensions.FileProviders
//                    .ManifestEmbeddedFileProvider(
//        typeof(Program).Assembly, "frontend"
//    )
//});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.Patient.Any())
    {
        db.Patient.AddRange(new[] {
            new Patient { Id = 1, Name = "¤ý©Ó®¦", OrderId = 1 },
            new Patient { Id = 2, Name = "³¯«~§°", OrderId = 2 },
            new Patient { Id = 3, Name = "§õ«É§Ê", OrderId = 3 },
            new Patient { Id = 4, Name = "±iÐwµá", OrderId = 4 },
            new Patient { Id = 5, Name = "§d¤l´¸", OrderId = 5 },
        });
        db.Order.AddRange(new[] {
            new Order { Id = 1, Message = "This patient was put into the regimen of pain control with PCA." },
            new Order { Id = 2, Message = "D.C. all regular narcotics." },
            new Order { Id = 3, Message = "Oxygen breathing equipment has to be standby for urgency." },
            new Order { Id = 4, Message = "PCA dose: 2.0mg." },
            new Order { Id = 5, Message = "Naloxone(Narcan) 0.4mg/ml amp should be ready at bedside." },
        });
        db.SaveChanges();
    }
}

app.RegisterCrudEndPoints();

app.Run();
