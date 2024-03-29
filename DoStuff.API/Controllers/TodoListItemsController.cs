﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoStuff.Models;
using DoStuff.DAL;
using Microsoft.EntityFrameworkCore;
using DoStuff.Models.Settings;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoStuff.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListItemsController : ControllerBase
    {
        private readonly TodoListContext _context;
        private readonly JwtSettings _jwtSettings;
        public TodoListItemsController(TodoListContext context, JwtSettings jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }

        // GET: api/<TodoListItemsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListItem>>> Get()
        {
            var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

            if (u == null)
            {
                return Unauthorized();
            }

            return await _context.TodoListItems.Where(t => t.TodoList.User == u).ToListAsync();
        }

        // GET api/<TodoListItemsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListItem>> Get(int id)
        {
            var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

            if (u == null)
            {
                return Unauthorized();
            }

            return await _context.TodoListItems.FirstOrDefaultAsync(t => t.Id == id && t.TodoList.User == u);
        }

        [HttpGet("{list}/{page}/{count}")]
        public async Task<ActionResult<IEnumerable<TodoListItem>>> Get(int list, int page, int count)
        {
            var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

            if (u == null)
            {
                return Unauthorized();
            }

            return await _context.TodoListItems
                .Where(tdi => tdi.TodoListId == list && tdi.TodoList.User == u)
                .OrderByDescending(tdi => tdi.Id)
                .Skip((page - 1) * count)
                .Take(count).ToListAsync();
        }

        [HttpGet("GetItemsCount/{list}")]
        public async Task<ActionResult<int>> GetItemsCount(int list)
        {
            var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

            if (u == null)
            {
                return Unauthorized();
            }

            return await _context.TodoListItems
                .Where(t => t.TodoListId == list && t.TodoList.User == u)
                .CountAsync();
        }

        // POST api/<TodoListItemsController>
        [HttpPost]
        public async Task<ActionResult<TodoListItem>> Post(TodoListItem todoListItem)
        {
            try
            {
                var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

                if (u == null)
                {
                    return Unauthorized();
                }

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
                var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

                if (u == null)
                {
                    return Unauthorized();
                }

                TodoListItem tdi = await _context.TodoListItems.FirstOrDefaultAsync(t => t.Id == todoListItem.Id && t.TodoList.User == u);

                if (tdi == null)
                {
                    return NotFound();
                }

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
                var u = await _context.Users.SingleOrDefaultAsync(u => u.Email == TokenUtil.GetUserEmail(Request.Headers["Authorization"], _jwtSettings));

                if (u == null)
                {
                    return Unauthorized();
                }

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