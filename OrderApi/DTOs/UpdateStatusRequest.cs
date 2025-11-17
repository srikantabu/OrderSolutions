namespace OrderApi.DTOs
{
    // -----------------------------------------------------------------------------
    // File: UpdateStatusRequest.cs
    // Project: OrderSolutions - Backend API
    // Description: Model for updating an order’s status via POST.
    // Author: Srikanta B U
    // -----------------------------------------------------------------------------
    public class UpdateStatusRequest
    {
        public string? Status { get; set; }
    }
}