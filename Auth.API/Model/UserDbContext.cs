using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.API.Model
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options): base(options)
        {
            
        }

        public DbSet<User> users { get; set; }

    }

    [Table("users")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId { get; set; }
        public string emailId { get; set; }
        public string password { get; set; }
        public DateTime createdDate { get; set; }
        public string fullName { get; set; }
        public string mobileNo { get; set; }
        public string role { get; set; } = "user";

    }
}
