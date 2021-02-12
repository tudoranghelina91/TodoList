using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DoStuff.Models;

namespace DoStuff.DAL
{
    public class TodoListContext : DbContext
    {
        public TodoListContext()
        {

        }
        public TodoListContext(DbContextOptions<TodoListContext> options) : base(options)
        {
            this.Database.EnsureCreatedAsync();
        }
        public DbSet<TodoListItem> TodoListItems { get; set; }
    }
}
