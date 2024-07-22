using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using WebApplication1;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration; // ���o IConfiguration
var mongoDbSettings = config.GetSection("MongoDatabase:ConnectionString").Value;
var mongoClient = new MongoClient(mongoDbSettings);

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseMongoDB(mongoClient, "Main");
});

builder.Services.AddSwaggerGen(opt => {
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Interview Exam",
        Description = "��ݱĥ�ASP.NET Core �إ� Minimal API�ä䴩 OpenAPI �зǡC"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // �[�J CORS �A��
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
            new Patient { Id = 1, Name = "���Ӯ�", OrderId = 1 },
            new Patient { Id = 2, Name = "���~��", OrderId = 2 },
            new Patient { Id = 3, Name = "���ɧ�", OrderId = 3 },
            new Patient { Id = 4, Name = "�i�w��", OrderId = 4 },
            new Patient { Id = 5, Name = "�d�l��", OrderId = 5 },
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
