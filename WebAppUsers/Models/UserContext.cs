using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace WebAppUsers.Models
{
    public class UserContext:IdentityDbContext<User>
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

