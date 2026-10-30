using Microsoft.EntityFrameworkCore;

namespace Mooo.Models
{
    public class OnlyFarmsDbContext : DbContext
    {
        public OnlyFarmsDbContext(DbContextOptions<OnlyFarmsDbContext> opts) : base(opts) { }
        public DbSet<Animals> Animal { get; set; }
    }
}
