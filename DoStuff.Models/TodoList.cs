using Newtonsoft.Json;
using System.Collections.Generic;

namespace DoStuff.Models
{
    public class TodoList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<TodoListItem> TodoListItems { get; set; }

        [JsonIgnore]
        public long UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        public TodoList()
        {
            TodoListItems = new HashSet<TodoListItem>();
        }
    }
}
