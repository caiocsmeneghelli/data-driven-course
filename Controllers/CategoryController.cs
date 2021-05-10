using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewShop.Data;
using NewShop.Models;

namespace NewShop.Controllers
{
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var list = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(list);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
        {
            var category = await context.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "batman, robin")]
        public async Task<ActionResult<Category>> Post([FromBody] Category model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não possivel realizar a inclusão da categoria." });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "batman, robin")]
        public async Task<ActionResult<Category>> Put(int id,
        [FromBody] Category model,
        [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro foi atualizado." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Falha ao atualizar o produto no banco." });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "batman")]
        public async Task<ActionResult<Category>> Delete(int id, [FromServices] DataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();
            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(category);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover categoria." });
            }
        }
    }
}