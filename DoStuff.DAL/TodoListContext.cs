﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoListItem> TodoListItems { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
