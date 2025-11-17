using OrderApi.Models;

namespace OrderApi.Services
{
    // -----------------------------------------------------------------------------
    // File: IOrderService.cs
    // Project: OrderSolutions - Backend API
    // Description: Interface defining operations for accessing and modifying orders.
    // Author: Srikanta B U
    // -----------------------------------------------------------------------------
    public interface IOrderService
    {
        IQueryable<Order> GetQueryable();
        Order? GetById(int Id);
        bool UpdateStatus(int Id, OrderStatus newStatus);
    }
}