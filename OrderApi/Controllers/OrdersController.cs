using Microsoft.AspNetCore.Mvc;
using OrderApi.DTOs;
using OrderApi.Models;
using OrderApi.Services;

namespace OrderApi.Controllers
{

    // -----------------------------------------------------------------------------
    // File: OrdersController.cs
    // Project: OrderSolutions - Backend API
    // Description: Controller exposing API endpoints for retrieving and updating orders.
    // Author: Srikanta B U
    // -----------------------------------------------------------------------------

    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
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
            _logger.LogInformation(
                "GET /orders called with page={Page}, limit={Limit}, search={Search}, status={Status}, sortBy={SortBy}, order={Order}",
                page, limit, search, status, sortBy, order
            );

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
            _logger.LogInformation(
                "POST /orders/{OrderId}/status called with payload {@Request}",
                id, updateStatusRequest
            );

            if (string.IsNullOrWhiteSpace(updateStatusRequest.Status))
            {
                _logger.LogWarning("Status update for order {OrderId} failed: missing status", id);
                return BadRequest(new { message = "Status is required" });
            }

            if (!Enum.TryParse<OrderStatus>(updateStatusRequest.Status, true, out var newStatus))
            {
                _logger.LogWarning("Status update for order {OrderId} failed: invalid status {Status}", id, updateStatusRequest.Status);
                return BadRequest(new { message = "Invalid status value" });
            }

            var updated = _orderService.UpdateStatus(id, newStatus);

            if (!updated)
            {
                _logger.LogWarning("Status update for order {OrderId} failed: order not found", id);
                return NotFound(new { message = $"Order with id '{id}' not found" });
            }

            _logger.LogInformation("Status update for order {OrderId} succeeded, new status {NewStatus}", id, newStatus);

            return Ok(new { message = "Status updated successfully" });
        }
    }
}