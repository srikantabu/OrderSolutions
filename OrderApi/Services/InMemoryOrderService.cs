using OrderApi.Models;

namespace OrderApi.Services
{
    public class InMemoryOrderService : IOrderService
    {
        private readonly List<Order> _orders;

        public InMemoryOrderService()
        {
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
                string customerName = $"{names[rnd.Next(names.Length)]}";

                orders.Add(new Order
                {
                    Id = i,
                    CustomerName = customerName,
                    Amount = Math.Round(rnd.NextDouble() * 5000, 2),
                    Status = statuses[rnd.Next(statuses.Length)],
                    CreatedDate = DateTime.UtcNow.AddDays(-rnd.Next(1, 30))
                });
            }

            return orders;
        }

        public IQueryable<Order> GetQueryable() => _orders.AsQueryable();

        public Order? GetById(int id) =>
            _orders.FirstOrDefault(o => o.Id == id);


        public bool UpdateStatus(int id, OrderStatus newStatus)
        {
            var order = GetById(id);
            if (order == null) return false;

            order.Status = newStatus;
            return true;
        }
    }
}