using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoStuff.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public byte[] Salt { get; set; }
        public string AccessToken { get; set; }
        public long ExpiresIn { get; set; }

        public IEnumerable<TodoList> TodoLists { get; set; }

        public User()
        {
            this.TodoLists = new HashSet<TodoList>();
        }
    }
}
