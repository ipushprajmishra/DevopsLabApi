using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace KubernetesMetricsApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{

    private static readonly Counter OrdersProcessed =
    Metrics.CreateCounter(
        "orders_processed_total",
        "Total number of orders processed.");

    private static readonly Gauge ActiveOrders =
        Metrics.CreateGauge(
            "active_orders",
            "Current number of active orders.");

    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(new[]
        {
            new { Id = 1, Name = "Laptop" },
            new { Id = 2, Name = "Keyboard" },
            new { Id = 3, Name = "Mouse" }
        });
    }

    [HttpGet("CreateOrder")]
    public IActionResult CreateOrder()
    {
        ActiveOrders.Inc();

        return Ok(new
        {
            Message = "Order created",
            ProductId = 1,
            Quantity = 1
        });
    }
    [HttpPost("CompleteOrder")]
    public IActionResult CompleteOrder()
    {
        ActiveOrders.Dec();

        return Ok(new
        {
            Message = "Order completed"
        });
    }

    [HttpGet("failure")]
    public IActionResult Failure()
    {
        throw new InvalidOperationException(
            "Intentional test failure");
    }
}