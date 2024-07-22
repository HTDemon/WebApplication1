using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;

namespace WebApplication1
{
    public static class CrudApis
    {
        public static void RegisterCrudEndPoints(this WebApplication app)
        {
            var UrlGroup = app.MapGroup("/main");
            UrlGroup.MapGet("/list/Patient", GetPatients).WithTags("住民").WithName("GetPatientList").WithOpenApi();
            UrlGroup.MapGet("/get/Order/{id}", GetOrders).WithTags("醫囑").WithName("GetOrder").WithOpenApi();
            UrlGroup.MapPost("/add/Order", AddOrder).WithTags("醫囑").WithName("AddOrder").WithOpenApi();
            UrlGroup.MapPut("/edit/Order", EditOrder).WithTags("醫囑").WithName("EditOrder").WithOpenApi();
            UrlGroup.MapDelete("/edit/Delete/{id}", DeleteOrder).WithTags("醫囑").WithName("DeleteOrder").WithOpenApi();
        }

        private static async Task<Ok<Patient[]>> GetPatients(AppDbContext db)
        {
            var list = await db.Patient.ToArrayAsync();
            return TypedResults.Ok(list);
        }

        private static async Task<Ok<Order[]>> GetOrders(AppDbContext db, int id)
        {
            var list = await db.Order.Where(x => x.Id == id).ToArrayAsync();
            return TypedResults.Ok(list);
        }

        private static async Task<Results<Ok<Order>, BadRequest<List<ValidationResult>>>> AddOrder(AppDbContext db, AddOrder addOrder)
        {
            var results = new List<ValidationResult>();
            //var isValid = Validator.TryValidateObject(order, new ValidationContext(order), results, true);
            //if (!isValid) return TypedResults.BadRequest(results);
            var lastOrderId = db.Order.OrderByDescending(x => x.Id).FirstOrDefault();
            var p = db.Patient.Where(x => x.Id == addOrder.PatientId).FirstOrDefault();

            if (lastOrderId == null || addOrder.Message.Length == 0)
            {
                return TypedResults.BadRequest(results);
            }
            var nextOrderId = lastOrderId.Id + 1;
            var o = new Order()
            {
                Id = nextOrderId,
                Message = addOrder.Message.Trim()
            };
            db.Order.Add(o);
            p.OrderId = nextOrderId;
            await db.SaveChangesAsync();
            return TypedResults.Ok(o);
        }

        private static async Task<Results<Ok<Order>, BadRequest<List<ValidationResult>>>> EditOrder(AppDbContext db, Order order)
        {
            var results = new List<ValidationResult>();
            var rv = await db.Order.FindAsync(order.Id);
            if (rv == null) return TypedResults.BadRequest(results);
            var isValid = Validator.TryValidateObject(order, new ValidationContext(order), results, true);
            if (!isValid) return TypedResults.BadRequest(results);
            rv.Message = order.Message;
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