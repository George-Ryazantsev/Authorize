using BookLib_Auth_Mapp_NF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLib_Auth_Mapp_NF.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options) 
        {

        }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<BookChanges> BookChanges { get; set; }

    }
}
