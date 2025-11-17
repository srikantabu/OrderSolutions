using OrderApi.Models;

namespace OrderApi.Services
{
    public interface IOrderService
    {
        IQueryable<Order> GetQueryable();
        Order? GetById(int Id);
        bool UpdateStatus(int Id, OrderStatus newStatus);
    }
}