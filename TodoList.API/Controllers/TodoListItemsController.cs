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
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TodoListItemsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
