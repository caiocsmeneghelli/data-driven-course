using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewShop.Data;
using NewShop.Models;

namespace NewShop.Controllers
{
    [Route("products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context.Products.Include(x => x.Category).AsNoTracking().ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context)
        {
            var product = await context.Products.Include(x => x.Category).AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return NotFound(new { message = "Produto não encontrado." });
            return Ok(product);
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<ActionResult<List<Product>>> GetByCategoriesId(int id, [FromServices] DataContext context)
        {
            var products = await context.Products.Include(x => x.Category).AsNoTracking().Where(x => x.CategoryId == id).ToListAsync();
            if (products == null)
                return NotFound(new { message = "Nenhum produto nessa categoria." });
            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "batman, robin")]
        public async Task<ActionResult<Product>> Post(
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
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel criar a produto." });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "batman, robin")]
        public async Task<ActionResult<Product>> Put(
            int id,
            [FromBody] Product model,
            [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                context.Entry<Product>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro ja foi atualizado." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Falha ao atualizar o produto no banco." });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "batman")]
        public async Task<ActionResult<Product>> Delete(int id, [FromServices] DataContext context)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return NotFound(new { message = "Produto não encontrado." });
            try
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return Ok(product);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover o produto." });
            }
        }
    }
}