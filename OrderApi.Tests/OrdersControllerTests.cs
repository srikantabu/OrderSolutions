using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderApi;
using OrderApi.DTOs;
using OrderApi.Models;
using Xunit;
using System.Linq;

namespace OrderApi.Tests;

// -----------------------------------------------------------------------------
// File: OrdersControllerTests.cs
// Project: OrderSolutions - Backend API
// Description: Integration tests validating OrdersController routing,
//              filtering, searching, and update behavior.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrdersControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // Ensures the orders listing endpoint responds with HTTP 200
    // and returns at least one order with valid paging parameters.
    [Fact]
    public async Task GetOrders_ShouldReturnOk_AndData()
    {
        var response = await _client.GetAsync("/v1/orders?page=1&limit=10");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<OrdersResponse>();
        Assert.NotNull(payload);
        Assert.NotEmpty(payload!.Data);
    }

    // Verifies that attempting to update a non-existent order ID
    // results in a 404 NotFound response.
    [Fact]
    public async Task UpdateStatus_ShouldReturnNotFound_ForInvalidId()
    {
        var body = new { status = "Completed" };

        var response = await _client.PostAsJsonAsync("/v1/orders/999999/status", body);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // Ensures that applying a status filter returns only orders whose status
    // exactly matches the requested value ("Pending").
    [Fact]
    public async Task GetOrders_FilterByStatus_ShouldReturnOnlyThatStatus()
    {
        var response = await _client.GetAsync("/v1/orders?page=1&limit=50&status=Pending");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<OrdersResponse>();
        Assert.NotNull(payload);
        Assert.NotEmpty(payload!.Data);

        Assert.All(payload.Data, o =>
        {
            Assert.Equal("Pending", o.Status);
        });
    }

    // Verifies that the search query correctly filters results by customer name.
    // Uses a known name from the first page of data to guarantee a valid hit.
    [Fact]
    public async Task GetOrders_SearchByCustomerName_ShouldReturnMatchingResults()
    {
        // Fetch a real customer name to use as the search term
        var initialResponse = await _client.GetAsync("/v1/orders?page=1&limit=1");
        Assert.Equal(HttpStatusCode.OK, initialResponse.StatusCode);

        var initialPayload = await initialResponse.Content.ReadFromJsonAsync<OrdersResponse>();
        Assert.NotNull(initialPayload);
        Assert.NotEmpty(initialPayload!.Data);

        var sampleCustomerName = initialPayload.Data.First().CustomerName;
        Assert.False(string.IsNullOrWhiteSpace(sampleCustomerName));

        // Execute search
        var encoded = Uri.EscapeDataString(sampleCustomerName);
        var response = await _client.GetAsync($"/v1/orders?page=1&limit=50&search={encoded}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<OrdersResponse>();
        Assert.NotNull(payload);
        Assert.NotEmpty(payload!.Data);

        // Ensure each returned record matches the searched name
        Assert.All(payload.Data, o =>
        {
            Assert.Contains(sampleCustomerName, o.CustomerName, StringComparison.OrdinalIgnoreCase);
        });
    }
}
