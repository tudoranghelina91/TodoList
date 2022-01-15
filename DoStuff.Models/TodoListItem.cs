using Newtonsoft.Json;

namespace DoStuff.Models
{
    public class TodoListItem
    {
        public int Id { get; set; }
        public int TodoListId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        
        [JsonIgnore]
        public TodoList TodoList { get; set; }
    }
}
