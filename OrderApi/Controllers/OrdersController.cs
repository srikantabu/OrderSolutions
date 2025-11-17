using Microsoft.AspNetCore.Mvc;
using OrderApi.DTOs;
using OrderApi.Models;
using OrderApi.Services;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public ActionResult<OrdersResponse> GetOrders(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? status = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? order = "asc"
        )
        {
            if (page <= 0) page = 1;
            if (limit <= 0) page = 10;


            var query = _orderService.GetQueryable();

            //Filter
            if (!string.IsNullOrWhiteSpace(status) && !status.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                if (Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
                {
                    query = query.Where(o => o.Status == parsedStatus);
                }
            }

            //Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(o => o.Id.ToString() == term || o.CustomerName!.ToLower().Contains(term));
            }

            //Sort
            bool desc = order?.ToLower() == "desc";

            query = sortBy?.ToLower() switch
            {
                "amount" => desc ? query.OrderByDescending(o => o.Amount) : query.OrderBy(o => o.Amount),
                "createddate" => desc ? query.OrderByDescending(o => o.CreatedDate) : query.OrderBy(o => o.CreatedDate),
                _ => query.OrderBy(o => o.Id)
            };

            //Pagination
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)limit);

            var items = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    Amount = o.Amount,
                    Status = o.Status.ToString(),
                    CreatedDate = o.CreatedDate
                })
                .ToList();

            return Ok(new OrdersResponse
            {
                Data = items,
                Page = page,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });
        }

        [HttpPost("{id}/status")]
        public ActionResult UpdateOrderStatus(int id, [FromBody] UpdateStatusRequest updateStatusRequest)
        {
            if (string.IsNullOrWhiteSpace(updateStatusRequest.Status))
            {
                return BadRequest(new { message = "Status is required" });
            }

            if (!Enum.TryParse<OrderStatus>(updateStatusRequest.Status, true, out var newStatus))
            {
                return BadRequest(new { message = "Invalid status" });
            }

            var updated = _orderService.UpdateStatus(id, newStatus);

            if (!updated)
                return NotFound(new { message = $"Order {id} not found" });

            return Ok(new { message = "Status updated successfully" });
        }
    }
}