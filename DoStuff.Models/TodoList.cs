using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoStuff.Models
{
    public class TodoList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<TodoListItem> TodoListItems { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public TodoList()
        {
            TodoListItems = new HashSet<TodoListItem>();
        }
    }
}
