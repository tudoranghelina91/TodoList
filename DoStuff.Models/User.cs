﻿using System.Collections.Generic;

namespace DoStuff.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        
        public string HashedPassword { get; set; }

        public byte[] Salt { get; set; }

        public IEnumerable<TodoList> TodoLists { get; set; }

        public User()
        {
            this.TodoLists = new HashSet<TodoList>();
        }
    }
}
