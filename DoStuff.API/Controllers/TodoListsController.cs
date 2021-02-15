using DoStuff.DAL;
using DoStuff.Models;
using Microsoft.AspNetCore.Http;
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
        public TodoListsController(TodoListContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoList>> Get()
        {
            return await _context.TodoLists.ToListAsync();
        }

        [HttpGet("{page}/{count}")]
        public async Task<IEnumerable<TodoList>> Get(int page, int count)
        {
            return await _context.TodoLists.OrderByDescending(td => td.Id)
                .Skip((page - 1) * count)
                    .Take(count).ToListAsync();
        }

        [HttpGet("GetItemsCount")]
        public async Task<int> GetItemsCount()
        {
            return await _context.TodoLists.CountAsync();
        }

        [HttpGet("{id}")]
        public async Task<TodoList> Get(int id)
        {
            return await _context.TodoLists.FirstOrDefaultAsync(td => td.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<TodoList>> Post(TodoList todoList)
        {
            try
            {
                var t = await _context.TodoLists.AddAsync(todoList);
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
                TodoList td = await _context.TodoLists.FirstOrDefaultAsync(td => td.Id == id);
                if (td != null)
                {
                    var t = _context.Remove(td);
                    await _context.SaveChangesAsync();
                    return t.Entity;
                }

                return null;
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
