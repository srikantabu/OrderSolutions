using Microsoft.Extensions.Logging;
using Moq;
using OrderApi.Models;
using OrderApi.Services;
using Xunit;

namespace OrderApi.Tests;

// -----------------------------------------------------------------------------
// File: InMemoryOrderServiceTests.cs
// Project: OrderSolutions - Backend API
// Description: Unit tests for verifying the behavior of InMemoryOrderService.
// Author: Srikanta B U
// -----------------------------------------------------------------------------

public class InMemoryOrderServiceTests
{
    private readonly InMemoryOrderService _service;

    public InMemoryOrderServiceTests()
    {
        var logger = new Mock<ILogger<InMemoryOrderService>>();
        _service = new InMemoryOrderService(logger.Object);
    }

    // Ensures the service seeds a non-empty set of orders when constructed.
    [Fact]
    public void Seed_ShouldCreateOrders()
    {
        var queryable = _service.GetQueryable();
        Assert.True(queryable.Any());
    }

    // Verifies that updating the status of an existing order returns true
    // and that the stored order actually reflects the new status value.
    [Fact]
    public void UpdateStatus_ShouldUpdate_WhenOrderExists()
    {
        var firstOrder = _service.GetQueryable().First();
        var oldStatus = firstOrder.Status;

        var newStatus = oldStatus == OrderStatus.Completed
            ? OrderStatus.Pending
            : OrderStatus.Completed;

        var result = _service.UpdateStatus(firstOrder.Id, newStatus);

        Assert.True(result);

        var updated = _service.GetById(firstOrder.Id);

        Assert.NotNull(updated);
        Assert.Equal(newStatus, updated!.Status);
        Assert.NotEqual(oldStatus, updated.Status);
    }

    // Ensures UpdateStatus returns false when the provided ID does not match any order.
    [Fact]
    public void UpdateStatus_ShouldReturnFalse_WhenOrderNotFound()
    {
        var result = _service.UpdateStatus(999999, OrderStatus.Completed);
        Assert.False(result);
    }
}
