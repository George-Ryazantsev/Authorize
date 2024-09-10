using BookLib_Auth_Mapp_NF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLib_Auth_Mapp_NF.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            
        }        
        public DbSet<UserModel> UserModel { get; set; }
    }
}
