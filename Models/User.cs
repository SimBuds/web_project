using Microsoft.AspNetCore.Identity;

namespace web_project.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public int Role { get; set; }

        public User() { }
    }
}
