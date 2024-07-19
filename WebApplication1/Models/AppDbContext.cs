using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace WebApplication1
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Order> Order { get; init; }
        public DbSet<Patient> Patient { get; init; }

        public static AppDbContext Create(IMongoDatabase database) =>
            new(new DbContextOptionsBuilder<AppDbContext>()
                .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
                .Options);

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Patient>().ToCollection("patients");
            modelBuilder.Entity<Order>().ToCollection("orders");
        }
    }


    internal class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^.{3,}")]
        public string Message { get; set; }
    }

    internal class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^.{2,}")]
        public string Name { get; set; }
        [Required]
        public int OrderId { get; set; }
    }
}
