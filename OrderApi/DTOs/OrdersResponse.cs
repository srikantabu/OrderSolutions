namespace OrderApi.DTOs
{
    // -----------------------------------------------------------------------------
    // File: OrdersResponse.cs
    // Project: OrderSolutions - Backend API
    // Description: Wrapper DTO for paginated orders response structure.
    // Author: Srikanta B U
    // -----------------------------------------------------------------------------
    public class OrdersResponse
    {
        public IEnumerable<OrderDto> Data { get; set; } = Enumerable.Empty<OrderDto>();
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }
}