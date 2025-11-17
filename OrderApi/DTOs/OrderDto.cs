namespace OrderApi.DTOs
{
    // -----------------------------------------------------------------------------
    // File: OrderDto.cs
    // Project: OrderSolutions - Backend API
    // Description: DTO for returning order data to the client.
    // Author: Srikanta B U
    // -----------------------------------------------------------------------------
    public class OrderDto
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; } = default;
        public double Amount { get; set; }
        public string? Status { get; set; } = default;
        public DateTime CreatedDate { get; set; }
    }
}