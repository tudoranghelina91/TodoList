using DoStuff.DAL;
using DoStuff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoStuff.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly TodoListContext _context;
        private readonly IAuthService _authService;
        public TodoListsController(TodoListContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoList>>> Get()
        {
            var u = await _context.Users.FirstOrDefaultAsync(u => u.AccessToken == Request.Headers["Authorization"].ToString());
            if (_authService.IsAuthorized(u))
            {
                return await _context.TodoLists
                    .Where(t => t.User == u)
                    .ToListAsync();
            }
            return Unauthorized();
        }

        [HttpGet("{page}/{count}")]
        public async Task<ActionResult<IEnumerable<TodoList>>> Get(int page, int count)
        {
            var u = await _context.Users.FirstOrDefaultAsync(u => u.AccessToken == Request.Headers["Authorization"].ToString());

            if (_authService.IsAuthorized(u))
            {
                return await _context.TodoLists
                    .Where(t => t.User == u)
                    .OrderByDescending(td => td.Id)
                    .Skip((page - 1) * count)
                    .Take(count).ToListAsync();
            }

            return Unauthorized();
        }

        [HttpGet("GetItemsCount")]
        public async Task<ActionResult<int>> GetItemsCount()
        {
            var u = await _context.Users.FirstOrDefaultAsync(u => u.AccessToken == Request.Headers["Authorization"].ToString());
            if (_authService.IsAuthorized(u))
            {
                return await _context.TodoLists
                    .Where(t => t.User == u)
                    .CountAsync();
            }

            return Unauthorized();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> Get(int id)
        {
            var u = await _context.Users.FirstOrDefaultAsync(u => u.AccessToken == Request.Headers["Authorization"].ToString());
            if (_authService.IsAuthorized(u))
            {
                return await _context.TodoLists.FirstOrDefaultAsync(td => td.Id == id);
            }

            return Unauthorized();
        }

        [HttpPost]
        public async Task<ActionResult<TodoList>> Post(TodoList todoList)
        {
            try
            {
                var u = await _context.Users.FirstOrDefaultAsync(u => u.AccessToken == Request.Headers["Authorization"].ToString());
                if (_authService.IsAuthorized(u))
                {
                    var t = await _context.TodoLists.AddAsync(todoList);
                    t.Entity.UserId = u.Id;
                    t.Entity.User = u;
                    await _context.SaveChangesAsync();
                    return t.Entity;
                }

                return Unauthorized();

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
                var u = await _context.Users.FirstOrDefaultAsync(u => u.AccessToken == Request.Headers["Authorization"].ToString());
                if (_authService.IsAuthorized(u))
                {
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

                return Unauthorized();

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
                var u = await _context.Users.FirstOrDefaultAsync(u => u.AccessToken == Request.Headers["Authorization"].ToString());
                if (_authService.IsAuthorized(u))
                {
                    TodoList td = await _context.TodoLists.FirstOrDefaultAsync(td => td.Id == id);
                    if (td != null)
                    {
                        var t = _context.Remove(td);
                        await _context.SaveChangesAsync();
                        return t.Entity;
                    }

                    return null;
                }

                return Unauthorized();

            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
