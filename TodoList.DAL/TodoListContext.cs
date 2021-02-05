using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TodoList.Models;

namespace TodoList.DAL
{
    public class TodoListContext : DbContext
    {
        public TodoListContext()
        {
            
        }
        public TodoListContext(DbContextOptions<TodoListContext> options) : base(options)
        {
        }
        public DbSet<TodoListItem> TodoListItems { get; set; }
    }
}
