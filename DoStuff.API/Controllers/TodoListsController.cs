using DoStuff.DAL;
using DoStuff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace DoStuff.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly TodoListContext _context;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        public TodoListsController(TodoListContext context, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _context = context;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoList>>> Get()
        {
            var token = Convert.ToString(Request.Headers["Authorization"]).Replace("Bearer", string.Empty).TrimStart();
            var jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(token);
            var u = await _context.Users.FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(jwtSecurityToken.Subject));

            if (u == null)
            {
                return Unauthorized();
            }

            return await _context.TodoLists
                    .Where(t => t.User == u)
                    .ToListAsync();
        }

        [HttpGet("{page}/{count}")]
        public async Task<ActionResult<IEnumerable<TodoList>>> Get(int page, int count)
        {
            var token = Convert.ToString(Request.Headers["Authorization"]).Replace("Bearer", string.Empty).TrimStart();
            var jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(token);
            var u = await _context.Users.FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(jwtSecurityToken.Subject));

            if (u == null)
            {
                return Unauthorized();
            }

            return await _context.TodoLists
                .Where(t => t.User == u)
                .OrderByDescending(td => td.Id)
                .Skip((page - 1) * count)
                .Take(count).ToListAsync();
        }

        [HttpGet("GetItemsCount")]
        public async Task<ActionResult<int>> GetItemsCount()
        {
            var token = Convert.ToString(Request.Headers["Authorization"]).Replace("Bearer", string.Empty).TrimStart();
            var jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(token);
            var u = await _context.Users.FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(jwtSecurityToken.Subject));

            if (u == null)
            {
                return Unauthorized();
            }

            return await _context.TodoLists
                    .Where(t => t.User == u)
                    .CountAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> Get(int id)
        {
            var token = Convert.ToString(Request.Headers["Authorization"]).Replace("Bearer", string.Empty).TrimStart();
            var jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(token);
            var u = await _context.Users.FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(jwtSecurityToken.Subject));

            if (u == null)
            {
                return Unauthorized();
            }

            return await _context.TodoLists.FirstOrDefaultAsync(td => td.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<TodoList>> Post(TodoList todoList)
        {
            try
            {
                var token = Convert.ToString(Request.Headers["Authorization"]).Replace("Bearer", string.Empty).TrimStart();
                var jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(token);
                var u = await _context.Users.FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(jwtSecurityToken.Subject));

                if (u == null)
                {
                    return Unauthorized();
                }

                var t = await _context.TodoLists.AddAsync(todoList);
                    t.Entity.UserId = u.Id;
                    t.Entity.User = u;
                    await _context.SaveChangesAsync();
                    return t.Entity;
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult<TodoList>> Put(TodoList todoList)
        {
            try
            {
                var token = Convert.ToString(Request.Headers["Authorization"]).Replace("Bearer", string.Empty).TrimStart();
                var jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(token);
                var u = await _context.Users.FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(jwtSecurityToken.Subject));

                if (u == null)
                {
                    return Unauthorized();
                }

                TodoList td = await _context.TodoLists.FirstOrDefaultAsync(td => td.Id == todoList.Id);
                
                if (td != null)
                {
                    td.Name = todoList.Name;
                    td.Description = todoList.Description;
                    await _context.SaveChangesAsync();
                    return td;
                }

                return NotFound();

            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoList>> Delete(int id)
        {
            try
            {
                var token = Convert.ToString(Request.Headers["Authorization"]).Replace("Bearer", string.Empty).TrimStart();
                var jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(token);
                var u = await _context.Users.FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(jwtSecurityToken.Subject));

                if (u == null)
                {
                    return Unauthorized();
                }

                TodoList td = await _context.TodoLists.FirstOrDefaultAsync(td => td.Id == id);
                if (td != null)
                {
                    var t = _context.Remove(td);
                    await _context.SaveChangesAsync();
                    return t.Entity;
                }

                return NotFound();

            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
