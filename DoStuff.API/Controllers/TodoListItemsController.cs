using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoStuff.Models;
using DoStuff.DAL;
using Microsoft.EntityFrameworkCore;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoStuff.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListItemsController : ControllerBase
    {
        private readonly TodoListContext _context;
        public TodoListItemsController(TodoListContext context)
        {
            _context = context;
        }

        // GET: api/<TodoListItemsController>
        [HttpGet]
        public async Task<IEnumerable<TodoListItem>> Get()
        {
            return await _context.TodoListItems.ToListAsync();
        }

        [HttpGet("{list}/{page}/{count}")]
        public async Task<IEnumerable<TodoListItem>> Get(int list, int page, int count)
        {
            return await _context.TodoListItems.Where(tdi => tdi.TodoListId == list).OrderByDescending(tdi => tdi.Id)
                .Skip((page - 1) * count)
                    .Take(count).ToListAsync();
        }

        [HttpGet("GetItemsCount/{list}")]
        public async Task<int> GetItemsCount(int list)
        {
            return await _context.TodoListItems.Where(t => t.TodoListId == list).CountAsync();
        }

        // GET api/<TodoListItemsController>/5
        [HttpGet("{id}")]
        public async Task<TodoListItem> Get(int id)
        {
            return await _context.TodoListItems.FirstOrDefaultAsync(t => t.Id == id);
        }

        // POST api/<TodoListItemsController>
        [HttpPost]
        public async Task<ActionResult<TodoListItem>> Post(TodoListItem todoListItem)
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
        public async Task<ActionResult<TodoListItem>> Put(TodoListItem todoListItem)
        {
            try
            {
                TodoListItem tdi = await _context.TodoListItems.FirstOrDefaultAsync(t => t.Id == todoListItem.Id);
                if (tdi != null)
                {
                    tdi.Name = todoListItem.Name;
                    tdi.Description = todoListItem.Description;
                    tdi.Completed = todoListItem.Completed;
                    await _context.SaveChangesAsync();
                    return tdi;
                }
                else
                {
                    return NotFound();
                }
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
                TodoListItem tdi = await _context.TodoListItems.FirstOrDefaultAsync(tdi => tdi.Id == id);
                if (tdi != null)
                {
                    var t = _context.Remove(tdi);
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