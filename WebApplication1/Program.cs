using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using WebApplication1;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration; // 取得 IConfiguration
var mongoDbSettings = config.GetSection("MongoDatabase:ConnectionString").Value;
var mongoClient = new MongoClient(mongoDbSettings);

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseMongoDB(mongoClient, "Main");
});

builder.Services.AddSwaggerGen(opt => {
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Interview Exam",
        Description = "後端採用ASP.NET Core 建立 Minimal API並支援 OpenAPI 標準。"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // 加入 CORS 服務
    const string OriginsFromSetting = "OriginsFromAppSettingsJson";
    builder.Services.AddCors(options => {
        options.AddPolicy(
            name: OriginsFromSetting,
            builder => {
                builder.WithOrigins(config.GetSection("AllowOrigins").Get<string[]>());
            }
        );
    });

    app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.Patient.Any())
    {
        db.Patient.AddRange(new[] {
            new Patient { Id = 1, Name = "王承恩", OrderId = 1 },
            new Patient { Id = 2, Name = "陳品妍", OrderId = 2 },
            new Patient { Id = 3, Name = "李宥廷", OrderId = 3 },
            new Patient { Id = 4, Name = "張苡菲", OrderId = 4 },
            new Patient { Id = 5, Name = "吳子晴", OrderId = 5 },
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

app.UseFileServer(new FileServerOptions
{
    RequestPath = "",
    FileProvider = new Microsoft.Extensions.FileProviders
                    .ManifestEmbeddedFileProvider(
        typeof(Program).Assembly, "frontend"
    )
});

app.RegisterCrudEndPoints();

app.Run();
