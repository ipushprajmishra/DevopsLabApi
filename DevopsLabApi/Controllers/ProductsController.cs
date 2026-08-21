using DevopsLabApi.Data;
using DevopsLabApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevopsLabApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ProductsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _dbContext.Products.ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        product.Id = 0;
        product.CreatedAtUtc = DateTime.UtcNow;

        _dbContext.Products.Add(product);

        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Product request)
    {
        var product = await _dbContext.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.IsActive = request.IsActive;
        product.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        _dbContext.Products.Remove(product);

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}