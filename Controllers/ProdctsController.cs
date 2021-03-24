using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

[Route("products")]
public class ProductController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
    {
        try
        {
            var products = await context.Products.Include(x => x.Category).AsNoTracking().ToListAsync();
            return Ok(products);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possivel buscar os produtos" });
        }

    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(
        int id,
        [FromServices] DataContext context)
    {
        try
        {
            var product = await context.Products.Include(x => x.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(product);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possivel buscar o produto" });
        }

    }


    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Product>>> GetByCategory(
        int id,
        [FromServices] DataContext context)
    {
        try
        {
            var products = await context.Products.
            Include(x => x.Category)
            .AsNoTracking()
            .Where(x => x.CategoryId == id)
            .FirstOrDefaultAsync(x => x.Id == id);
            return Ok(products);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possivel buscar o produto" });
        }

    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Product>> POST(
        [FromBody] Product model,
        [FromServices] DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Products.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possivel criar produto" });
        }

    }
}