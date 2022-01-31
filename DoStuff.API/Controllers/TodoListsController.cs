using DoStuff.DAL;
using DoStuff.Models;
using DoStuff.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly JwtSettings _jwtSettings;
        public TodoListsController(TodoListContext context, JwtSettings jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoList>>> Get()
        {
            var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

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
            var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

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
            var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

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
            var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

            if (u == null)
            {
                return Unauthorized();
            }

            return await _context.TodoLists
                .Include(t => t.TodoListItems)
                .SingleOrDefaultAsync(td => td.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<TodoList>> Post(TodoList todoList)
        {
            try
            {
                var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

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
                var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

                if (u == null)
                {
                    return Unauthorized();
                }

                TodoList td = await _context.TodoLists.Include(td => td.TodoListItems).FirstOrDefaultAsync(td => td.Id == todoList.Id);
                
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
                var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

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
