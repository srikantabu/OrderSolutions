namespace OrderApi.Models
{
    // -----------------------------------------------------------------------------
    // File: Order.cs
    // Project: OrderSolutions - Backend API
    // Description: Represents an order entity including customer details, status, and amount.
    // Author: Srikanta B U
    // -----------------------------------------------------------------------------
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public double Amount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
