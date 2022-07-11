using Microsoft.EntityFrameworkCore;
using Talamus_V2.Net6.Models;

namespace Talamus_V2.Net6
{
    public class TalamusContext : DbContext
    {
        public TalamusContext(DbContextOptions<TalamusContext> options)
          : base(options)
        {
        }

        public DbSet<Part> Parts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Saving> Savings { get; set; }
        public DbSet<Subsequent> Subsequents { get; set; }
    }
}
