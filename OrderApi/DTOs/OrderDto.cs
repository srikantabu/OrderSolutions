namespace OrderApi.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; } = default;
        public double Amount { get; set; }
        public string? Status { get; set; } = default;
        public DateTime CreatedDate { get; set; }
    }
}