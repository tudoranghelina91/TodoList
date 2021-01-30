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
        public DbSet<TodoListItem> TodoListItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=TodoList.db;");
    }
}
