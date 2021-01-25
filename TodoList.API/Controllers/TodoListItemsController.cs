using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;
using TodoList.DAL;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListItemsController : ControllerBase
    {
        // GET: api/<TodoListItemsController>
        [HttpGet]
        public async Task<List<TodoListItem>> Get()
        {
            using (var context = new TodoListContext())
            {
                return await Task.Run(() =>
                {
                    return context.TodoListItems.ToList();
                });
            }
        }

        // GET api/<TodoListItemsController>/5
        [HttpGet("{id}")]
        public TodoListItem Get(int id)
        {
            using (var context = new TodoListContext())
            {
                return context.TodoListItems.FirstOrDefault(t => t.Id == id);
            }
        }

        // POST api/<TodoListItemsController>
        [HttpPost]
        public async Task<ActionResult<TodoListItem>> Post([FromBody] TodoListItem todoListItem)
        {
            using (var context = new TodoListContext())
            {
                var t = await context.TodoListItems.AddAsync(todoListItem);
                try
                {
                    await context.SaveChangesAsync();
                    return t.Entity;
                }
                catch
                {
                    return BadRequest();
                }
            }
        }

        // PUT api/<TodoListItemsController>/5
        [HttpPut]
        public async Task<ActionResult<TodoListItem>> Put([FromBody] TodoListItem todoListItem)
        {
            using (var context = new TodoListContext())
            {
                try
                {
                    TodoListItem tdi = context.TodoListItems.Find(todoListItem.Id);
                    tdi.Name = todoListItem.Name;
                    tdi.Description = todoListItem.Description;
                    tdi.Completed = todoListItem.Completed;
                    await context.SaveChangesAsync();
                    return tdi;
                }
                catch
                {
                    return BadRequest();
                }
            }
        }

        // DELETE api/<TodoListItemsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
