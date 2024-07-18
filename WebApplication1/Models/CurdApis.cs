using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace WebApplication1
{
    public static class CrudApis
    {
        public static void RegisterCrudEndPoints(this WebApplication app)
        {
            var bookmarkGroup = app.MapGroup("/main");
            bookmarkGroup.MapGet("/list/Patient", GetPatients);
            bookmarkGroup.MapGet("/list/Order", GetOrders);
            bookmarkGroup.MapPost("/add/Order", AddOrder);
            bookmarkGroup.MapPut("/edit/Order/{id}", EditOrder);
        }

        private static async Task<Ok<Patient[]>> GetPatients(AppDbContext db)
        {
            var list = await db.Patient.ToArrayAsync();
            return TypedResults.Ok(list);
        }

        private static async Task<Ok<Order[]>> GetOrders(AppDbContext db)
        {
            var list = await db.Order.ToArrayAsync();
            return TypedResults.Ok(list);
        }

        private static async Task<Results<Ok<Order>, BadRequest<List<ValidationResult>>>> AddOrder(AppDbContext db, Order order)
        {
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(order, new ValidationContext(order), results, true);
            if (!isValid) return TypedResults.BadRequest(results);
            db.Order.Add(order);
            await db.SaveChangesAsync();
            return TypedResults.Ok(order);
        }

        private static async Task<Results<Ok<Order>, BadRequest<List<ValidationResult>>>> EditOrder(AppDbContext db, Order order)
        {
            var results = new List<ValidationResult>();
            var rv = await db.Order.FindAsync(order.Id);
            if (rv == null) return TypedResults.BadRequest(results);
            var isValid = Validator.TryValidateObject(order, new ValidationContext(order), results, true);
            if (!isValid) return TypedResults.BadRequest(results);
            db.Order.Update(order);            
            await db.SaveChangesAsync();
            return TypedResults.Ok(order);
        }

        private static async Task<Results<Ok<string>, NotFound>> DeleteOrder(AppDbContext db, int id)
        {
            var order = await db.Order.FindAsync(id);
            if (order == null) return TypedResults.NotFound();
            db.Order.Remove(order);
            await db.SaveChangesAsync();
            return TypedResults.Ok("OK");
        }
    }
}