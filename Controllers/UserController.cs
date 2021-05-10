using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewShop.Data;
using NewShop.Models;
using NewShop.Services;

namespace NewShop.Controllers
{
    [Route("users")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model)
        {
            // Verificar se os dados estão validos
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                context.Users.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel criar o usuario." });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
        {
            var user = await context.Users
            .AsNoTracking()
            .Where(x => x.Username == model.Username && x.Password == model.Password)
            .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "Usuario ou senha inválidos." });
            }
            var token = TokenService.GenerateToken(user);
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "batman, robin")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            var users = await context.Users.AsNoTracking().ToListAsync();
            return Ok(users);
        }
    }
}