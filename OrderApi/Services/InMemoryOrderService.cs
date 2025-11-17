using OrderApi.Models;

namespace OrderApi.Services
{
    // -----------------------------------------------------------------------------
    // File: InMemoryOrderService.cs
    // Project: OrderSolutions - Backend API
    // Description: Provides in-memory storage and operations for orders, used as a mock database.
    // Author: Srikanta B U
    // -----------------------------------------------------------------------------

    public class InMemoryOrderService : IOrderService
    {
        private readonly List<Order> _orders;
        private readonly ILogger<InMemoryOrderService> _logger;

        public InMemoryOrderService(ILogger<InMemoryOrderService> logger)
        {
            _logger = logger;
            _orders = Seed();
        }

        private List<Order> Seed(int count = 55)
        {
            var rnd = new Random();

            string[] names = { "John", "Srikanta", "Mark", "Swaroop", "Alice", "Bob", "Sarah", "Todd", "Emma", "Sophia" };

            var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            var orders = new List<Order>();

            for (int i = 1; i <= count; i++)
            {
                var order = new Order
                {
                    Id = i,
                    CustomerName = $"{names[rnd.Next(names.Length)]}",
                    Amount = Math.Round(rnd.NextDouble() * 5000, 2),
                    Status = statuses[rnd.Next(statuses.Length)],
                    CreatedDate = DateTime.UtcNow.AddDays(-rnd.Next(1, 30))
                };

                orders.Add(order);

                // _logger.LogInformation(
                // "Seeded order {OrderId} for customer {CustomerName} with amount {Amount} and status {Status}",
                // order.Id,
                // order.CustomerName,
                // order.Amount,
                // order.Status);
            }

            _logger.LogInformation("Seeded {Count} orders into in-memory store", orders.Count);

            return orders;
        }

        public IQueryable<Order> GetQueryable() => _orders.AsQueryable();

        public Order? GetById(int id) =>
            _orders.FirstOrDefault(o => o.Id == id);


        public bool UpdateStatus(int id, OrderStatus newStatus)
        {
            var order = GetById(id);
            if (order == null)
            {
                _logger.LogWarning("Attempted to update status for non-existent order {OrderId}", id);
                return false;
            }

            var oldStatus = order.Status;
            order.Status = newStatus;

            _logger.LogInformation(
                "Order {OrderId} status updated from {OldStatus} to {NewStatus}",
                id,
                oldStatus,
                newStatus
            );

            return true;
        }
    }
}