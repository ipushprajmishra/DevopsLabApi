using DevopsLabApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevopsLabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> Products = new()
    {
        new Product
        {
            Id = 1,
            Name = "Laptop",
            Description = "Developer laptop",
            Price = 75000,
            IsActive = true
        },
        new Product
        {
            Id = 2,
            Name = "Monitor",
            Description = "27 inch monitor",
            Price = 25000,
            IsActive = true
        }
    };

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Products);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var product = Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            product.Id = Products.Count == 0
                ? 1
                : Products.Max(x => x.Id) + 1;

            product.CreatedAtUtc = DateTime.UtcNow;

            Products.Add(product);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                product);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Product request)
        {
            var product = Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.IsActive = request.IsActive;
            product.UpdatedAtUtc = DateTime.UtcNow;

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var product = Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            Products.Remove(product);

            return NoContent();
        }
    }
}
