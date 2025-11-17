namespace OrderApi.DTOs
{
    public class OrdersResponse
    {
        public IEnumerable<OrderDto> Data {get; set;} = Enumerable.Empty<OrderDto>();
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }
}