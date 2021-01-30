using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;
using TodoList.DAL;
using Microsoft.EntityFrameworkCore;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListItemsController : ControllerBase
    {
        private readonly TodoListContext _context;
        public TodoListItemsController(TodoListContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // GET: api/<TodoListItemsController>
        [HttpGet]
        public async Task<IEnumerable<TodoListItem>> Get()
        {
            return await _context.TodoListItems.ToListAsync();
        }

        [HttpGet("{page}/{count}")]
        public async Task<IEnumerable<TodoListItem>> Get(int page, int count)
        {
                return await _context.TodoListItems.OrderByDescending(tdi => tdi.Id)
                .Skip(page * count - count)
                .Take(count).ToListAsync();
        }

        [HttpGet("GetItemsCount")]
        public async Task<int> GetItemsCount()
        {
            return await _context.TodoListItems.CountAsync();
        }

        // GET api/<TodoListItemsController>/5
        [HttpGet("{id}")]
        public async Task<TodoListItem> Get(int id)
        {
            return await _context.TodoListItems.FirstOrDefaultAsync(t => t.Id == id);
        }

        // POST api/<TodoListItemsController>
        [HttpPost]
        public async Task<ActionResult<TodoListItem>> Post([FromBody] TodoListItem todoListItem)
        {
            try
            {
                var t = await _context.TodoListItems.AddAsync(todoListItem);
                await _context.SaveChangesAsync();
                return t.Entity;
            }
            catch
            {
                return BadRequest();
            }
        }

        // PUT api/<TodoListItemsController>/5
        [HttpPut]
        public async Task<ActionResult<TodoListItem>> Put([FromBody] TodoListItem todoListItem)
        {
            try
            {
                TodoListItem tdi = await _context.TodoListItems.FindAsync(todoListItem.Id);
                tdi.Name = todoListItem.Name;
                tdi.Description = todoListItem.Description;
                tdi.Completed = todoListItem.Completed;
                await _context.SaveChangesAsync();
                return tdi;
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE api/<TodoListItemsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoListItem>> Delete(int id)
        {
            try
            {
                TodoListItem tdi = await _context.TodoListItems.FindAsync(id);
                if (tdi != null)
                {
                    var t = _context.Remove(tdi);
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